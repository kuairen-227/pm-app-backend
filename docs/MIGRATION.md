# 💾 Database

- DB: PostgreSQL
- Migration: Entity Framework Core

## コマンド

```bash
# マイグレーションファイルの作成
dotnet ef migrations add <マイグレーション名> --project WebApi.Infrastructure --startup-project WebApi.Infrastructure
# マイグレーションの実行
dotnet ef database update --project WebApi.Infrastructure --startup-project WebApi.Infrastructure
# 初期化
dotnet ef database drop --project WebApi.Infrastructure --startup-project WebApi.Infrastructure
```
