# 变更提案: WinUI 自动拉起后端并自动连接

## 需求背景

当前 WinUI 端已具备 WS Chat 页面，但仍需要用户手动启动后端并输入 WS 地址。为了降低使用门槛并满足“开箱即用”，需要在 WinUI 启动时自动拉起 Bridge Server，并在 Chat 页面自动连接到本机服务，无需手动配置。

## 变更内容
1. WinUI 启动时自动启动 Bridge Server 子进程（回环地址、随机可用端口）
2. WinUI 自动探测后端就绪（health 探测）并在 Chat 页面自动连接
3. 构建时自动将 `codex-bridge-server` 输出复制到 WinUI 输出目录，便于随应用一起启动

## 影响范围
- **模块:** WinUI Client、Bridge Server（启动方式）、Protocol（无变更）
- **文件:** WinUI 新增后端管理类、更新 App 启动逻辑与 Chat 页；更新知识库与 Changelog

## 核心场景

### 需求: 一键启动
**模块:** WinUI Client

#### 场景: 打开 WinUI 即可聊天
用户启动 WinUI 应用，后端自动在本机回环启动并被前端自动连接，用户无需输入地址与手动运行命令即可发送 prompt 并获取流式输出。

## 风险评估
- **风险:** 端口冲突/后端未就绪导致连接失败
- **缓解:** 使用随机可用端口；增加 health 轮询与重试；异常时在 UI 状态栏提示

