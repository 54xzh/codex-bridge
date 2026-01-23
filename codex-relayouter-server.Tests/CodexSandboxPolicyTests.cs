using System.Reflection;

namespace codex_bridge_server.Tests;

public sealed class CodexSandboxPolicyTests
{
    [Fact]
    public void WorkspaceWriteSandboxPolicy_EnablesNetworkAccess()
    {
        var method = typeof(codex_bridge_server.Bridge.CodexAppServerRunner).GetMethod(
            "CreateSandboxPolicy",
            BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);

        var policy = method!.Invoke(null, new object?[] { "workspace-write", "C:\\work" });
        Assert.NotNull(policy);

        var policyType = policy!.GetType();

        var typeValue = policyType.GetProperty("type")?.GetValue(policy) as string;
        Assert.Equal("workspaceWrite", typeValue);

        var networkValue = policyType.GetProperty("networkAccess")?.GetValue(policy);
        Assert.IsType<bool>(networkValue);
        Assert.True((bool)networkValue!);

        var rootsValue = policyType.GetProperty("writableRoots")?.GetValue(policy);
        Assert.IsType<string[]>(rootsValue);
        Assert.Equal(new[] { "C:\\work" }, (string[])rootsValue!);
    }
}

