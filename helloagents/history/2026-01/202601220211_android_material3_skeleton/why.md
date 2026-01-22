# 变更提案: Android Material3 UI 骨架

## 需求背景
当前 `codex-bridge-android` 已具备基础的配对、会话列表与聊天能力，但 UI 代码集中在单一实现中，页面职责与状态耦合较重，不利于后续迭代与功能扩展。

本变更目标是在不改变现有协议与后端接口的前提下，为 Android 端搭建清晰的三大模块骨架：会话列表（主界面）/聊天界面/连接设备，并统一采用 Material3 的页面组织方式。

## 变更内容
1. 引入基于 Navigation-Compose 的页面路由，将 UI 拆分为三个独立模块页面
2. 抽离连接配置持久化与 token 加密逻辑，作为可复用基础设施
3. 为调试构建补充 Cleartext 允许配置，便于连接局域网 HTTP 后端（仅 Debug 生效）

## 影响范围
- **模块:**
  - Android Client
- **文件:**
  - `codex-bridge-android/app/src/main/java/com/xzh/bridge/ui/**`
  - `codex-bridge-android/app/src/main/java/com/xzh/bridge/storage/**`
  - `codex-bridge-android/app/src/main/java/com/xzh/bridge/security/**`
  - `codex-bridge-android/gradle/libs.versions.toml`
  - `codex-bridge-android/app/build.gradle.kts`
  - `codex-bridge-android/app/src/debug/AndroidManifest.xml`
- **API:** 无新增/变更（复用现有 Bridge Server HTTP/WS）
- **数据:** 本地 SharedPreferences 中保存 `baseUrl/deviceToken/deviceId`（deviceToken 加密）

## 核心场景

### 需求: Android UI 骨架（三模块）
**模块:** Android Client
为 Android 端提供会话列表（主界面）/聊天界面/连接设备三个模块页面，并可相互跳转。

#### 场景: 首次启动配对后进入会话列表
首次启动或本地无有效配对信息时
- 进入“连接设备”页面
- 完成配对后保存 deviceToken，并跳转到“会话列表”

#### 场景: 从会话列表进入聊天
已完成配对
- 进入“会话列表”作为主界面
- 点击任一会话，进入“聊天界面”并加载历史消息

#### 场景: 从主界面进入连接设备
已完成配对但需要重新配对或切换后端
- 从“会话列表”进入“连接设备”
- 可返回会话列表或完成新配对后回到会话列表

## 风险评估
- **风险:** Debug 构建允许 Cleartext 可能导致误用
  - **缓解:** 配置仅放在 `src/debug`，Release 不开启
- **风险:** deviceToken 落盘泄露
  - **缓解:** 使用 AndroidKeyStore(AES-GCM) 加密保存 token

