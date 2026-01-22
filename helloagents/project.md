# 项目技术约定

## 技术栈
- **Windows 前端:** .NET 8 / C# / WinUI 3（Windows App SDK）
- **Android 前端:** Kotlin / Jetpack Compose / Material3
- **后端服务（Bridge Server）:** .NET 8 / ASP.NET Core（Kestrel）
- **与 Codex 集成:** 调用本机 `codex` CLI，优先使用 `codex exec --json` 事件流
- **远程接口:** HTTP + WebSocket（JSON 消息），默认仅本机回环
- **数据持久化（会话/设置）:** 本地文件（MVP 可先 JSON，后续可升级 SQLite）

---

## 开发约定
- **解决方案结构:** `codex-bridge`（WinUI3）+ `codex-bridge-server`（后端服务）+ `codex-bridge-shared`（协议/DTO，可选）
- **命名约定:** C# 使用 PascalCase；文件夹/项目名使用 kebab-case
- **异步规范:** I/O 全部使用 `async/await`，避免阻塞 UI 线程
- **序列化:** .NET 侧统一使用 `System.Text.Json`；Android 侧 MVP 使用 Gson（协议字段统一采用 camelCase）

---

## 错误与日志
- **统一错误模型:** 后端对外返回统一错误结构（code/message/details）
- **关联标识:** 每次运行（Run）生成 `runId`，贯穿日志与事件流
- **日志策略:** 后端输出结构化日志（含 workspace/session/run 维度）

---

## 测试与流程
- **测试类型:** 后端单元测试（协议/解析/存储）+ 集成测试（HTTP/WS）
- **发布形态:** Windows 端可用 MSIX；后端可作为子进程随 WinUI 启动或独立运行
