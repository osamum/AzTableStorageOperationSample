# AzTableStorageOperationSample
&lt;English&gt;

This is a simple sample program that uses Microsoft.Azure.Cosmos.Table to work with Azure Storage Table.

This console application how to use is the following:
## Preparation before build
You should write connection string of your any Azure storage account to the Setting.json in this project.

## How to use
This sample program does support feature is create and delete table, add and remove an entity and enumerates entities.

### Feature and command line arguments:
* Create table : create &lt;table name&gt;
* Delete table : delete &lt;table name&gt;
* Add entity : add &lt;firstname&gt;,&lt;lastname&gt;,&lt;email address&gt;,&lt; phone number&gt;
* Delete entity : remove &lt;firstname&gt;,&lt;lastname&gt;
* Display entity : show &lt;firstname&gt;,&lt;lastname&gt;
* Enumerates entity : enum &lt;table name&gt; or enum &lt;table name&gt; &lt;number of takes&gt;

You able to set these command line(Application) arguments using \[Debug\] tab in the project property view (when using Visual Studio 201x).
　

Enjoy!


&lt;Japanese&gt;

このプログラムは Microsoft.Azure.Cosmos.Table を使用して Azure ストレージ テーブルを操作するシンプルなサンプル アプリケーションです。

このコンソール アプリケーションの使用方法は以下のとおりです :

## ビルドする前の準備
このプロジェクト内の Setting.json に目的の Azure ストレージアカウンへの接続文字列を記述します。

## 使い方
このサンプル プログラムはテーブルの作成と削除、エンティティの追加と削除、エンティティの列挙 機能を持っています。

### 機能とコマンドライン引数
* テーブルの作成 : create &lt;テーブル名&gt;
* テーブルの削除 : delete &lt;テーブル名&gt;
* エンティティの追加 : add &lt;名&gt;,&lt;l姓&gt;,&lt;メール アドレス&gt;,&lt;電話番号&gt;
* エンティティの削除 : remove &lt;名&gt;,&lt;l姓&gt;
* エンティティの表示 : show&lt;名&gt;,&lt;l姓&gt;
* エンティティの列挙 : enum &lt;テーブル名&gt; もしくは enum &lt;テーブル名&gt; &lt;取得数&gt;

これらのコマンドライン引数は、プロジェクトのプロパティ画面の \[デバッグ\] タブで指定できます。(Visual Studio 201x 使用時)
　

楽しめ!