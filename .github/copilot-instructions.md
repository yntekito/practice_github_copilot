# Copilot Instructions for practice_github_copilot

## アーキテクチャ概要
- ASP.NET Core Web API（`TaskManagementApi/`）とユニットテスト（`TaskManagementApiTests/`）で構成。
- データアクセスは Entity Framework Core（`TaskManagementContext`）を利用。
- サービス層（`Services/`）でビジネスロジックを分離。
- コントローラー（`Controllers/`）がAPIエンドポイントを提供。

## 主要な開発ワークフロー
- ビルド: `dotnet build TaskManagementApi/TaskManagementApi.csproj`
- 実行: `dotnet run --project TaskManagementApi/TaskManagementApi.csproj`
- テスト: `dotnet test TaskManagementApiTests/TaskManagementApiTests.csproj`
- デバッグ: launch.json で `TaskManagementApi` を起動（Swagger UIが自動で開く）

## コーディング規約・パターン
- サービスクラスは `I[Service名]` インターフェースを実装（例: `UserService : IUserService`）。
- データモデルは `Models/`、DTOは `DTOs/` に配置。
- エラーは null返却または `Dictionary<string, object>` で詳細情報を返す（例: `GenerateComplexUserAnalyticsReportAsync`）。
- 日付は UTC で管理（`CreatedAt`, `UpdatedAt`）。
- Include/ThenInclude で関連データを明示的にロード。

## 依存関係・統合
- Entity Framework Core（`Microsoft.EntityFrameworkCore`）
- Swagger（launch.jsonで自動起動）

## 重要ファイル・ディレクトリ
- `TaskManagementApi/Controllers/` ... APIエンドポイント
- `TaskManagementApi/Services/` ... ビジネスロジック
- `TaskManagementApi/Models/` ... データモデル
- `TaskManagementApi/Data/TaskManagementContext.cs` ... DBコンテキスト
- `TaskManagementApiTests/` ... テストコード

## プロジェクト固有の注意点
- 複雑な分析処理は `UserService.GenerateComplexUserAnalyticsReportAsync` に集約。
- API仕様はコントローラーとサービスのメソッドシグネチャを参照。
- DBファイル（SQLite）は `TaskManagementApi/taskmanagement.db`。

## 例: サービスクラスのパターン
```csharp
public class UserService : IUserService {
    // ...
    public async Task<User?> GetUserByIdAsync(int id) {
        return await _context.Users
            .Include(u => u.Projects)
            .Include(u => u.AssignedTasks)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
```

---
この内容で不明点や追加したい情報があればご指摘ください。
