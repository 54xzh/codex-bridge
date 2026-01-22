# 任务清单: 连接体验优化（局域网开关与端口持久化）

目录: `helloagents/plan/202601220259_connections_persist/`

---

## 1. WinUI（后端启动参数持久化）
- [√] 1.1 记住“允许局域网连接”开关：下次启动自动恢复，无需每次手动开启
- [√] 1.2 端口持久化：首次选择空闲端口后落盘复用，避免每次启动导致 baseUrl 变化从而重复配对
- [√] 1.3 端口冲突兜底：若持久化端口被占用，自动回退到新端口并继续启动（保留可用性）

## 2. 文档同步
- [√] 2.1 更新 `helloagents/wiki/modules/winui-client.md` 说明（端口/开关持久化行为）
- [√] 2.2 更新 `helloagents/CHANGELOG.md` 记录体验优化

## 3. 验证
- [√] 3.1 `dotnet build codex-bridge -p:Platform=x64` 编译通过
