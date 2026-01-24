using System.Text.Json;
using codex_bridge_server.Bridge;

namespace codex_bridge_server.Tests;

public class WebSocketHubEventEnrichmentTests
{
    private static JsonSerializerOptions WebJsonOptions { get; } = new(JsonSerializerDefaults.Web);

    [Fact]
    public void ExtractSessionIdForRun_prefersSessionIdOverThreadId()
    {
        var data = JsonSerializer.SerializeToElement(new { sessionId = " abc ", threadId = "def" }, WebJsonOptions);
        Assert.Equal("abc", WebSocketHub.ExtractSessionIdForRun(data));
    }

    [Fact]
    public void ExtractSessionIdForRun_fallsBackToThreadId()
    {
        var data = JsonSerializer.SerializeToElement(new { threadId = "  def  " }, WebJsonOptions);
        Assert.Equal("def", WebSocketHub.ExtractSessionIdForRun(data));
    }

    [Fact]
    public void ExtractSessionIdForRun_returnsNullWhenMissing()
    {
        var data = JsonSerializer.SerializeToElement(new { runId = "r1" }, WebJsonOptions);
        Assert.Null(WebSocketHub.ExtractSessionIdForRun(data));
    }

    [Fact]
    public void EnsureSessionId_addsSessionIdWhenMissing()
    {
        var envelope = new BridgeEnvelope
        {
            Type = "event",
            Name = "chat.message.delta",
            Data = JsonSerializer.SerializeToElement(new { runId = "r1" }, WebJsonOptions),
        };

        var enriched = WebSocketHub.EnsureSessionId(envelope, sessionId: "s1");

        Assert.True(enriched.Data.TryGetProperty("runId", out var runId));
        Assert.Equal("r1", runId.GetString());
        Assert.True(enriched.Data.TryGetProperty("sessionId", out var sessionId));
        Assert.Equal("s1", sessionId.GetString());
    }

    [Fact]
    public void EnsureSessionId_doesNotOverrideExistingSessionId()
    {
        var envelope = new BridgeEnvelope
        {
            Type = "event",
            Name = "chat.message.delta",
            Data = JsonSerializer.SerializeToElement(new { runId = "r1", sessionId = "existing" }, WebJsonOptions),
        };

        var enriched = WebSocketHub.EnsureSessionId(envelope, sessionId: "s1");

        Assert.True(enriched.Data.TryGetProperty("sessionId", out var sessionId));
        Assert.Equal("existing", sessionId.GetString());
    }
}

