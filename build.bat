@echo off

call msbuild "PDRPC.sln" -t:Restore -p:Configuration=Release
call msbuild "PDRPC.sln" -t:Rebuild -p:Configuration=Release -p:Platform=x64