# 数据模型

## 概述

本项目以 Bridge Server 为状态中心，但会话数据优先复用 Codex CLI 的本地存储：`%USERPROFILE%\\.codex\\sessions`。
当前对外暴露的“会话列表/恢复”以读取 `session_meta` 元数据为主，Bridge 自身的持久化（JSON/SQLite）保留为后续扩展点。

---

## 数据对象

### SessionSummary（已实现）
| 字段名 | 类型 | 约束 | 说明 |
|--------|------|------|------|
| id | string | 非空 | Codex 会话 UUID（用于 `resume`） |
| title | string | 非空 | 会话标题（优先取“首条 user 消息”截断；否则回退到 cwd 或 id） |
| createdAt | string | 非空 | 会话创建时间（来自 `session_meta.payload.timestamp`） |
| cwd | string | 可空 | 会话工作目录（来自 `session_meta.payload.cwd`） |
| originator | string | 可空 | 会话来源（如 `codex_vscode` / `codex_bridge`） |
| cliVersion | string | 可空 | CLI 版本（如 `0.86.0-alpha.1`） |

### SessionMessage（已实现）
| 字段名 | 类型 | 约束 | 说明 |
|--------|------|------|------|
| role | string | 非空 | user / assistant |
| text | string | 非空 | 纯文本内容（从 JSONL content 中提取/拼接；user 文本会做上下文清洗，仅保留真实请求） |
| kind | string | 可空 | 当前固定为 message（为后续扩展预留） |

### Workspace
| 字段名 | 类型 | 约束 | 说明 |
|--------|------|------|------|
| id | string | 非空 | 工作区标识 |
| path | string | 非空 | 本地路径 |
| lastUsedAt | string | 可空 | 最近使用时间 |

### Session（规划）
| 字段名 | 类型 | 约束 | 说明 |
|--------|------|------|------|
| id | string | 非空 | 会话标识（Bridge Server） |
| title | string | 可空 | 会话标题 |
| workspaceId | string | 非空 | 关联工作区 |
| codexSessionId | string | 可空 | Codex 侧会话 UUID（用于 resume） |
| createdAt | string | 非空 | 创建时间 |
| updatedAt | string | 非空 | 更新时间 |

### Run（规划）
| 字段名 | 类型 | 约束 | 说明 |
|--------|------|------|------|
| id | string | 非空 | 运行标识 |
| sessionId | string | 非空 | 关联会话 |
| status | string | 非空 | running/succeeded/failed/canceled |
| startedAt | string | 非空 | 开始时间 |
| finishedAt | string | 可空 | 结束时间 |

---

## 存储策略（建议）

当前实现：会话元数据直接来自 `~/.codex/sessions`。
后续如需 Bridge 自身持久化（例如“会话标题/归档/固定/多端同步缓存”），MVP 可采用 JSON 文件存储，成熟后按需升级到 SQLite。
