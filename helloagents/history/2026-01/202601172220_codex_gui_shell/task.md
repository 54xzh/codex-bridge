# 任务清单: Codex GUI Shell（WinUI3 + Bridge Server）

目录: `helloagents/history/2026-01/202601172220_codex_gui_shell/`

---

## 1. Bridge Server（后端）
- [√] 1.1 新增后端项目 `codex-bridge-server`（ASP.NET Core），提供 `GET /api/v1/health`，验证 why.md#需求-远程访问与多端同步-场景-两端同时在线
- [√] 1.2 实现 `WS /ws` 基础连接与鉴权（回环默认放行，远程需 Bearer Token），验证 why.md#需求-远程访问与多端同步-场景-开启局域网公网并配对手机
- [√] 1.3 实现 `CodexRunner`：通过 `codex exec --json` 启动进程、逐行解析 JSONL、支持取消，验证 why.md#需求-聊天与流式输出-场景-发送消息并流式渲染
- [-] 1.4 实现 `SessionStore`：保存 `sessionId`↔`codexSessionId` 映射并支持列出/创建/归档，验证 why.md#需求-会话管理-场景-创建切换恢复会话
  > 备注: 未在本次骨架实现中引入持久化与会话索引
- [-] 1.5 实现工作区管理：选择工作区并以 `-C <workspace>` 运行 Codex，验证 why.md#需求-工作区选择-场景-选择仓库作为运行目录
  > 备注: 当前仅支持在 `chat.send` 中传入 `workingDirectory`（可选），尚未实现工作区列表与选择接口
- [-] 1.6 实现 diff 产物接口（先基于 `git diff`/文件快照做最小可用），验证 why.md#需求-diff-展示-场景-展示变更并允许应用回滚
  > 备注: 未在本次骨架实现中提供 diff 相关 API

## 2. WinUI Client（Windows 前端）
- [-] 2.1 设计导航与页面骨架：Chat / Sessions / Diff / Settings，验证 why.md#需求-会话管理-场景-创建切换恢复会话
  > 备注: 本次仅完成后端骨架，未改动 WinUI 前端
- [-] 2.2 接入 WS 事件流并实现流式渲染（增量追加、可取消），验证 why.md#需求-聊天与流式输出-场景-发送消息并流式渲染
  > 备注: 本次仅完成后端骨架，未改动 WinUI 前端
- [-] 2.3 实现会话列表与会话切换（绑定后端 SessionStore），验证 why.md#需求-会话管理-场景-创建切换恢复会话
  > 备注: 未实现 SessionStore
- [-] 2.4 实现工作区选择 UI（文件夹选择器）并调用后端选择接口，验证 why.md#需求-工作区选择-场景-选择仓库作为运行目录
  > 备注: 未实现工作区选择接口
- [-] 2.5 实现 diff 页面（文件列表+差异视图+应用入口），验证 why.md#需求-diff-展示-场景-展示变更并允许应用回滚
  > 备注: 未实现 diff 相关 API
- [-] 2.6 实现设置页与切换栏（权限模式/模型/思考深度 → 映射到后端运行参数），验证 why.md#需求-设置与模式切换-场景-切换权限模式模型思考深度
  > 备注: 未实现设置 API 与前端切换栏

## 3. Protocol（协议与多端）
- [√] 3.1 定义协议版本与消息 envelope（command/event/response），并在后端/WinUI 固化实现，验证 why.md#需求-远程访问与多端同步-场景-两端同时在线
- [√] 3.2 输出协议文档（更新 `helloagents/wiki/api.md` 与 `helloagents/wiki/modules/protocol.md`），确保 Android 侧可按文档实现

## 4. 安全检查
- [√] 4.1 执行安全检查（输入校验、令牌存储、默认回环、远程显式开关、日志脱敏）

## 5. 文档更新
- [√] 5.1 更新知识库：架构与 API 文档对齐实现（`helloagents/wiki/arch.md`、`helloagents/wiki/api.md`）
- [√] 5.2 更新 `helloagents/CHANGELOG.md`（记录新增模块与关键行为）

## 6. 测试
- [-] 6.1 后端单测：JSONL 解析与协议映射
  > 备注: 本次未添加测试项目
- [-] 6.2 后端集成测试：HTTP/WS + 多客户端同步场景
  > 备注: 本次未添加测试项目
