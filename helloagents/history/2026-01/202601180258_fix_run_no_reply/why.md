# 变更提案: fix_run_no_reply

## 需求背景
当前在 Chat 页触发一次运行后，UI 可能显示“完成”，但实际上没有任何可见回复。典型原因包括：
1. Codex CLI 在非 Git 仓库目录下会拒绝运行（未指定 `--skip-git-repo-check`），但前端仅显示完成状态，用户难以定位原因。
2. Codex 进程退出码非 0 时仍被当作正常完成处理，导致错误被吞掉或不明显。

## 变更内容
1. 后端将 Codex 退出码非 0 视为失败并向前端透出错误信息（含 exitCode 与错误摘要）。
2. 前端新增“跳过 Git 检查”开关，显式控制是否传递 `--skip-git-repo-check`，确保行为继承 Codex CLI 的权限边界且可由用户决策。

## 影响范围
- **模块:** Bridge Server、WinUI Client
- **协议:** WebSocket `chat.send` 增加 `skipGitRepoCheck` 参数
- **体验:** 失败可见、可自助排障；非 Git 目录可选择允许运行

## 核心场景

### 需求: 失败应可见
**模块:** Bridge Server / WinUI Client
当 Codex 运行失败（退出码非 0）时，用户应能看到明确的失败原因。

#### 场景: 非 Git 目录直接运行
用户将工作区设置为非 Git 仓库目录（例如 `D:\\TWRP`）并发送 prompt。
- 预期结果：默认提示失败原因；用户可勾选“跳过 Git 检查”后再次运行以继续。

### 需求: 继承 Codex CLI 权限
**模块:** Bridge Server / WinUI Client
是否跳过 Git 检查必须由用户显式选择，而非后端隐式绕过。

## 风险评估
- **风险:** 允许在非 Git 目录运行可能扩大可操作范围
- **缓解:** 默认不开启；由用户显式勾选；仍受 sandbox 配置限制

