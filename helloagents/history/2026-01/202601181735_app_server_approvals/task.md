# task

- [√] 后端：引入 `CodexAppServerRunner`（启动 `codex app-server`，turn/start 流式转发）
- [√] 后端：WS 协议新增 `approval.requested`/`approval.respond` 并实现等待回传
- [√] 后端：新增 delta 事件（assistant/command/reasoning）并广播
- [√] 前端：Chat 页新增 `approvalPolicy`/`effort` 下拉框并随 `chat.send` 发送
- [√] 前端：实现 `approval.requested` 弹窗并回传 `approval.respond`
- [√] 前端：支持 delta 渲染（assistant message / command output / reasoning summary）
- [√] 验证：`dotnet build`（server + WinUI x64）通过
- [√] 文档：更新 `wiki/api.md`、模块文档、CHANGELOG
- [√] 收尾：迁移方案包到 `history/` 并更新索引
