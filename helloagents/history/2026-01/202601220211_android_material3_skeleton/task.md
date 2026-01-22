# 任务清单: Android Material3 UI 骨架

目录: `helloagents/plan/202601220211_android_material3_skeleton/`

---

## 1. UI 三模块骨架（Material3）
- [√] 1.1 在 `codex-bridge-android` 中引入 Navigation-Compose，并建立路由骨架（connect/sessions/chat），验证 why.md#需求-android-ui-骨架三模块
- [√] 1.2 实现“连接设备”页面骨架（配对/保存 deviceToken），验证 why.md#场景-首次启动配对后进入会话列表
- [√] 1.3 实现“会话列表”主界面骨架（列表/刷新/跳转），验证 why.md#场景-从会话列表进入聊天
- [√] 1.4 实现“聊天界面”骨架（历史/WS/发送/plan），验证 why.md#场景-从会话列表进入聊天

## 2. 基础设施抽离
- [√] 2.1 抽离连接配置持久化 `BridgePreferences` 与 `ConnectionConfig`
- [√] 2.2 抽离 token 加解密 `TokenCrypto`（AndroidKeyStore + AES-GCM）

## 3. 安全检查
- [√] 3.1 执行安全检查（按G9: 敏感信息不明文落盘、Debug Cleartext 限制在 debug manifest）

## 4. 文档更新
- [√] 4.1 更新知识库：新增 Android Client 模块文档并补充 overview/project/changelog

## 5. 测试
- [√] 5.1 执行 `codex-bridge-android` 的 `:app:assembleDebug` 验证编译通过

