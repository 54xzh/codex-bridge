# 任务清单（轻量迭代）

- [√] 后端：扩展 `/api/v1/sessions/{sessionId}/messages`，回放 trace（agent_reasoning + tool calls）
- [√] 后端：TraceEntry 解析与排序（按 JSONL 顺序）
- [√] 前端：Chat 页按“Trace → 最终回答”顺序展示（思考/命令/回答时间线）
- [√] 前端：agent_reasoning 的 `**标题**` 作为摘要逐条展示
- [√] 文档：更新 `helloagents/wiki/*` 与 `helloagents/CHANGELOG.md`
- [√] 验证：`dotnet build`（后端/WinUI）通过
- [√] 迁移：将方案包迁移至 `helloagents/history/2026-01/` 并更新 `helloagents/history/index.md`
