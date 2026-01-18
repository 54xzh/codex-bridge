# 任务清单: WinUI 窗口初始大小/最小尺寸/居中

目录: `helloagents/plan/202601190247_window_init_size_center/`

---

## 1. WinUI 窗口行为
- [√] 1.1 在 `codex-bridge/MainWindow.xaml.cs` 中调整启动窗口逻辑：初始大小增大，并与最小尺寸一致
- [√] 1.2 在 `codex-bridge/WindowSizing.cs` 中实现 WinUI 3 窗口最小尺寸限制（WM_GETMINMAXINFO）与屏幕居中

## 2. 文档更新
- [√] 2.1 更新 `helloagents/wiki/modules/winui-client.md`：补充窗口初始尺寸/最小尺寸/居中的行为约定，并添加变更历史条目
- [√] 2.2 更新 `helloagents/CHANGELOG.md`：记录本次窗口体验调整
- [√] 2.3 更新 `helloagents/history/index.md`：记录本次变更索引

## 3. 构建验证
- [√] 3.1 执行 `dotnet build` 验证编译通过
  > 备注: 由于 MSIX 打包限制，构建需指定平台，例如 `dotnet build -p:Platform=x64`（AnyCPU 可能失败）。
