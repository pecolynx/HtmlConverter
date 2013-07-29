HtmlConverter
=============
ソースコードをフォルダ単位でHTMLに変換します。  
ソースコードの表示には[SyntaxHighlighter](http://alexgorbatchev.com/SyntaxHighlighter/)を使用しています。

ビルド方法
----------
.NET Framework 4.0が必要です。  
build.batを編集しMSBuildのパスを設定してください。  
build.batを実行するとbinフォルダにビルドされます。  

実行方法
--------
HtmlConverter.exe [ソースコードが含まれるフォルダ] [出力フォルダ] [SyntaxHighlighterの設定ファイル]

実行例
------
example.batを実行してください。  
HtmlConverter-htmlフォルダが作成されます。  
HtmlConverter-htmlフォルダ内にあるindex.htmlファイルをブラウザで表示してください。  
