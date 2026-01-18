# 变更提案: 会话管理（列表/创建/重启保留）

## 需求背景
当前 WinUI 端仅提供聊天骨架，缺少“会话”的可视化管理能力：用户无法查看历史会话、创建新会话并在应用重启后继续使用同一会话上下文。

同时，用户已在本机 `~/.codex` 下拥有 Codex CLI 的既有配置与会话数据，希望尽量复用现有会话存储与 `resume` 能力，而不是在 Bridge 层重复存储一套“会话历史”。

## 变更内容
1. Bridge Server 提供会话列表与创建接口（HTTP），并在聊天发送时支持指定会话以便 resume
2. WinUI Client 增加会话页：展示会话列表、创建会话、选择会话进入聊天
3. 协议补齐会话相关字段/事件，保证前端可感知“当前会话”与“新会话创建”

## 影响范围
- **模块:**
  - WinUI Client
  - Bridge Server
  - Protocol
- **文件:**
  - `codex-bridge-server/*`（新增会话扫描与会话 API、CodexRunner 支持 resume/解析 session_meta）
  - `codex-bridge/*`（会话页 UI、Chat 发送携带会话、会话选择状态）
  - `helloagents/wiki/*`（API/数据模型/模块文档同步）
- **API:**
  - `GET /api/v1/sessions`
  - `POST /api/v1/sessions`
  - （可选）`GET /api/v1/sessions/{sessionId}`
- **数据:**
  - 读取：`%USERPROFILE%\\.codex\\sessions`（扫描 `session_meta` 用于列表/恢复）
  - 写入：用户点击“新建会话”时写入最小 `session_meta` JSONL（可回滚删除）

## 核心场景

### 需求: 会话列表
**模块:** Bridge Server, WinUI Client
从本机 `.codex/sessions` 扫描会话元数据并在 WinUI 展示。

#### 场景: 打开会话页看到最近会话
应用启动后进入“会话”页
- 展示最近 N 条会话（含创建时间、cwd/工作区、来源/客户端）
- 支持刷新

### 需求: 创建会话
**模块:** Bridge Server, WinUI Client
用户点击“新建会话”后得到一个可被选择的会话，并在重启后仍存在。

#### 场景: 新建会话并进入聊天
在“会话”页点击“新建”
- 会话出现在列表中
- 选择该会话后进入聊天页，后续 `chat.send` 绑定该会话上下文

### 需求: 重启后会话仍在
**模块:** Bridge Server, WinUI Client
应用退出并重新启动后仍能看到之前的会话并继续聊天（resume）。

#### 场景: 选择历史会话继续对话
重启应用进入“会话”页
- 仍能看到之前的会话
- 选择某会话后，聊天发送可通过 `codex ... resume <sessionId>` 延续上下文

## 风险评估
- **风险:** `.codex/sessions` 属于 Codex CLI 内部存储格式，直接写入可能存在兼容性风险。
  - **缓解:** 当前实现仅写入最小 `session_meta`（不写入对话内容），且仅在用户主动创建会话时执行；如后续发现兼容性问题，可切换为“只读扫描 + 运行时创建会话”的模式并提供回滚（删除生成文件）。
- **风险:** 远程访问开启后可能暴露会话元数据（cwd 等）。
  - **缓解:** 远程默认关闭；开启后强制 BearerToken；会话接口遵循与 WS 相同的鉴权策略。
