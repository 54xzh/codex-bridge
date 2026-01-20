# 任务清单: 修复无序列表下行内代码下移

目录: `helloagents/plan/202601210008_markdown_inline_code_list_offset_fix/`

---

## 1. 行内代码对齐
- [√] 1.1 在列表渲染上下文中移除行内代码的基线下移（避免首行“向下偏移”）
- [√] 1.2 保持普通段落行内代码的对齐策略不变

## 2. 文档更新
- [√] 2.1 更新 `helloagents/wiki/modules/winui-client.md`：同步行内代码基线对齐策略说明与最后更新日期
- [√] 2.2 更新 `helloagents/CHANGELOG.md`：记录无序列表行内代码对齐修复
- [√] 2.3 更新 `helloagents/history/index.md`：记录本次变更索引

## 3. 质量验证
- [√] 3.1 执行 `dotnet build` 验证编译通过
- [√] 3.2 执行 `dotnet test` 验证现有测试通过
