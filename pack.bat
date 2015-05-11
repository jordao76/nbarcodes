@echo off

rem Runs tests
msbuild Build.proj /t:Build

rem Builds NuGet target
msbuild Build.proj /t:NuGetCompile

rem Creates NuGet package
nuget pack NBarCodes\NBarCodes.csproj -Prop Configuration=Release.net40
