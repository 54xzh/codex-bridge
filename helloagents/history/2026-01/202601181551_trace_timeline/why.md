# 为什么要做“Trace 时间线”

## 背景
当前 Chat 页仅能稳定展示最终回答，且思考/命令等过程信息在 UI 中不可见或展示不符合预期；对已有 `~/.codex/sessions` 会话的回放也无法展示这些过程信息。

## 目标
- 以时间顺序展示一次响应中的：思考过程、使用的命令、最终回答
- 支持从 `~/.codex/sessions/*.jsonl` 回放 `agent_reasoning` 与工具调用（如 `shell_command`）记录
- `agent_reasoning` 中 `**...**` 作为每条摘要标题，逐条展示而非合并

## 成功标准
- Chat 页在一次运行中可按发生顺序展示 trace（思考/命令）并在最后展示最终回答
- 打开历史会话时可回放对应的 trace（若会话文件中存在）
- `dotnet build`（后端与 WinUI）通过

