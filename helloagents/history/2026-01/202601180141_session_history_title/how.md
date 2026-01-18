# 技术设计: session_history_title

## 技术方案

### 核心技术
- 后端：ASP.NET Core（Bridge Server）
- 前端：WinUI 3
- 存储：复用 Codex CLI 本地 `%USERPROFILE%\\.codex\\sessions`（JSONL）

### 实现要点
- Bridge Server 扫描会话 JSONL：首行读取 `session_meta`；后续行筛选 `type=response_item` 且 `payload.type=message` 的记录。
- 会话标题：优先提取首条 `role=user` 的 message 文本，规范化空白并截断到约 50 字（超出则 49+“…”）。
- 历史消息：新增 HTTP API 获取指定会话的历史消息，支持 `limit` 并返回末尾 N 条；WinUI Chat 页在已选择会话时自动调用并渲染。

## API设计

### [GET] /api/v1/sessions
- **变更:** 返回模型新增 `title` 字段（非空，带回退策略）。

### [GET] /api/v1/sessions/{sessionId}/messages
- **请求:** `?limit=200(optional)`
- **响应:** `[{ role, text, kind? }, ...]`

## 安全与性能
- **安全:** 继承既有鉴权（默认回环；远程开启后 Bearer Token）。
- **性能:** 标题提取限制扫描行数；历史接口支持 limit 并返回末尾 N 条，避免一次性拉取过大内容。

## 测试与部署
- 运行 `dotnet build` 验证编译通过。
- WinUI 选择会话后进入 Chat 页，确认历史消息可见；会话列表标题按首条 user 消息截断显示。

