# 購物網站_想想食室(負責後端，此專案共兩人合作)
> _大四上_友鋒課期末分組作業_   

* 功能說明
  1. 購物車系統(購物下單，購物車結帳)。
  2. 會員系統(註冊、登入、修改會員資料、查詢訂單)。
  3. 訂單管理系統(查看每筆訂單之品項及訂購人)。
  4. 商品管理系統(可針對商品CRUD，並最多新增三組圖片，用作產品頁面輪播)。
 
* 使用技術
  1. ASP.NET MVC
  2. C#
  3. js
  4. Entity Framework
  5. LINQ
  6. git
  
* 程式架構
  |        | 說明 |程式 |
  |------- |:-----|:------:|
  | **前端**   |  1. 前端主要由組員負責 |  [程式碼](https://github.com/hank444tw/finalexam/tree/new_master/FinalExam/Views) |
  | **後端**   |  1. 以`LINQ`語法透過`model`，對資料庫進行存取。</br> 2. 每張訂單編號，皆是由`Guid`隨機產生。</br> 3. 若店家新增商品未上傳圖片，則以預設圖片替代;若有的話則以第一張作為商品封面。</br>4. 將由`model`取得之資料，以串列形式回傳前端。 |  [程式碼](https://github.com/hank444tw/finalexam/blob/new_master/FinalExam/Controllers/HomeController.cs) |
  | **資料庫** |  1. 使用`ASP.NET MVC`的`Entity Framework`進行資料庫設計。</br> 2. 建置`model`來對資料庫進行存取。 |   [程式碼](https://github.com/hank444tw/finalexam/tree/new_master/FinalExam/Models) | 
  | **版控** |  1. 使用`git`搭配`sourcetree`工具實施版本控制。</br> 2. 一人負責前端，一人後端，兩人做完後再merge。 |   [Commits](https://github.com/hank444tw/finalexam/commits/new_master) | 

* 網站截圖
<img src="https://github.com/hank444tw/finalexam/blob/new_master/banner.JPG" stryle="float:right" />  

<img src="https://github.com/hank444tw/finalexam/blob/new_master/banner1.JPG" stryle="float:right" />    

<img src="https://github.com/hank444tw/finalexam/blob/new_master/banner2.JPG" stryle="float:right" />
