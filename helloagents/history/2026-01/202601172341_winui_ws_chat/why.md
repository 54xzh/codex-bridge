# 变更提案: WinUI 端接入 Bridge Server（WS 聊天与流式输出）

## 需求背景

当前仓库已具备 Bridge Server 的最小骨架（health + WebSocket + 驱动 `codex exec --json`）。为实现可用的桌面端体验与后续 Android 同步，需要让 WinUI3 前端接入 `/ws` 通道，完成聊天发送、流式输出渲染与取消操作，并搭好页面骨架（Chat/Sessions/Diff/Settings）。

## 变更内容
1. 新增 WinUI 侧 Bridge 协议模型与 WebSocket 客户端（收发 envelope）
2. 增加主窗口导航与页面骨架（Chat/Sessions/Diff/Settings）
3. Chat 页面打通：连接 WS、发送 `chat.send`、渲染 `codex.line` 流式事件、支持 `run.cancel`

## 影响范围
- **模块:** WinUI Client、Protocol
- **文件:** WinUI 项目新增页面、服务类与模型文件；更新知识库文档与 Changelog

## 核心场景

### 需求: WinUI 聊天与流式输出
**模块:** WinUI Client

#### 场景: 连接后端并发送消息
用户输入服务端地址（默认本机），连接成功后发送 prompt，服务端广播运行事件，WinUI 按事件增量渲染输出。

#### 场景: 取消当前运行
用户点击取消，WinUI 发送 `run.cancel` 命令，服务端终止当前运行并广播取消事件。

## 风险评估
- **风险:** WS 消息解析失败导致 UI 无响应或崩溃
- **缓解:** 客户端侧做 JSON 解析容错与连接状态管理；UI 更新通过 DispatcherQueue 串行化

