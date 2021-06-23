# Define the location of the xml report and html report.
$coverageDir = "$PSScriptRoot\artifacts\coverage" 
$coverageReport = "$coverageDir\coverage.xml"

# Get the set of test assemblies and the work folder.
$testAssemblies = Get-ChildItem -Include *.Tests.dll -Recurse | Where-Object {$_.FullName -like "*bin\Release*"}
$workArgs = "$PSScriptRoot\artifacts\tests"

# Create an artifacts directory create the command to build the report.
New-Item -ItemType Directory -Force -Path "$PSScriptRoot\artifacts\coverage"
$command = "OpenCover.Console.exe -target:nunit3-console.exe -targetargs:`"$testAssemblies --work=$workArgs`" -filter:`"+[SharpGL.SceneGraph*]* -[SharpGL.SceneGraph.Tests*]*`" -register:user -output:$coverageReport"
Write-Host "Running: `"$command`""
Invoke-Expression $command

# Create a local report.
reportgenerator "-reports:$coverageReport" "-targetdir:$coverageDir\html"

# Upload the report to codecov. The CODECOV_TOKEN env var must be set.
codecov -f "$coverageReport"

