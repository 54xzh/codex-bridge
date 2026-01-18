using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using codex_bridge.Models;
using codex_bridge.Pages;
using codex_bridge.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace codex_bridge
{
    public sealed partial class MainWindow : Window
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly HttpClient _httpClient = new();
        private int _sessionLimit = 20;
        private bool _hasMoreSessions = true;

        public ObservableCollection<SessionSummaryViewModel> RecentSessions { get; } = new();

        public MainWindow()
        {
            InitializeComponent();

            WindowSizing.ApplyStartupSizingAndCenter(this);

            // Load recent sessions on startup
            _ = LoadRecentSessionsAsync();

            Navigate("chat");
        }

        private async Task LoadRecentSessionsAsync(bool append = false)
        {
            try
            {
                await App.BackendServer.EnsureStartedAsync();

                var baseUri = App.BackendServer.HttpBaseUri;
                if (baseUri is null)
                {
                    return;
                }

                var uri = new Uri(baseUri, $"api/v1/sessions?limit={_sessionLimit}");
                using var response = await _httpClient.GetAsync(uri, CancellationToken.None);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(CancellationToken.None);
                var items = JsonSerializer.Deserialize<SessionSummary[]>(json, JsonOptions) ?? Array.Empty<SessionSummary>();

                DispatcherQueue.TryEnqueue(() =>
                {
                    RecentSessions.Clear();

                    foreach (var item in items)
                    {
                        var title = string.IsNullOrWhiteSpace(item.Title) ? "未命名会话" : item.Title;
                        RecentSessions.Add(new SessionSummaryViewModel(item.Id, title, item.CreatedAt, item.Cwd, item.Originator, item.CliVersion));
                    }

                    // Check if there are more sessions to load
                    _hasMoreSessions = items.Length >= _sessionLimit;

                    UpdateSidebarSessions();
                });
            }
            catch
            {
                // Silently ignore loading errors
            }
        }

        private void UpdateSidebarSessions()
        {
            // Find the header index
            int headerIndex = -1;
            for (int i = 0; i < NavView.MenuItems.Count; i++)
            {
                if (NavView.MenuItems[i] is NavigationViewItemHeader)
                {
                    headerIndex = i;
                    break;
                }
            }

            if (headerIndex < 0)
            {
                return;
            }

            // Remove existing session items (after header)
            while (NavView.MenuItems.Count > headerIndex + 1)
            {
                NavView.MenuItems.RemoveAt(headerIndex + 1);
            }

            // Add new session items
            foreach (var session in RecentSessions)
            {
                var item = new NavigationViewItem
                {
                    Tag = $"session:{session.Id}",
                    Content = new TextBlock
                    {
                        Text = session.Title,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                    },
                    Icon = new SymbolIcon(Symbol.Message),
                };
                ToolTipService.SetToolTip(item, session.Subtitle);
                NavView.MenuItems.Add(item);
            }

            // Add "Load More" button if there are more sessions
            if (_hasMoreSessions)
            {
                var loadMoreItem = new NavigationViewItem
                {
                    Tag = "loadmore",
                    Content = new TextBlock
                    {
                        Text = "加载更多...",
                        Opacity = 0.7,
                    },
                    Icon = new SymbolIcon(Symbol.More),
                    SelectsOnInvoked = false,
                };
                NavView.MenuItems.Add(loadMoreItem);
            }
        }

        public async Task RefreshRecentSessionsAsync()
        {
            _sessionLimit = 20;
            _hasMoreSessions = true;
            await LoadRecentSessionsAsync();
        }

        private async void HandleLoadMore()
        {
            _sessionLimit += 10;
            await LoadRecentSessionsAsync();
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is not NavigationViewItem item)
            {
                return;
            }

            var tag = item.Tag?.ToString();
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            if (tag == "loadmore")
            {
                HandleLoadMore();
                return;
            }

            if (tag == "newchat")
            {
                HandleNewChat();
                return;
            }

            if (tag.StartsWith("session:"))
            {
                var sessionId = tag.Substring("session:".Length);
                HandleSelectSession(sessionId);
                return;
            }

            Navigate(tag);
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            // Only handle footer items via selection change
            if (args.SelectedItem is not NavigationViewItem item)
            {
                return;
            }

            var tag = item.Tag?.ToString();
            if (string.IsNullOrEmpty(tag) || tag == "newchat" || tag.StartsWith("session:"))
            {
                return;
            }

            Navigate(tag);
        }

        private void HandleNewChat()
        {
            // Clear current session to start fresh
            App.SessionState.CurrentSessionId = null;
            Navigate("chat");

            // Refresh the ChatPage if already on it
            if (ContentFrame.CurrentSourcePageType == typeof(ChatPage))
            {
                ContentFrame.Navigate(typeof(ChatPage));
            }
        }

        private void HandleSelectSession(string sessionId)
        {
            var session = FindSessionById(sessionId);
            if (session is not null)
            {
                App.SessionState.CurrentSessionCwd = session.Cwd;
                App.SessionState.CurrentSessionId = session.Id;
            }

            Navigate("chat");

            // Force refresh if already on chat page
            if (ContentFrame.CurrentSourcePageType == typeof(ChatPage))
            {
                ContentFrame.Navigate(typeof(ChatPage));
            }
        }

        private SessionSummaryViewModel? FindSessionById(string sessionId)
        {
            foreach (var session in RecentSessions)
            {
                if (session.Id == sessionId)
                {
                    return session;
                }
            }
            return null;
        }

        private void Navigate(string? tag)
        {
            var target = tag switch
            {
                "chat" => typeof(ChatPage),
                "sessions" => typeof(SessionsPage),
                "diff" => typeof(DiffPage),
                "settings" => typeof(SettingsPage),
                _ => typeof(ChatPage),
            };

            if (ContentFrame.CurrentSourcePageType == target && tag != "chat")
            {
                return;
            }

            ContentFrame.Navigate(target);
        }
    }
}
