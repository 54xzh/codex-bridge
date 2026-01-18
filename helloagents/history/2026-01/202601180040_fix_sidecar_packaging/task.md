# 任务清单: 修复 WinUI 打包/部署时后端 Sidecar 缺失

目录: `helloagents/history/2026-01/202601180040_fix_sidecar_packaging/`

---

## 1. 构建/部署
- [√] 1.1 调整 WinUI 构建目标：将 `codex-bridge-server` 输出同时复制到 `$(OutDir)bridge-server/` 与 `$(OutDir)AppX/bridge-server/`，确保打包部署后可被 `AppContext.BaseDirectory/bridge-server/` 找到

## 2. 文档更新
- [√] 2.1 更新知识库模块说明（WinUI/Bridge Server）
- [√] 2.2 更新 `helloagents/CHANGELOG.md`

## 3. 测试
- [√] 3.1 WinUI x64 Debug 编译验证
- [√] 3.2 验证 `AppX/bridge-server/codex-bridge-server.exe` 生成
