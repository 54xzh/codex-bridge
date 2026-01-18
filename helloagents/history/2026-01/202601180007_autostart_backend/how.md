# 技术设计: 自动拉起后端并自动连接

## 技术方案

### 核心技术
- **子进程管理:** `System.Diagnostics.Process`
- **端口选择:** `TcpListener(IPAddress.Loopback, 0)` 申请随机可用端口
- **就绪探测:** 轮询 `GET /api/v1/health`

### 实现要点
- **后端可执行文件定位:** 运行时从 `AppContext.BaseDirectory/bridge-server/` 查找 `codex-bridge-server.exe`
- **构建时复制:** WinUI 项目构建后将 `codex-bridge-server` 的输出复制到 `$(OutDir)bridge-server/`，确保运行时可用
- **生命周期:** WinUI 退出时终止后端进程（避免残留后台服务）

## 安全与默认策略
- 默认仅回环（后端本身也会拒绝非回环，除非启用 RemoteEnabled）
- 自动启动仅用于本机；远程模式后续放到 Settings 页显式开关

