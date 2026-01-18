# 轻量迭代任务清单：修复 session_meta 缺少 cli_version 导致 resume 失败

- [√] Bridge Server：创建会话时写入 `session_meta.payload.cli_version`（Codex 必填）
- [√] Bridge Server：resume 前自动补写缺失的 `cli_version`（并可顺带清理 UTF-8 BOM）
- [√] 更新知识库与变更记录（Bridge Server/API + CHANGELOG）
- [√] 本机构建验证（Bridge Server + WinUI x64 Debug）
- [√] 迁移方案包至 `helloagents/history/2026-01/` 并更新 `helloagents/history/index.md`
