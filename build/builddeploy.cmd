cls
echo off
SET DIR=%~dp0%
IF NOT EXIST "%DIR%log" MKDIR "%DIR%log"
"%PROGRAMFILES(X86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MsBuild.exe" /m /v:n "%DIR%DavidLievrouw.Voter.proj" /target:BuildDeploy /logger:FileLogger,Microsoft.Build.Engine;LogFile=%DIR%log/builddeploy.log
pause