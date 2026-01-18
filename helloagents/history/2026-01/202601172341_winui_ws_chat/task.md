# 任务清单: WinUI WS 客户端与 Chat 页面骨架

目录: `helloagents/history/2026-01/202601172341_winui_ws_chat/`

---

## 1. WinUI Client
- [√] 1.1 新增 Bridge 协议模型与 `BridgeClient`（WS 连接、收发、事件回调），验证 why.md#需求-winui-聊天与流式输出-场景-连接后端并发送消息
- [√] 1.2 主窗口增加导航（Chat/Sessions/Diff/Settings）并创建页面骨架，验证 why.md#需求-winui-聊天与流式输出-场景-连接后端并发送消息
- [√] 1.3 Chat 页面打通：连接 WS、发送 `chat.send`、渲染 `codex.line`、支持 `run.cancel`，验证 why.md#需求-winui-聊天与流式输出-场景-取消当前运行
- [√] 1.4 补齐协议支持：后端广播 `chat.message`，并支持 `chat.send` 的 `model/sandbox` 扩展字段

## 2. 文档更新
- [√] 2.1 更新知识库协议说明（`helloagents/wiki/api.md`、`helloagents/wiki/modules/winui-client.md`）
- [√] 2.2 更新 `helloagents/CHANGELOG.md`

## 3. 测试
- [√] 3.1 运行 WinUI 编译验证（至少 x64 Debug）
