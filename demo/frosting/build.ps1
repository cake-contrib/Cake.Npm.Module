$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot
dotnet run --project build\Build.csproj -- $args
exit $LASTEXITCODE;
