# 变更提案: 待办计划（Turn Plan）展示与查询

## 需求背景
当前项目已通过 `codex app-server` 实现会话续聊、流式输出、命令执行与思考摘要等能力，但未暴露 Codex 生成的“计划/待办（plan）”信息。用户在 GUI 中无法直观看到模型接下来要做什么、当前进度如何，影响可预期性，也不利于在需要审批（command/file change）时理解上下文与风险。

本变更旨在将 `codex app-server` 的 `turn/plan/updated` 通知映射为稳定的 Bridge 协议事件，并在 WinUI 端提供可视化展示；同时提供按会话查询“最新计划”的 HTTP 接口，用于进入会话/重连时回填显示（方案2）。

## 变更内容
1. **Bridge Server：**
   - 接入 `turn/plan/updated` 通知，解析 `plan[]` 与 `explanation`，广播为 Bridge event（拟定 `run.plan.updated`）。
   - 维护按 `sessionId(threadId)` 维度的“最新计划”内存缓存。
   - 新增接口 `GET /api/v1/sessions/{sessionId}/plan`，用于前端回填。
2. **WinUI Client：**
   - Chat 页新增“待办/计划”区域，实时渲染步骤列表与状态（`pending`/`inProgress`/`completed`）。
   - 切换/加载会话时，通过 HTTP 拉取最新计划并显示。
3. **文档与协议：**
   - 补齐协议事件与 HTTP API 文档，并更新模块说明与变更记录。

## 影响范围
- **模块:** Bridge Server / Protocol / WinUI Client
- **文件（预估）:**
  - `codex-bridge-server/Bridge/CodexAppServerRunner.cs`
  - `codex-bridge-server/Bridge/WebSocketHub.cs`（如需补充事件说明/回填时机）
  - `codex-bridge-server/Controllers/SessionsController.cs`
  - `codex-bridge/Pages/ChatPage.xaml`
  - `codex-bridge/Pages/ChatPage.xaml.cs`
  - `helloagents/wiki/modules/protocol.md`
  - `helloagents/wiki/modules/bridge-server.md`
  - `helloagents/wiki/modules/winui-client.md`
  - `helloagents/wiki/api.md`
  - `helloagents/CHANGELOG.md`
- **API:**
  - 新增 HTTP：`GET /api/v1/sessions/{sessionId}/plan`
  - 新增 WS event：`run.plan.updated`
- **数据:** 仅服务端内存缓存（不引入持久化；服务重启后缓存清空）

## 核心场景

### 需求: 待办计划实时展示
**模块:** Protocol / Bridge Server / WinUI Client

#### 场景: 流式更新待办列表
运行中 `codex app-server` 推送 `turn/plan/updated`。
- 服务端解析并广播 `run.plan.updated`（包含 `plan[]`、可选 `explanation`）。
- 前端实时更新“待办/计划”列表，并展示每步状态。

#### 场景: 会话进入/重连回填
用户进入已有 `sessionId` 或 Chat 页重新打开。
- 前端调用 `GET /api/v1/sessions/{sessionId}/plan` 获取最新计划并渲染。
- 若服务端无缓存计划，则返回 404（前端隐藏/清空计划区域）。

### 需求: 会话计划查询接口
**模块:** Bridge Server

#### 场景: 获取最新计划
提供 `sessionId` 请求最新计划。
- 命中缓存：返回 `threadId/sessionId`、`turnId`、`plan[]`、`explanation`、`updatedAt`。
- 未命中：返回 404。

## 风险评估
- **风险:** 计划内容可能包含文件路径/命令等敏感信息，多客户端连接时广播可能导致越权可见。
  - **缓解:** 复用现有 WS/HTTP BearerToken 鉴权；保持与现有事件一致的授权边界。后续如需要多用户并发，再引入 client/session 级订阅隔离。
- **风险:** 缓存增长导致内存占用上升。
  - **缓解:** 仅保留每个会话的最新快照；增加容量上限与过期清理（可选）。

