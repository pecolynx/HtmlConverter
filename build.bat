SET MSBUILD=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe
CALL %MSBUILD% HtmlConverter.sln /p:Configuration=Release
XCOPY /E /I /Y HtmlConverter\bin\Release bin
XCOPY /E /I /Y HtmlConverter\resources bin\resources
PAUSE
