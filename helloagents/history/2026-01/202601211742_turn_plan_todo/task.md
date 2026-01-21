# 任务清单: 待办计划（Turn Plan）展示与查询

目录: `helloagents/plan/202601211742_turn_plan_todo/`

---

## 1. Bridge Server（计划事件 + 缓存 + HTTP）
- [√] 1.1 新增计划数据模型与缓存服务（如 `CodexTurnPlanStore`/`TurnPlanSnapshot`），并在 DI 注册；验证 why.md#需求-会话计划查询接口-场景-获取最新计划
- [√] 1.2 在 `codex-bridge-server/Bridge/CodexAppServerRunner.cs` 处理 `turn/plan/updated`：解析 `plan[]/explanation`，广播 `run.plan.updated`，并写入缓存；验证 why.md#需求-待办计划实时展示-场景-流式更新待办列表
- [√] 1.3 在 `codex-bridge-server/Controllers/SessionsController.cs` 新增 `GET /api/v1/sessions/{sessionId}/plan`：命中返回最新快照，未命中返回 404；验证 why.md#需求-会话计划查询接口-场景-获取最新计划

## 2. WinUI Client（待办 UI + 实时更新 + 回填）
- [√] 2.1 在 `codex-bridge/Pages/ChatPage.xaml` 增加“待办/计划”UI（建议 `Expander + ListView`），支持展示 step 与 status；验证 why.md#需求-待办计划实时展示-场景-流式更新待办列表
- [√] 2.2 在 `codex-bridge/Pages/ChatPage.xaml.cs` 增加对 `run.plan.updated` 事件处理，更新 UI 列表与说明文本；验证 why.md#需求-待办计划实时展示-场景-流式更新待办列表
- [√] 2.3 在会话切换/加载历史流程后调用 `GET /api/v1/sessions/{sessionId}/plan` 做回填（复用现有 BearerToken 逻辑）；验证 why.md#需求-待办计划实时展示-场景-会话进入-重连回填

## 3. 文档与协议同步
- [√] 3.1 更新 `helloagents/wiki/modules/protocol.md`：新增 event `run.plan.updated` 定义与字段说明
- [√] 3.2 更新 `helloagents/wiki/modules/bridge-server.md` 与 `helloagents/wiki/api.md`：新增 `GET /api/v1/sessions/{sessionId}/plan` 与事件列表补充
- [√] 3.3 更新 `helloagents/wiki/modules/winui-client.md`：补充“待办/计划”区域的交互与显示规则
- [√] 3.4 更新 `helloagents/CHANGELOG.md`：记录新增事件与接口

## 4. 安全检查
- [√] 4.1 确认 WS 与 HTTP 查询接口均受 `BridgeRequestAuthorizer` 保护；`sessionId`/返回字段做基本校验与脱敏评估

## 5. 测试
- [√] 5.1 在 `codex-bridge-server.Tests` 增加计划解析/缓存/接口测试（命中/未命中/未授权）
- [√] 5.2 运行服务端与 WinUI 编译并进行基本联调：实时 plan 推送 + HTTP 回填
  > 备注: 使用 CLI 构建 WinUI 项目时需指定 `-p:Platform=x64`（AnyCPU 会触发 MSIX 打包架构错误）。
