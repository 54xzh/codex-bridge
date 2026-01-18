# 任务清单: 会话管理（列表/创建/重启保留）

目录: `helloagents/history/2026-01/202601180102_session_management/`

---

## 1. Bridge Server（会话 API + 扫描）
- [√] 1.1 新增会话扫描服务：从 `%USERPROFILE%\\.codex\\sessions` 读取最近会话元数据（仅读首行 `session_meta`），验证 why.md#核心场景-需求-会话列表-场景-打开会话页看到最近会话
- [√] 1.2 新增 `GET /api/v1/sessions` 返回会话列表，验证 why.md#核心场景-需求-会话列表-场景-打开会话页看到最近会话
- [√] 1.3 新增 `POST /api/v1/sessions`（写入最小 `session_meta` 并返回 `sessionId`），验证 why.md#核心场景-需求-创建会话-场景-新建会话并进入聊天

## 2. Bridge Server（聊天绑定会话）
- [√] 2.1 扩展 WS `chat.send` 支持 `sessionId`；`sessionId` 非空时调用 `codex exec resume --json <sessionId>`，验证 why.md#核心场景-需求-重启后会话仍在-场景-选择历史会话继续对话
- [√] 2.2 解析 `session_meta` 并广播 `session.created { runId, sessionId }`，用于新会话首轮对话拿到 `sessionId`

## 3. WinUI Client（会话页）
- [√] 3.1 实现 `SessionsPage`：拉取 `/api/v1/sessions` 并展示列表/刷新，验证 why.md#核心场景-需求-会话列表-场景-打开会话页看到最近会话
- [√] 3.2 实现“新建会话”按钮（调用 `POST /api/v1/sessions` 创建 `session_meta` 并跳转聊天），验证 why.md#核心场景-需求-创建会话-场景-新建会话并进入聊天
- [√] 3.3 Chat 页支持绑定当前会话：发送 `chat.send` 携带 `sessionId`；收到 `session.created` 后更新当前会话并提示/可刷新列表，验证 why.md#核心场景-需求-重启后会话仍在-场景-选择历史会话继续对话

## 4. 安全检查
- [√] 4.1 执行安全检查（按G9：输入验证、敏感信息处理、鉴权一致性、避免泄露用户目录信息）

## 5. 文档更新
- [√] 5.1 更新 `helloagents/wiki/api.md`（标注已实现会话接口与 WS 扩展）
- [√] 5.2 更新 `helloagents/wiki/data.md`（补齐 SessionSummary 与 `.codex/sessions` 说明）
- [√] 5.3 更新 `helloagents/wiki/modules/winui-client.md` 与 `helloagents/wiki/modules/bridge-server.md`
- [√] 5.4 更新 `helloagents/CHANGELOG.md`

## 6. 测试
- [√] 6.1 后端 Debug 编译验证
- [√] 6.2 WinUI x64 Debug 编译验证
