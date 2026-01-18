# 轻量迭代任务清单：过滤 Codex `--json` 事件噪音

- [√] Bridge Server：将 `codex exec --json` 输出解析为 `session.created` 与 `chat.message(role=assistant)`，避免前端显示原始 JSON
- [√] WinUI Chat 页：接收 `chat.message(role=assistant)` 时更新当前 run 的占位消息（避免空白/重复）
- [√] 更新知识库与变更记录（Protocol/Bridge Server + CHANGELOG）
- [√] 本机构建验证（Bridge Server + WinUI x64 Debug）
- [√] 迁移方案包至 `helloagents/history/2026-01/` 并更新 `helloagents/history/index.md`
