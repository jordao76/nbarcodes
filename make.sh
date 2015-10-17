#!/bin/sh
nuget restore
xbuild Build.proj
mono packages/NUnit.Runners.2.6.4/tools/nunit-console.exe NBarCodes.Tests/bin/Release/NBarCodes.Tests.dll
