cls
echo off
SET DIR=%~dp0%
IF NOT EXIST "%DIR%log" MKDIR "%DIR%log"
"%PROGRAMFILES(X86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MsBuild.exe" /m /v:n "%DIR%/restorepackages.proj" /target:RestoreCorePackagesOnly /logger:FileLogger,Microsoft.Build.Engine;LogFile=%DIR%log/restorecorepackagesonly.log
pause