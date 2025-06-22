# enjoy-dessert

![Hero](https://github.com/user-attachments/assets/5091ca07-fd10-4d5e-8442-78d39959204c)

🍰 [瀏覽線上網站](https://enjoydessert.store)

## 專案簡介

enjoy-dessert 是一個為虛構甜點品牌打造的官方網站專案，具備完整的前台購物與後台管理功能。使用者可透過前台瀏覽商品、下單購買與查詢訂單，管理員則可透過後台管理商品與訂單、發布消息等。

本專案採用前後端分離架構，前端以 Angular 實作單頁式應用，後端則使用 ASP.NET Core Web API，搭配 PostgreSQL 與 Redis 提升資料處理效能。

## 專案結構

專案共分為四個模組：

1. **Client**（Angular）
   前端單頁式應用，負責呈現 UI 與 API 串接。
2. **API**（ASP.NET Core Web API）
   提供 RESTful API，處理業務邏輯與資料傳輸。
3. **Core**
   定義 Entity、Repository Interface、Service Interface 與查詢規格（Specification Pattern）。
4. **Infrastructure**
   實作 Repository、Service、資料庫設定、資料遷移（Migrations），並整合 Redis 快取機制。

## 功能簡介

### 前台功能（使用者）

- 商品瀏覽、熱銷推薦、詳細資訊
- 查閱最新消息
- 商品下單與訂單查詢
- 購物車功能（支援未登入狀態，資料儲存在 Redis）
- Stripe 金流整合，提供線上付款功能（支援測試模式）

### 後台功能（管理員）

- 商品新增、編輯與上下架管理
- 最新消息發布與編輯
- 訂單與使用者資料管理

## 金流整合（Stripe）

本專案整合 Stripe 金流服務，可支援測試模式與正式交易。以下為使用測試金鑰時的付款流程：

- 使用測試信用卡號碼（如 `4242 4242 4242 4242`）進行付款
- 導向 Stripe Checkout 頁面完成付款流程
- Stripe 回傳付款結果後，系統更新訂單狀態
- 可在 Stripe 的 Dashboard 中檢視每一筆模擬交易（Transactions）

若設定為正式金鑰，則可切換為實際金流交易。

### 環境變數與金流設定

Stripe 整合功能需要以下三個設定項目，請妥善管理這些敏感資訊以確保安全性：

```json
"StripeSettings": {
    "PublishableKey": "<STRIPE_PUBLISHABLE_KEY>",
    "SecretKey": "<STRIPE_SECRET_KEY>",
    "WhSecret": "<STRIPE_WH_SECRET>"
}
```

這些設定可以放置於 `appsettings.json`、`appsettings.Development.json`，或透過環境變數提供。為了資訊安全，建議使用環境變數方式注入，而非直接寫入設定檔中。

## 架構與實作概念

- 採用分層式架構（Layered Architecture），劃分 API、Domain、Infrastructure 與 Client 各層責任
- 使用 Repository Pattern、Unit of Work Pattern 管理資料存取與交易一致性
- 使用 Specification Pattern 封裝複雜查詢條件，提高查詢邏輯重用性
- 整合 Redis 快取部分資料（如商品列表、熱銷商品與詳細資訊）以減少資料庫壓力

## 使用技術

| 類別     | 技術                                   |
| -------- | -------------------------------------- |
| 前端     | Angular, TypeScript, RxJS, SCSS        |
| 後端     | ASP.NET Core Web API, EF Core          |
| 資料庫   | PostgreSQL                             |
| 快取     | Redis                                  |
| 金流     | Stripe（支援 Test 與 Live 模式）       |
| 設計工具 | Figma, HTML/CSS/JavaScript (Prototype) |

## 專案執行

### 開發環境

#### 前端(開發)

```bash
# 進入 client 資料夾
cd client

# 安裝依賴
npm install

# 啟動 Angular 開發伺服器（預設為 http://localhost:4200）
ng serve
```

#### 後端(開發)

```bash
# 啟動開發用的容器環境（Docker Compose）
docker compose up -d

# 執行後端 API 專案
dotnet run --project API
```

### 專案部署

#### 前端(部署)

```bash
# 進入 client 資料夾
cd client

# 編譯並輸出至 ../API/wwwroot（供後端靜態檔案使用）
ng build --configuration production
```

#### 後端(部署)

```bash
# 發佈後端專案
dotnet publish -c Release
```
