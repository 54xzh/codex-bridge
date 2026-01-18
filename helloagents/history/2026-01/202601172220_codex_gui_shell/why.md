# 变更提案: Codex CLI GUI 壳 + Bridge Server

## 需求背景

`codex` CLI 已具备强大的编程辅助能力，但在日常使用中仍存在 UI 可用性与多端协作的需求：需要更直观的会话管理、diff 展示、设置切换，以及面向 Android 的远程控制与同步展示能力。

## 产品分析

### 目标用户与场景
- **用户群体:** Windows 开发者；需要在手机端“查看/跟随/轻量控制”同一会话的用户
- **使用场景:** 本机开发时以 WinUI 为主；手机作为第二屏进行同步查看与辅助操作；必要时开启局域网/公网远程
- **核心痛点:** CLI/TUI 不便进行会话浏览、diff 预览与设置切换；多端难以共享同一会话状态

### 价值主张与成功指标
- **价值主张:** 以 Bridge Server 统一会话与事件流，实现多端同步的 GUI 体验，并复用本机 `~/.codex` 配置
- **成功指标:**
  - WinUI 端能稳定展示 `codex exec --json` 的流式输出
  - 同一会话同时连接 WinUI/Android 时，消息与运行事件保持一致显示
  - 默认仅本机访问；开启远程访问后具备最小可用的安全防护（显式开关+令牌）

### 人文关怀
默认关闭远程访问，避免“意外暴露本机能力”；尽可能减少对用户现有 `~/.codex` 配置的干扰，并提供清晰的风险提示。

## 变更内容
1. 引入 Bridge Server（ASP.NET Core）作为状态中心与对外接口
2. 通过 `codex exec --json` 驱动 Codex，解析 JSONL 事件并转发到前端
3. WinUI3 前端实现：聊天、流式输出、会话管理、工作区选择、diff、设置页、模式切换栏
4. 预留 Android 端接入能力：统一协议与事件同步机制（同一会话多端一致）

## 影响范围
- **模块:** WinUI Client、Bridge Server、Protocol
- **文件:** 将新增后端项目与共享协议文件；WinUI 侧新增页面/视图模型/服务层
- **API:** 新增本机 HTTP/WS API（可选开启远程）
- **数据:** 新增本地会话索引与运行记录存储（MVP 可 JSON）

## 核心场景

### 需求: 聊天与流式输出
**模块:** WinUI Client / Bridge Server / Protocol

#### 场景: 发送消息并流式渲染
用户在 WinUI 输入 prompt，服务端启动 `codex exec --json`，前端按事件增量渲染；Android 作为第二端可同步看到同一流。

### 需求: 会话管理
**模块:** WinUI Client / Bridge Server

#### 场景: 创建/切换/恢复会话
用户创建会话并在会话列表中切换；服务端记录 `codexSessionId` 以支持 `codex exec resume`。

### 需求: 工作区选择
**模块:** WinUI Client / Bridge Server

#### 场景: 选择仓库作为运行目录
用户选择工作区路径，后续运行均在该目录执行（`codex exec -C <dir>`），并用于生成 diff 展示。

### 需求: Diff 展示
**模块:** WinUI Client / Bridge Server

#### 场景: 展示变更并允许应用/回滚
服务端提供“变更摘要/文件列表/diff 内容”给前端展示；前端提供应用/回滚入口（实现方式在技术设计中明确）。

### 需求: 设置与模式切换
**模块:** WinUI Client / Bridge Server

#### 场景: 切换权限模式/模型/思考深度
前端提供切换栏；服务端将选择映射为 `codex exec` 参数（如 `--model`、`--sandbox`、`-c key=value` 或 profile）。

### 需求: 远程访问与多端同步
**模块:** Bridge Server / Protocol

#### 场景: 开启局域网/公网并配对手机
默认仅回环；用户显式开启远程并生成令牌后，Android 可连接并与 WinUI 同步同一会话的事件与状态。

## 风险评估
- **风险:** 开启远程访问等同远程驱动本机 `codex` 能力（可能读写文件/执行命令）
- **缓解:** 默认仅回环；远程必须显式开启 + 强令牌；公网需 TLS/绑定策略/明确风险提示；限制并发与消息大小

