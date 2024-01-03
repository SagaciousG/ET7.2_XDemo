@echo off
echo %date% %time% -------------------
set devnev="D:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe"
e:
cd "E:\XGame\ProjectX\DotNet"
%devnev% DotNet.sln /build Debug /out E:\XGame\Tools\ServerTools

echo -----------------build complete----------------

rd /s /q E:\XGame\Server

echo -----------------Copy APP----------------------
xcopy "E:\XGame\ProjectX\Bin" "E:\XGame\Server" /i /y /e

echo -----------------Copy Config----------------------
xcopy "E:\XGame\ProjectX\Config" "E:\XGame\Server\Config" /i /y /e

pause