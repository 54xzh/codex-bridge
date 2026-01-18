# 任务清单: Chat 页工作区菜单显示最近 cwd（轻量迭代）

目录: `helloagents/plan/202601190015_workspace_recent_cwd/`

---

## 1. 最近 cwd 记录
- [√] 1.1 在 `ConnectionService` 记录最近使用的工作目录（最多 5 条）
- [√] 1.2 持久化到本机（LocalAppData），启动后自动加载

## 2. 菜单优化
- [√] 2.1 菜单新增“在资源管理器中打开”
- [√] 2.2 菜单展示 5 条最近 cwd，点击可快速切换

## 3. 文档与变更
- [√] 3.1 更新 `helloagents/wiki/modules/winui-client.md`
- [√] 3.2 更新 `helloagents/CHANGELOG.md`

## 4. 验证
- [√] 4.1 `dotnet build` 通过
