# Create an artifacts directory and build the report.
New-Item -ItemType Directory -Force -Path "$PSScriptRoot\artifacts\tests"

# Find all test assemblies.
$testAssemblies = Get-ChildItem -Include *.Tests.dll -Recurse | Where-Object {$_.FullName -like "*bin\Release*"}
$command = "nunit3-console.exe $testAssemblies --work=`"$PSScriptRoot\artifacts\tests`""
Write-Host "Running: `"$command`""
Invoke-Expression $command
