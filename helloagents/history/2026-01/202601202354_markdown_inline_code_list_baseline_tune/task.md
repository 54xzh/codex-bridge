# 任务清单: Markdown 行内代码与列表对齐微调

目录: `helloagents/plan/202601202354_markdown_inline_code_list_baseline_tune/`

---

## 1. 行内代码基线对齐
- [√] 1.1 在 `codex-bridge/Markdown/FilePathMarkdownRenderer.cs` 中将行内代码 `InlineUIContainer` 的基线偏移改为按 `FontSize` 缩放（减少不同字号/字体下的偏移差异）

## 2. 无序列表对齐
- [√] 2.1 在 `codex-bridge/Markdown/FilePathMarkdownRenderer.cs` 中对列表渲染临时去除 `ParagraphMargin.Top`（避免 bullet 与首行内容垂直不齐）

## 3. 文档同步
- [√] 3.1 更新 `helloagents/wiki/modules/winui-client.md`（补充列表对齐与偏移策略说明）
- [√] 3.2 更新 `helloagents/CHANGELOG.md`（修复项记录）

## 4. 质量验证
- [√] 4.1 执行 `dotnet build codex-bridge/codex-bridge.csproj -c Debug -p:Platform=x64`
- [√] 4.2 执行 `dotnet test codex-bridge-server.Tests/codex-bridge-server.Tests.csproj -c Debug`

## 5. 方案包归档
- [√] 5.1 将方案包迁移至 `helloagents/history/2026-01/` 并更新 `helloagents/history/index.md`
