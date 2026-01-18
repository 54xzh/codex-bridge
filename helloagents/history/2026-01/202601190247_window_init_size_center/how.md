# 技术设计: WinUI 窗口初始大小/最小尺寸/居中

## 技术方案
### 核心技术
- WinUI 3（Windows App SDK）`AppWindow`：设置窗口尺寸/位置
- Win32 `WM_GETMINMAXINFO`：限制窗口最小可拖拽尺寸（MinTrackSize）

### 实现要点
- 将窗口 sizing/居中逻辑集中在 `codex-bridge/WindowSizing.cs`，主窗口构造函数只调用一次 `ApplyStartupSizingAndCenter`。
- 初始尺寸策略：
  - 以基准值 `1440x900` 为目标；
  - 在当前显示器工作区（`DisplayArea.WorkArea`）内按 95% 上限自适应，避免小屏溢出；
  - 设定最低兜底（`640x480`）防止异常屏幕信息导致过小。
- 最小尺寸策略：
  - 使用 `SetWindowLongPtrW(GWL_WNDPROC)` 安装 WndProc 钩子；
  - 在 `WM_GETMINMAXINFO` 时写入 `MinTrackSize = 初始尺寸`，确保用户无法缩到小于初始尺寸。
- 居中策略：
  - 以工作区矩形计算 `(workArea - size)/2`，并使用 `AppWindow.Move` 移动到居中位置。

## 架构决策 ADR
### ADR-001: 使用 Win32 消息钩子限制最小尺寸
**上下文:** WinUI 3 目前缺少直接的 Window.MinWidth/MinHeight 行为等价物（可拖拽最小尺寸约束）。
**决策:** 使用 `WM_GETMINMAXINFO` 方式设置 `MinTrackSize`。
**理由:** 兼容性好、实现范围小、只影响窗口 sizing 行为。
**替代方案:** 仅在窗口被缩小时强制回弹到最小尺寸 → 拒绝原因: 体验差、容易闪烁且逻辑复杂。
**影响:** 引入少量 Win32 interop；如未来自定义 WndProc/窗口多实例，需要统一管理钩子。

## 安全与性能
- **安全:** 不涉及外部输入、权限或敏感信息；仅使用本地 Win32 API。
- **性能:** 仅在窗口 sizing 相关消息触发时执行常量级逻辑，影响可忽略。

## 测试与部署
- **测试:** 通过 `dotnet build -p:Platform=x64` 验证编译；运行应用手动验证：启动居中、窗口不可缩小到小于初始尺寸。
- **部署:** 无额外部署步骤。
