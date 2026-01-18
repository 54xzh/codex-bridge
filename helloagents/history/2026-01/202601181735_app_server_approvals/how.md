# how

## 总体思路

将运行链路从 `codex exec --json` 切换为 `codex app-server`（JSON-RPC over stdio），利用其协议内置的：
- `item/commandExecution/requestApproval`、`item/fileChange/requestApproval` 等审批请求
- `item/*/delta` 流式通知

Bridge Server 负责：
1. 启动 `codex app-server` 子进程并维护 stdio 通道
2. 将 app-server 的通知映射为现有 WS 事件（并新增 delta/approval 事件）
3. 将 app-server 的审批 request 转发为 WS `approval.requested`，等待前端 `approval.respond` 再回写到 app-server

WinUI 负责：
- Chat 页新增 `approvalPolicy`/`effort` 下拉框
- 收到 `approval.requested` 弹出对话框（允许/拒绝/取消任务；可选“本会话内自动允许”）
- 渲染新增 delta 事件，实现流式输出与思考摘要实时刷新

## 协议调整（Bridge WS）

新增 command：
- `approval.respond`：`{ runId, requestId, decision }`

新增 event：
- `approval.requested`：`{ runId, requestId, kind, threadId, turnId, itemId?, reason? }`
- `chat.message.delta`：`{ runId, itemId, delta }`
- `run.command.outputDelta`：`{ runId, itemId, delta }`
- `run.reasoning.delta`：`{ runId, itemId, textDelta }`

## 关键实现点

- app-server 使用行分隔 JSON（每行一个 JSON-RPC message）；服务端基于 `id/method/result/error` 区分 response/request/notification
- 审批 request 需要同步等待 UI 返回：Bridge Server 使用 `TaskCompletionSource` 暂存单个 pending approval
- sandbox 映射：`--sandbox workspace-write` 在非 Git 目录可能回落为只读，因此 turn/start 统一使用 `sandboxPolicy` 显式指定 `workspaceWrite`（writableRoots 取 workingDirectory）

