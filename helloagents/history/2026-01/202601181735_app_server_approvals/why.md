# why

## 背景

现有实现通过 `codex exec --json` 以子进程方式运行 Codex CLI，并解析 JSONL 事件转发到前端。

在该模式下，当 `approvalPolicy=untrusted/on-request` 触发“需要用户批准”的场景时，Codex CLI 在无交互 TTY 的环境会直接按 policy 拒绝命令（表现为 `blocked by policy`），导致 **workspace-write 模式无法在 GUI 内请求/完成审批**。

## 目标

- 支持 Codex 的 **审批请求 → 用户选择 → 回传批准/拒绝** 的完整闭环
- 支持 **流式输出**（assistant message delta、命令输出 delta、思考摘要 delta）
- 前端可配置：
  - `approvalPolicy`（untrusted/on-request/on-failure/never）
  - `effort`（思考深度）

## 非目标

- 不在本次实现中重做全部会话/工作区 API（仅补齐运行链路与审批闭环）
- 不在本次实现中实现二维码配对/多用户鉴权

