# 任务清单: Markdown 行内代码基线对齐修复

目录: `helloagents/plan/202601202330_markdown_inline_code_baseline_fix/`

---

## 1. 渲染与交互修复
- [√] 1.1 在 `codex-bridge/Markdown/FilePathMarkdownRenderer.cs` 中使用 `InlineRenderContext.InlineCollection` 渲染行内代码，避免部分上下文回退导致样式/点击失效
- [√] 1.2 在 `codex-bridge/Markdown/FilePathMarkdownRenderer.cs` 中对 `InlineUIContainer` 的 `Border` 统一使用 `TranslateTransform.Y=4`，对齐基线并消除“漂移”
- [√] 1.3 在 `codex-bridge/Pages/ChatPage.xaml` 中将 `InlineCodeBorderThickness` 设为 `0`，避免回退渲染出现轮廓描边

## 2. 文档同步
- [√] 2.1 更新 `helloagents/wiki/modules/winui-client.md`（行内代码样式/打开文件行为/已知限制）
- [√] 2.2 更新 `helloagents/CHANGELOG.md`（修复项/描述准确性）

## 3. 质量验证
- [√] 3.1 执行 `dotnet build codex-bridge/codex-bridge.csproj -c Debug -p:Platform=x64`

## 4. 方案包归档
- [√] 4.1 将方案包迁移至 `helloagents/history/2026-01/` 并更新 `helloagents/history/index.md`
