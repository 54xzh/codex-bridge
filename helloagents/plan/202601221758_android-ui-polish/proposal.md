# Android 客户端体验优化（过渡动画/连接扫码/聊天样式）

## 背景
当前 Android 客户端已具备会话/聊天/连接三模块骨架，但在页面过渡、配对入口与聊天视觉上仍有优化空间。

## 目标
- 页面切换使用 Navigation-Compose 原生过渡效果，并支持预测性返回（Predictive Back）。
- 连接页面增加扫码器，且简化输入，仅保留“二维码内容”输入框。
- 聊天页面移除用户/助手头像；助手消息改为纯文本样式（不使用对话框气泡）。

## 范围
### 范围内
- Android：`BridgeApp` 导航过渡、`AndroidManifest` 预测性返回配置
- Android：连接页扫码与二维码解析（CameraX + ML Kit）
- Android：聊天消息样式调整
- Android：为二维码解析逻辑补充单元测试
- 文档：同步更新 Android Client 模块文档与 CHANGELOG

### 范围外
- 协议/后端配对流程变更
- UI 大规模重构（仅做明确需求点）

## 方案与关键决策
### 1) 过渡动画与预测性返回
- 使用 `navigation-compose` 提供的 `NavHost` 原生过渡能力（滑动切换），避免额外引入第三方动画导航库。
- 在 `AndroidManifest.xml` 为 Activity 显式开启 `android:enableOnBackInvokedCallback="true"`，确保系统返回手势可用并支持预测性返回。

### 2) 连接页：扫码器 + 单输入框
- 连接页仅保留“二维码内容”输入框（粘贴/扫码填充）。
- 新增扫码器：CameraX 取景 + ML Kit QR 识别，扫码结果回填输入框并自动解析出 `baseUrl` 与 `pairingCode`。
- 解析逻辑抽离为纯函数，提供单元测试覆盖核心用例。

### 3) 聊天页：无头像 + 助手纯文本
- 移除用户/助手头像控件。
- 用户消息仍使用气泡（右侧对齐）；助手消息改为左侧纯文本样式，减少视觉噪音并提升阅读感。

## 验收标准
- 返回手势在支持 Predictive Back 的系统上可生效（不禁用 OnBackInvokedCallback）。
- 连接页可扫码识别二维码内容并完成配对；页面无多余输入框。
- 聊天页无头像；助手消息不使用气泡容器。
- Android 单元测试通过。
