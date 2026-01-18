# 任务清单: Chat 页目录按钮交互（轻量迭代）

目录: `helloagents/plan/202601182343_workspace_button_menu/`

---

## 1. UI 展示
- [√] 1.1 目录按钮描述：`cwd` 显示目录名（basename）；未设置显示“未选择”

## 2. 菜单行为
- [√] 2.1 点击目录按钮：菜单仅两项
- [√] 2.2 选项1=完整路径：点击用系统资源管理器打开（未选择/目录不存在时禁用或提示）
- [√] 2.3 选项2=重新选择：弹出 FolderPicker，选择后更新 `WorkingDirectory`

## 3. 文档
- [√] 3.1 更新 `helloagents/wiki/modules/winui-client.md`
- [√] 3.2 更新 `helloagents/CHANGELOG.md`

## 4. 验证
- [√] 4.1 `dotnet build` 通过
