# 如何实现

## 总体思路
- 将“思考/命令”统一抽象为 **TraceEntry**，并与一次助手回复关联
- UI 侧以 **TraceEntry 列表 + 最终回答** 的顺序渲染，确保时间顺序直观
- 回放侧从 `~/.codex/sessions` 的 JSONL 中解析：
  - `event_msg.payload.type=agent_reasoning` → TraceEntry(kind=reasoning)
  - `response_item.payload.type=function_call`/`function_call_output` → TraceEntry(kind=command)
  - `response_item.payload.type=message` → ChatMessage（user/assistant）

## 关键规则
- TraceEntry 按 JSONL 行出现顺序追加，保证时间顺序
- `agent_reasoning.text`：
  - 若以 `**标题**` 开头：标题作为摘要，剩余文本作为详情
  - 否则：取首行作为摘要（超长截断），全文作为详情
- 工具调用（以 `shell_command` 为主）：
  - 优先从 `arguments` JSON 提取 `command`
  - `function_call_output.output` 作为可展开输出
- 最终回答（assistant message）始终显示在该次 trace 之后

## API 变更
- 复用既有 `/api/v1/sessions/{sessionId}/messages`
- 扩展响应结构：message 增加可选字段 `trace: TraceEntry[]`

