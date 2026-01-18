# 技术设计: 会话管理（列表/创建/重启保留）

## 技术方案

### 核心技术
- **后端:** ASP.NET Core (.NET 8)
- **前端:** WinUI 3
- **协议:** HTTP + WebSocket(JSON)
- **与 Codex 集成:** `codex exec --json` / `codex exec resume --json`

### 实现要点
1. **会话列表（读取 `.codex/sessions`）**
   - 通过 `Environment.SpecialFolder.UserProfile` 定位 `%USERPROFILE%\\.codex\\sessions`
   - 扫描 `sessions/YYYY/MM/DD/*.jsonl`，仅读取首行 `type=session_meta` 获取：
     - `payload.id`（sessionId）
     - `payload.timestamp`（创建时间）
     - `payload.cwd`（工作目录）
     - `payload.originator` / `payload.cli_version`（可选展示字段）
   - 以文件最后写入时间或 `payload.timestamp` 作为排序依据，返回最近 N 条

2. **创建会话（当前实现：直接写入 `.codex/sessions`）**
   - **当前实现（模式B）:** “新建会话”在 `.codex/sessions` 写入一个最小 JSONL（只包含 `session_meta`），让会话立即可见并可被 `resume`
     - 风险：依赖 Codex 内部格式；但属于新增文件、可回滚删除
   - **备选（模式A/后续可开关）:** “新建会话”仅创建 Bridge 侧占位，实际 Codex session 在首次 `chat.send`（不带 sessionId）后由 Codex 生成并出现在 `.codex/sessions` 中

3. **聊天绑定会话（resume）**
   - 扩展 WS command `chat.send`：增加可选 `sessionId`
     - `sessionId` 为空 → `codex exec --json -`（新会话）
     - `sessionId` 非空 → `codex exec resume --json <sessionId> -`（续聊）
   - `CodexRunner` 增加 `SessionId` 参数并选择不同子命令

4. **解析 session_meta（用于前端感知“新会话 id”）**
   - 后端读取 `codex exec --json` 的 JSONL 输出时，识别 `type=session_meta` 并提取 `payload.id`
   - 广播新增事件（示例）：
     - `session.created`：`{ runId, sessionId }`
   - 前端在首次新会话对话时拿到 `sessionId`，写入“当前会话”状态并可刷新会话列表

## API 设计

### [GET] /api/v1/sessions
- **描述:** 返回最近会话列表
- **响应(示例):**
  - `[{ "id": "...", "createdAt": "...", "cwd": "C:\\path", "originator": "...", "cliVersion": "..." }]`

### [POST] /api/v1/sessions
- **描述:** 创建会话（当前实现：写入 `.codex/sessions` 并返回真实 `sessionId`）
- **请求(可选):** `{"cwd":"C:\\path(optional)"}`

## 数据模型
### SessionSummary（对外）
- `id: string`
- `createdAt: string`
- `cwd: string?`
- `originator: string?`
- `cliVersion: string?`

## 安全与性能
- **安全:**
  - 默认仅回环访问；远程访问遵循 BearerToken 策略
  - 会话列表接口与 WS 一致校验 remoteIp + token
- **性能:**
  - 列表扫描仅读首行，避免读取大型 JSONL
  - 返回最近 N 条并支持分页/limit（后续）

## 测试与部署
- **测试:**
  - 后端/前端 `dotnet build` 验证
  - 通过会话页拉取 `/api/v1/sessions` 冒烟：能看到 `.codex/sessions` 中的会话
- **部署:**
  - 仍按 sidecar 方式随 WinUI 部署；无需额外安装
