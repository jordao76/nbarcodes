@echo off

rem Default is Release mode
msbuild Build.proj

rem Debug mode
rem msbuild Build.proj /p:BuildConfiguration=Debug