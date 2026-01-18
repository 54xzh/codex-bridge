// WindowSizing：集中管理 WinUI 3 主窗口的初始尺寸、最小尺寸约束与屏幕居中逻辑。
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Graphics;

namespace codex_bridge;

internal static class WindowSizing
{
    private const int BaselineWidth = 1800;
    private const int BaselineHeight = 1080;

    private static readonly Dictionary<nint, MinSizeHook> MinSizeHooks = new();

    public static void ApplyStartupSizingAndCenter(Window window)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
        var workArea = displayArea.WorkArea;

        var initialSize = CalculateInitialSize(workArea, BaselineWidth, BaselineHeight);
        EnsureMinSizeHook(hwnd, initialSize);

        appWindow.Resize(initialSize);
        CenterInWorkArea(appWindow, workArea, initialSize);
    }

    private static SizeInt32 CalculateInitialSize(RectInt32 workArea, int baselineWidth, int baselineHeight)
    {
        var maxWidth = (int)Math.Floor(workArea.Width * 0.95);
        var maxHeight = (int)Math.Floor(workArea.Height * 0.95);

        var width = Math.Min(baselineWidth, maxWidth);
        var height = Math.Min(baselineHeight, maxHeight);

        width = Math.Max(640, width);
        height = Math.Max(480, height);

        return new SizeInt32(width, height);
    }

    private static void CenterInWorkArea(AppWindow appWindow, RectInt32 workArea, SizeInt32 size)
    {
        var x = workArea.X + Math.Max(0, (workArea.Width - size.Width) / 2);
        var y = workArea.Y + Math.Max(0, (workArea.Height - size.Height) / 2);
        appWindow.Move(new PointInt32(x, y));
    }

    private static void EnsureMinSizeHook(nint hwnd, SizeInt32 minSize)
    {
        if (MinSizeHooks.TryGetValue(hwnd, out var existingHook))
        {
            existingHook.MinSize = minSize;
            return;
        }

        var hook = new MinSizeHook(hwnd, minSize);
        if (hook.IsInstalled)
        {
            MinSizeHooks[hwnd] = hook;
        }
    }

    private sealed class MinSizeHook
    {
        private const int GwlWndProc = -4;
        private const uint WmGetMinMaxInfo = 0x0024;

        private readonly nint _hwnd;
        private readonly WndProc _callback;
        private readonly nint _oldWndProc;

        public bool IsInstalled => _oldWndProc != 0;

        public SizeInt32 MinSize { get; set; }

        public MinSizeHook(nint hwnd, SizeInt32 minSize)
        {
            _hwnd = hwnd;
            MinSize = minSize;

            _callback = WindowProc;
            var newWndProc = Marshal.GetFunctionPointerForDelegate(_callback);
            _oldWndProc = SetWindowLongPtrW(_hwnd, GwlWndProc, newWndProc);
        }

        private nint WindowProc(nint hwnd, uint message, nint wParam, nint lParam)
        {
            if (message == WmGetMinMaxInfo && lParam != 0)
            {
                var minMaxInfo = Marshal.PtrToStructure<MinMaxInfo>(lParam);
                minMaxInfo.MinTrackSize.X = MinSize.Width;
                minMaxInfo.MinTrackSize.Y = MinSize.Height;
                Marshal.StructureToPtr(minMaxInfo, lParam, false);
                return 0;
            }

            return CallWindowProcW(_oldWndProc, hwnd, message, wParam, lParam);
        }

        private delegate nint WndProc(nint hwnd, uint message, nint wParam, nint lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MinMaxInfo
        {
            public Point Reserved;
            public Point MaxSize;
            public Point MaxPosition;
            public Point MinTrackSize;
            public Point MaxTrackSize;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        private static extern nint SetWindowLongPtrW(nint hWnd, int nIndex, nint dwNewLong);

        [DllImport("user32.dll", EntryPoint = "CallWindowProcW")]
        private static extern nint CallWindowProcW(nint lpPrevWndFunc, nint hWnd, uint msg, nint wParam, nint lParam);
    }
}
