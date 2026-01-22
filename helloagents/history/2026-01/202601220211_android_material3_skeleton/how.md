# 技术设计: Android Material3 UI 骨架

## 技术方案

### 核心技术
- Kotlin / Jetpack Compose
- Material3
- Navigation-Compose（页面路由）
- OkHttp + Gson（复用现有 Android 端网络实现）

### 实现要点
- 使用 `NavHost` 组织三大页面：
  - `sessions`：会话列表（主界面）
  - `chat?sessionId=`：聊天界面（支持进入既有会话或新建会话）
  - `connect`：连接设备（配对/保存 deviceToken）
- 抽离基础设施：
  - `BridgePreferences`：连接配置的持久化
  - `TokenCrypto`：deviceToken 本地加解密（AndroidKeyStore + AES-GCM）
- Debug 构建允许 HTTP（Cleartext）以支持局域网后端快速调试

## 架构决策 ADR

### ADR-001: UI 路由采用 Navigation-Compose
**上下文:** 三模块页面需要清晰边界与可扩展的跳转关系，单文件条件分支会逐步失控  
**决策:** 使用 Navigation-Compose 作为页面路由与参数传递机制（`sessionId` 作为可选 query 参数）  
**理由:** Compose 原生生态、依赖轻、可与 Material3 Scaffold/TopAppBar 自然结合  
**替代方案:** 单 Composable 通过 state 切页 → 拒绝原因: 模块边界不清晰、状态耦合加重  
**影响:** 需要引入 `androidx.navigation:navigation-compose` 依赖，并将 UI 拆分为独立 screen 文件

## 安全与性能
- **安全:** deviceToken 使用 AndroidKeyStore 加密存储；Debug 才允许 Cleartext
- **性能:** UI 仅加载必要的会话/消息数据；聊天 WS 连接在聊天页面生命周期内维护

## 测试与部署
- **测试:** `codex-bridge-android` 执行 `:app:assembleDebug` 以验证编译通过
- **部署:** Debug 安装到设备后，通过“连接设备”页面完成局域网配对

