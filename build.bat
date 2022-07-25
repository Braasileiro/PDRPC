@echo off

REM Native
call msbuild "PDRPC\PDRPC.sln" -t:Rebuild -p:Configuration=Release -p:Platform=x64

REM Managed
call msbuild "PDRPC.Core\PDRPC.Core.sln" -t:restore
call msbuild "PDRPC.Core\PDRPC.Core.sln" -t:Rebuild -p:Configuration=Release -p:Platform="Any CPU"
