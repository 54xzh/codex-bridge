# 任务清单: WinUI 同步 model/effort 到 config.toml（轻量迭代）

目录: `helloagents/plan/202601182329_model_effort_config_sync/`

---

## 1. config.toml 读写
- [√] 1.1 新增 `CodexCliConfig`：读取 `~/.codex/config.toml` 的 `model` 与 `model_reasoning_effort`
- [√] 1.2 支持更新/移除上述键，尽量保留其他内容，写入 UTF-8（无 BOM）

## 2. 应用接入（ConnectionService）
- [√] 2.1 启动时读取并填充 `Model` / `Effort`
- [√] 2.2 `Model` / `Effort` 变更时 debounce 写回 `config.toml`（避免设置页输入频繁写入）

## 3. UI（ChatPage）
- [√] 3.1 ChatPage 加载时根据 ConnectionService 刷新 Workspace/Sandbox/Model/Effort/Approval 的显示文本

## 4. 文档
- [√] 4.1 更新 `helloagents/wiki/modules/winui-client.md`（说明与 `config.toml` 的关联）
- [√] 4.2 更新 `helloagents/CHANGELOG.md`

## 5. 验证
- [√] 5.1 `dotnet build` 解决方案通过
