# 技术设计: 待办计划（Turn Plan）展示与查询

## 技术方案

### 核心技术
- **后端:** ASP.NET Core / `codex app-server`（JSON-RPC over stdio）/ `System.Text.Json`
- **前端:** WinUI 3 / XAML / `ObservableCollection`

### 实现要点
1. **协议扩展：** 新增 WS event `run.plan.updated`，用于向前端推送计划更新。
2. **后端解析：** 在 `CodexAppServerRunner` 的通知处理逻辑中新增 `turn/plan/updated` 分支，解析 `plan[]`（step/status）与 `explanation`。
3. **服务端缓存：** 引入 `CodexTurnPlanStore`（单例），按 `threadId(sessionId)` 保存最新计划快照，供 HTTP 查询与 UI 回填使用。
4. **HTTP 查询接口：** 在 `SessionsController` 增加 `GET /api/v1/sessions/{sessionId}/plan`，返回缓存快照；未命中返回 404。
5. **前端展示：** Chat 页新增“待办/计划”控件（建议 `Expander + ListView`），实时更新；加载会话历史后请求 plan 接口做回填。

## 架构设计

```mermaid
flowchart LR
  Codex[codex app-server] -->|turn/plan/updated| Runner[CodexAppServerRunner]
  Runner -->|event: run.plan.updated| WS[WebSocketHub]
  Runner --> Store[CodexTurnPlanStore]
  Store --> API[SessionsController: GET /sessions/{id}/plan]
  WS --> UI[WinUI ChatPage]
  API --> UI
```

## 架构决策 ADR

### ADR-001: 采用“内存最新快照 + HTTP 查询”的计划回填方案
**上下文:** `codex app-server` 会在运行中推送计划更新（turn/plan/updated）。仅靠 WS 实时推送无法在 Chat 页重开/重连时恢复计划显示；同时直接修改 `~/.codex/sessions` 写入新事件存在格式与兼容性风险。

**决策:** 在 Bridge Server 内新增 `CodexTurnPlanStore`，按 `sessionId(threadId)` 维护“最新计划快照”；新增 `GET /api/v1/sessions/{sessionId}/plan` 提供前端回填。

**理由:**
- 与现有架构贴合：服务端已负责协议映射与会话相关 API；
- 风险较低：不改动 Codex 自身 session 文件格式；
- 体验更完整：进入会话即可看到上次计划状态。

**替代方案:**
- 仅 WS 推送，不提供查询：重开/重连丢失计划 → 不满足方案2目标。
- 持久化写入 `~/.codex/sessions`：可能破坏 Codex 兼容性/未来升级 → 暂不采用。

**影响:**
- 需要在服务端维护缓存与清理策略（可设置容量上限/TTL）。

## API设计

### WS event: `run.plan.updated`
- **数据字段（建议）:**
  - `runId`: string
  - `threadId`: string（同 `sessionId`）
  - `turnId`: string
  - `explanation`: string?（可选）
  - `plan`: `{ step: string, status: "pending"|"inProgress"|"completed" }[]`
  - `updatedAt`: ISO-8601

### [GET] /api/v1/sessions/{sessionId}/plan
- **响应（建议）:**
  - `sessionId`: string
  - `turnId`: string
  - `explanation`: string?
  - `plan`: `{ step: string, status: string }[]`
  - `updatedAt`: ISO-8601
- **错误:**
  - 401：未授权
  - 404：无缓存计划

## 数据模型

```json
{
  "sessionId": "thread_xxx",
  "turnId": "turn_xxx",
  "explanation": "可选说明",
  "updatedAt": "2026-01-21T09:42:00Z",
  "plan": [
    { "step": "…", "status": "pending" },
    { "step": "…", "status": "inProgress" },
    { "step": "…", "status": "completed" }
  ]
}
```

## 安全与性能
- **安全:** 复用现有 BearerToken 鉴权；HTTP 与 WS 同等授权边界；`sessionId` 参数做非空校验。
- **性能:** 计划更新 payload 较小；缓存仅保存最新快照；可增加容量上限（例如最多缓存 1000 个 session）。

## 测试与部署
- **后端测试:**
  - 计划通知解析（合法/非法结构）
  - PlanStore 更新与查询
  - Controller：命中/未命中/未授权
- **前端验证:**
  - 实时更新：收到 `run.plan.updated` 后 UI 列表更新
  - 会话回填：打开已有 session 后 plan 区域能正确显示/清空

