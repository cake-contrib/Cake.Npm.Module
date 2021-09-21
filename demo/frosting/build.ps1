$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

# make sure we always get a fresh nuget-package and nothing from cache!
dotnet nuget locals all --clear

if(Test-Path .\build\tools) {
  rm -Force -Recurse .\build\tools
}

dotnet add .\build\Build.csproj package Cake.Npm.Module --prerelease
dotnet run --project build\Build.csproj -- $args
exit $LASTEXITCODE;