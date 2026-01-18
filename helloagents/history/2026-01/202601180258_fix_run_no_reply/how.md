# 技术设计: fix_run_no_reply

## 技术方案

### 核心技术
- 后端：ASP.NET Core（Bridge Server）
- 前端：WinUI 3
- 通信：WebSocket 命令/事件

### 实现要点
- Bridge Server：
  - `chat.send` 新增解析 `skipGitRepoCheck`（bool），并透传到 Codex 进程参数 `--skip-git-repo-check`。
  - 当 Codex 进程 exitCode 非 0 时发送 `run.failed`（携带 `exitCode` 与 stderr 摘要），不再误报 `run.completed`。
- WinUI：
  - Chat 页新增“跳过 Git 检查”复选框，并在 `chat.send` 中携带 `skipGitRepoCheck`。
  - `run.completed/run.failed` 状态展示包含 exitCode；失败时将错误文本写入对应 assistant 消息（避免空白）。

## API/协议变更
- WS command `chat.send` 新增字段：`skipGitRepoCheck: bool`（默认 false）

## 测试与验证
- `dotnet build`（后端 + WinUI x64）
- 在非 Git 目录下：
  - 未勾选：应收到失败提示（并可见原因）
  - 勾选后：Codex CLI 不再因 Git 检查拒绝启动

