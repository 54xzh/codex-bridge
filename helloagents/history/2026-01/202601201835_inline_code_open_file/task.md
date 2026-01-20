# 轻量迭代任务清单：Markdown 行内代码文件路径打开

> 方案类型: 轻量迭代（仅 task.md）
> 创建时间: 2026-01-20 18:35

## 任务

- [√] 识别 Markdown 行内代码中的文件路径（绝对/相对，基于 cwd 解析）并仅显示文件名
- [√] 点击行内文件名可打开文件（目录则在资源管理器中打开）
- [√] 兼容常见括号/引号包裹、尾随标点，并在渲染时无法解析但形似路径时支持点击重试解析
- [√] 更新 WinUI 客户端知识库说明
- [√] 更新 Changelog
- [√] 本地构建验证（`dotnet build codex-bridge/codex-bridge.csproj -c Debug -p:Platform=x64`）
