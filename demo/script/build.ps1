$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'
$env:CAKE_SETTINGS_SKIPPACKAGEVERSIONCHECK = 'true'

# make sure we always get a fresh nuget-package and nothing from cache!
dotnet nuget locals all --clear

if(Test-Path .\tools) {
  rm -Force -Recurse .\tools
}

dotnet tool restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet cake @args
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
