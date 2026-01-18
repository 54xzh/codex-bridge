# 技术设计: session_message_filter

## 技术方案

### 核心技术
- 后端：ASP.NET Core（Bridge Server）
- 存储：解析 `%USERPROFILE%\\.codex\\sessions` 下 JSONL

### 实现要点
- 解析 `response_item` + `message` 记录得到 (role,text)。
- 历史过滤：
  - 仅保留 `role=user|assistant`。
  - user 文本清洗：优先抽取 `## My request for Codex:` 段落；对未命中抽取的文本按已知上下文标记过滤（AGENTS.md / environment_context / IDE context 等）。
- 标题提取：基于清洗后的“首条 user 消息”进行截断（约 50 字），避免把上下文噪声当标题。

## API设计
- 无新增路径；行为增强体现在：
  - `GET /api/v1/sessions` 返回的 `title` 更接近真实对话
  - `GET /api/v1/sessions/{sessionId}/messages` 仅返回真实对话消息

## 安全与性能
- **安全:** 不引入外部依赖；不读取除 `~/.codex/sessions` 外的路径；仍沿用既有鉴权。
- **性能:** 清洗与过滤在流式读取 JSONL 时完成；标题提取限制扫描行数，避免大文件全量解析。

## 测试与部署
- 执行 `dotnet build` 验证编译通过。
- 选择包含大量注入上下文的会话进行回放，确认 UI 中不再展示 developer/指令块等内容；VSCode 插件会话能正确抽取 `## My request for Codex:`。

