# 任务清单: fix_codex_path

目录: `helloagents/plan/202601180234_fix_codex_path/`

---

## 1. Bridge Server
- [√] 1.1 在 `codex-bridge-server/Bridge/CodexRunner.cs` 中实现 Windows 下 `codex` 自动定位与启动封装（cmd/bat/ps1 兼容）
- [√] 1.2 在 `codex-bridge-server/Bridge/CodexRunner.cs` 中增加 `workingDirectory` 存在性校验，输出明确错误信息

## 2. 文档更新
- [√] 2.1 更新 `helloagents/wiki/modules/bridge-server.md`（补充 codex 自动定位说明）
- [√] 2.2 更新 `helloagents/CHANGELOG.md` 与 `helloagents/history/index.md`

## 3. 测试
- [√] 3.1 执行 `dotnet build`（后端与 WinUI x64）

