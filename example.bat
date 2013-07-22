SET HTMLCONVERTER=bin\HtmlConverter.exe
SET SRC=HtmlConverter
SET DST=HtmlConverter-html
SET XML=bin\brushlist.xml

CALL %HTMLCONVERTER% %SRC% %DST% %XML%

PAUSE
