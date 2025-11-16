# Azure AI Foundry ハンズオン ガイド

## 📚 目次

- [Azure AI Foundry ハンズオン ガイド](#azure-ai-foundry-ハンズオン-ガイド)
  - [📚 目次](#-目次)
  - [はじめに](#はじめに)
    - [学習目標](#学習目標)
    - [所要時間](#所要時間)
  - [事前準備](#事前準備)
    - [必要な環境](#必要な環境)
    - [事前チェックリスト](#事前チェックリスト)
  - [Module 1: Azure AI Foundry プロジェクト作成](#module-1-azure-ai-foundry-プロジェクト作成)
  - [Module 2: AI Agent の構築](#module-2-ai-agent-の構築)
  - [Module 3: Web アプリケーション開発とデプロイ](#module-3-web-アプリケーション開発とデプロイ)
  - [🎉 完了おめでとうございます！](#-完了おめでとうございます)

---

## はじめに

このハンズオンでは、**初心者でも**製造業向けの AI ソリューションを構築できるよう、Azure AI Foundry を使った実践的な手順を学びます。

### 学習目標

- ✅ Azure AI Foundry で製造業専門の AI Agent を構築できる
- ✅ Chat Playground で　Agentのテストができる
- ✅ Web アプリケーションに AI Agent を統合できる
- ✅ Azure Container Apps にデプロイして運用できる

### 所要時間

- **Module 1-2**: 10分
- **Module 3**: 15-20分
- **合計**: 約30分

---

## 事前準備

### 必要な環境

1. **Azure サブスクリプション**
   - 無料アカウント: https://azure.microsoft.com/ja-jp/free/
   - または組織のサブスクリプション

2. **開発環境**
   - [Visual Studio 2022 以上](https://visualstudio.microsoft.com/ja/vs/) 
   - [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) 以上

3. **ブラウザ**
   - Microsoft Edge または Google Chrome (最新版)

### 事前チェックリスト

**必須項目の確認**:

```powershell
# .NET SDK のインストール確認 (9.0 以上が必要)
dotnet --version
# 出力例: 9.0.100

# Azure へのサインイン確認（オプション: CLI を使う場合）
az account show
# または Azure Portal にブラウザでアクセス可能か確認
```

**開発環境の確認**:

- **Visual Studio 2022 を使用する場合**:
  - バージョン 17.8 以上

確認方法:
```powershell
# Visual Studio のバージョン確認
# Visual Studio Installer を起動して確認
```

**ブラウザ**:
- Microsoft Edge または Google Chrome の最新版

---

## Module 1: Azure AI Foundry プロジェクト作成

Azure AI Foundry でプロジェクトを作成し、AI Agent を構築するための基盤を整えます。

**📖 詳細手順**: [module1-project-setup.md](module1-project-setup.md)

**所要時間**: 約5分

**主な内容**:
- Azure AI Foundry へのアクセス
- 新しいプロジェクトの作成
- プロジェクト設定の確認

---

## Module 2: AI Agent の構築

Azure AI Foundry で製造業センサーデータ分析専門の AI Agent を作成し、Chat Playground でテストします。

**📖 詳細手順**: [module2-ai-agent.md](module2-ai-agent.md)

**所要時間**: 約5分

**主な内容**:
- GPT-4o-mini モデルのデプロイ
- Agent Instructions の設定
- テストデータでの動作確認
- Agent ID と Project URL の取得

---

## Module 3: Web アプリケーション開発とデプロイ

事前に準備された Blazor Web アプリケーションテンプレートを使用します。Agent ID と Project URL を設定し、Azure Container Apps にデプロイして運用環境を構築します。

**📖 詳細手順**: [module3-webapp.md](module3-webapp.md)

**所要時間**: 約 30〜45 分

**主な内容**:
- サンプルコードのダウンロードと設定
- Agent 情報の appsettings.json への記録
- .NET Aspire でのローカル実行と動作確認
- Visual Studio から Azure Container Apps へのデプロイ
- Azure Portal での環境変数設定
- デプロイの検証とログ確認
- リソースのクリーンアップ

**使用技術**:
- .NET 10 / Blazor Server / Minimal API
- .NET Aspire (オーケストレーション)
- Azure Container Apps
- Azure AI Foundry Persistent Agents SDK

---

## 🎉 完了おめでとうございます！

Azure AI Foundry を使った製造業 AI ソリューションの基礎が完成しました。