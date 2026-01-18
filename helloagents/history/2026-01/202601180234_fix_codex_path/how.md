# 技术设计: fix_codex_path

## 技术方案

### 核心技术
- 后端：ASP.NET Core（Bridge Server）
- 进程调用：`System.Diagnostics.Process`

### 实现要点
- Windows 下解析 `Bridge:Codex:Executable`：
  - 若为路径：校验存在并按扩展名选择启动方式（exe 直启；cmd/bat 走 `cmd.exe /c`；ps1 走 `powershell.exe -File`）。
  - 若为名称：按优先级在常见目录中查找（`%USERPROFILE%\\AppData\\Roaming\\npm`、`%USERPROFILE%\\.cargo\\bin`、WindowsApps 与 PATH）。
- 在设置 `ProcessStartInfo.WorkingDirectory` 前校验目录存在性，失败时抛出明确异常供上层提示。

## 测试与部署
- 运行 `dotnet build`
- 在 WinUI Chat 页设置工作区（如 `D:\\TWRP`）发送消息，确认不再出现 “找不到 codex” 的启动错误

