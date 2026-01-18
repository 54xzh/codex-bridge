# 变更提案: fix_codex_path

## 需求背景
在部分 Windows 环境（尤其是 MSIX/sidecar 启动）中，Bridge Server 启动 `codex` 时可能无法解析到正确的可执行文件（PATH 差异），从而出现 “找不到指定的文件” 的运行失败。

## 变更内容
1. Bridge Server 在 Windows 下自动定位 `codex` 可执行文件（优先从常见安装位置解析，如 npm/cargo/WindowsApps 与 PATH）。
2. 运行前对 `workingDirectory` 做存在性校验，输出明确错误信息。

## 影响范围
- **模块:** Bridge Server
- **配置:** `Bridge:Codex:Executable` 仍可手动覆盖；默认行为增强为“自动发现”

## 核心场景

### 需求: 可靠启动 Codex CLI
**模块:** Bridge Server
在 MSIX/sidecar 场景下仍能稳定找到并启动 `codex`。

#### 场景: WinUI 选择工作区后发送消息
用户在 Chat 页设置 `workingDirectory`（如 `D:\\TWRP`）并发送 prompt。
- 预期结果：Bridge Server 可成功启动 `codex`；若目录无效则提示“目录不存在或不可访问”。

## 风险评估
- **风险:** 自动发现目录不覆盖所有安装方式
- **缓解:** 保留 `Bridge:Codex:Executable` 绝对路径覆盖能力；自动发现覆盖 npm/cargo/WindowsApps 与 PATH 的常见安装方式

