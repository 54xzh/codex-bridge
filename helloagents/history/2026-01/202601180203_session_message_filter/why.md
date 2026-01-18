# 变更提案: session_message_filter

## 需求背景
当前从 Codex CLI 会话文件回放历史时，可能会把 harness/插件注入的上下文一并当作“用户消息”展示（如 developer 权限说明、AGENTS.md 指令块、environment_context、IDE 上下文等），导致会话内容噪声很大，影响阅读与多端同步的一致体验。

同时，VSCode 的 Codex 插件会把真实用户请求放在 `## My request for Codex:` 段落中，需要在回放与标题提取时正确抽取。

## 变更内容
1. 会话历史回放仅展示 `user` 与 `assistant` 两类角色消息，过滤 developer/system/tool 等非对话内容。
2. 对 user 文本进行清洗：
   - 若包含 `## My request for Codex:`，仅保留该段之后的真实用户请求。
   - 过滤 AGENTS.md 指令、environment_context、IDE 上下文等非用户真实输入内容。
3. 会话标题提取同样使用清洗后的“首条 user 消息”，避免把注入上下文当作标题。

## 影响范围
- **模块:** Bridge Server、WinUI Client
- **API:** 不新增接口；但 `/api/v1/sessions` 的 title 与 `/api/v1/sessions/{sessionId}/messages` 的返回内容更“干净”
- **数据:** 仍复用 `%USERPROFILE%\\.codex\\sessions`（JSONL）

## 核心场景

### 需求: 会话历史只显示真实对话
**模块:** Bridge Server / WinUI Client
会话回放不应展示 developer/harness/环境上下文等内容。

#### 场景: 打开历史会话查看内容
用户点击会话进入 Chat 页查看历史。
- 预期结果：历史列表仅包含 user/assistant 的真实对话文本。

### 需求: VSCode 插件会话正确抽取用户请求
**模块:** Bridge Server
当 user 消息包含 `## My request for Codex:` 段落时，应只展示该段落中的真实用户请求。

#### 场景: 回放 VSCode 插件产生的历史
用户打开由 VSCode 插件产生的会话。
- 预期结果：user 消息仅展示 `## My request for Codex:` 下的内容；会话标题同样来自该真实请求。

## 风险评估
- **风险:** 过滤规则过严导致误删少量特殊输入
- **缓解:** 过滤以已知上下文标记为主（environment_context/AGENTS/IDE 上下文等），并优先使用 `## My request for Codex:` 明确边界

