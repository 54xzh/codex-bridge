# 任务清单: 选择会话后自动使用 cwd（轻量迭代）

目录: `helloagents/plan/202601190003_session_cwd_autosync/`

---

## 1. 会话选择联动
- [√] 1.1 选择已有 session 后，自动将 `ConnectionService.WorkingDirectory` 设置为该 session 的 `cwd`
- [√] 1.2 修正 session 状态写入顺序，避免短暂使用旧 cwd

## 2. 新会话一致性
- [√] 2.1 收到 `session.created` 时同步记录当前 workingDirectory 到 `CurrentSessionCwd`

## 3. 文档与变更记录
- [√] 3.1 更新 `helloagents/wiki/modules/winui-client.md`
- [√] 3.2 更新 `helloagents/CHANGELOG.md`

## 4. 验证
- [√] 4.1 `dotnet build` 通过
