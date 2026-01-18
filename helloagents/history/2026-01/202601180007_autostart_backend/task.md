# 任务清单: WinUI 自动拉起后端并自动连接

目录: `helloagents/history/2026-01/202601180007_autostart_backend/`

---

## 1. WinUI Client
- [√] 1.1 新增 `BackendServerManager`：启动 `codex-bridge-server.exe`、随机端口、health 探测、退出清理，验证 why.md#需求-一键启动-场景-打开-winui-即可聊天
- [√] 1.2 在 `App.xaml.cs` 中启动后端并在窗口关闭时停止，验证 why.md#需求-一键启动-场景-打开-winui-即可聊天
- [√] 1.3 Chat 页面默认自动连接（无需填写 WS 地址），并显示连接状态，验证 why.md#需求-一键启动-场景-打开-winui-即可聊天

## 2. 构建集成
- [√] 2.1 WinUI 构建后复制后端输出到 `$(OutDir)bridge-server/`，确保运行时可启动

## 3. 文档更新
- [√] 3.1 更新知识库模块说明（WinUI/Bridge Server）
- [√] 3.2 更新 `helloagents/CHANGELOG.md`

## 4. 测试
- [√] 4.1 WinUI x64 Debug 编译验证
- [√] 4.2 后端 Debug 编译验证
