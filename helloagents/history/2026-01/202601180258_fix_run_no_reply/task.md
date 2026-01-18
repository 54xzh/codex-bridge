# 任务清单: fix_run_no_reply

目录: `helloagents/plan/202601180258_fix_run_no_reply/`

---

## 1. Bridge Server
- [√] 1.1 在 `codex-bridge-server/Bridge/WebSocketHub.cs` 中支持 `skipGitRepoCheck` 参数，并将 exitCode 非 0 视为失败（输出 `run.failed`）
- [√] 1.2 在 `codex-bridge-server/Bridge/CodexRunner.cs` 与 `codex-bridge-server/Bridge/CodexRunRequest.cs` 中支持 per-run 的 `SkipGitRepoCheck`

## 2. WinUI Client
- [√] 2.1 在 `codex-bridge/Pages/ChatPage.xaml` 增加“跳过 Git 检查”开关
- [√] 2.2 在 `codex-bridge/Pages/ChatPage.xaml.cs` 中发送 `skipGitRepoCheck`，并优化完成/失败状态显示（含 exitCode 与失败消息回填）

## 3. 文档更新
- [√] 3.1 更新 `helloagents/wiki/api.md`（`chat.send` 参数）
- [√] 3.2 更新 `helloagents/wiki/modules/bridge-server.md` 与 `helloagents/wiki/modules/winui-client.md`
- [√] 3.3 更新 `helloagents/CHANGELOG.md` 与 `helloagents/history/index.md`

## 4. 测试
- [√] 4.1 执行 `dotnet build`（后端 + WinUI x64）

