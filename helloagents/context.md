# 项目上下文

## 项目简介
`codex-bridge` 是 Codex CLI 的桌面/移动端壳：由 Windows 客户端、Android 客户端与本机 Bridge Server 组成，通过 HTTP/WS（JSON）实现会话与流式事件的同步与控制。

## 代码目录
- `codex-bridge-android/`：Android 客户端（Kotlin / Jetpack Compose / Material3 / Navigation-Compose）
- `codex-bridge/`：Windows 客户端（WinUI 3 / .NET 8）
- `codex-bridge-server/`：Bridge Server（ASP.NET Core / .NET 8）
- `codex-bridge-common/`：公共模型/协议（如有）

## 关键约束
- Android `minSdk=29`，单 Activity + Compose 导航
- 远程访问默认关闭，配对后使用 `deviceToken` 认证

