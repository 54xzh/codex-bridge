# 任务清单: session_message_filter

目录: `helloagents/plan/202601180203_session_message_filter/`

---

## 1. Bridge Server
- [√] 1.1 在 `codex-bridge-server/Bridge/CodexSessionStore.cs` 中实现会话消息过滤（仅 user/assistant）与 user 文本清洗（抽取 `## My request for Codex:` + 过滤上下文噪声）
- [√] 1.2 会话标题提取使用清洗后的首条 user 消息，避免注入上下文污染标题

## 2. 文档更新
- [√] 2.1 更新 `helloagents/wiki/api.md` 与 `helloagents/wiki/data.md`（记录过滤/清洗行为）
- [√] 2.2 更新 `helloagents/wiki/modules/bridge-server.md` 与 `helloagents/wiki/modules/winui-client.md`
- [√] 2.3 更新 `helloagents/CHANGELOG.md` 与 `helloagents/history/index.md`

## 3. 安全检查
- [√] 3.1 执行安全检查（输入处理、角色过滤、limit 约束）

## 4. 测试
- [√] 4.1 执行 `dotnet build`

