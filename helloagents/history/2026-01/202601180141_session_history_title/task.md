# 任务清单: session_history_title

目录: `helloagents/plan/202601180141_session_history_title/`

---

## 1. Bridge Server
- [√] 1.1 在 `codex-bridge-server/Bridge/CodexSessionStore.cs` 中提取会话标题：首条 user 消息截断约 50 字，回退到 cwd/id
- [√] 1.2 在 `codex-bridge-server/Controllers/SessionsController.cs` 中新增 `GET /api/v1/sessions/{sessionId}/messages` 接口
- [√] 1.3 在 `codex-bridge-server/Bridge/CodexSessionSummary.cs` 中为会话列表增加 `Title` 字段

## 2. WinUI Client
- [√] 2.1 在 `codex-bridge/Pages/ChatPage.xaml.cs` 中实现进入会话后自动加载历史消息并展示
- [√] 2.2 在 `codex-bridge/Pages/SessionsPage.xaml.cs` 与 `codex-bridge/ViewModels/SessionSummaryViewModel.cs` 中使用后端返回的 title 展示会话标题

## 3. 安全检查
- [√] 3.1 执行安全检查（鉴权复用、输入校验、limit 限制、防止路径注入）

## 4. 文档更新
- [√] 4.1 更新 `helloagents/wiki/api.md`（新增 messages 接口、sessions 返回 title）
- [√] 4.2 更新 `helloagents/wiki/data.md`（新增 title/SessionMessage）
- [√] 4.3 更新 `helloagents/wiki/modules/bridge-server.md` 与 `helloagents/wiki/modules/winui-client.md`
- [√] 4.4 更新 `helloagents/CHANGELOG.md` 与 `helloagents/history/index.md`

## 5. 测试
- [√] 5.1 执行 `dotnet build`（全项目）

