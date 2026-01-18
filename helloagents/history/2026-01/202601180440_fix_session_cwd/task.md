# 轻量迭代任务清单：修复会话文件缺少 cwd 导致 resume 失败

- [√] 修复会话创建：写入 `session_meta.payload.cwd`（必填且校验目录存在）
- [√] 兼容旧会话：resume 前如缺少 cwd，使用 workingDirectory 自动补写（或给出明确错误）
- [√] WinUI 创建会话：cwd 改为必填（或回退使用当前会话 cwd）
- [√] 更新知识库与变更记录（Bridge Server/WinUI Client + CHANGELOG）
- [√] 本机构建验证（Bridge Server + WinUI x64 Debug）
- [√] 迁移方案包至 `helloagents/history/2026-01/` 并更新 `helloagents/history/index.md`
