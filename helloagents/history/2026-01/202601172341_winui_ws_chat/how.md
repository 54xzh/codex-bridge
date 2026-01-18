# 技术设计: WinUI WS 客户端与 Chat 页面骨架

## 技术方案

### 核心技术
- **WinUI3:** `NavigationView` + `Frame` 组织页面
- **WebSocket 客户端:** `System.Net.WebSockets.ClientWebSocket`
- **协议模型:** 复用 Bridge Server 的 envelope 结构（`protocolVersion/type/name/id/ts/data`）

### 实现要点
- **连接管理:** 由 `BridgeClient` 维护 WS 生命周期与后台接收循环
- **线程模型:** 接收线程解析消息后，通过 `DispatcherQueue` 更新 UI 绑定集合
- **流式渲染策略:** `run.started` 创建“助手消息占位”，`codex.line` 追加文本，`run.completed/failed/canceled` 追加状态

## API/协议约定（WinUI 侧）
- `command chat.send`: `prompt` + 可选 `workingDirectory`（以及后续扩展字段）
- `command run.cancel`: 取消当前运行
- `event codex.line`: 原样展示（MVP）；后续可解析 JSONL 做更友好的 UI

## 安全与配置
- 默认连接 `ws://127.0.0.1:<port>/ws`；远程模式下可在 UI 输入 Bearer Token（如后端启用）

