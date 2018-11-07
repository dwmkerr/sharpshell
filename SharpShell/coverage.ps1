# Define the location of the xml report and html report.
$coverageDir = "$PSScriptRoot\artifacts\coverage" 
$coverageReport = "$coverageDir\coverage.xml"

# Create an artifacts directory and build the report.
New-Item -ItemType Directory -Force -Path "$PSScriptRoot\artifacts\coverage"
OpenCover.Console.exe "-target:nunit3-console.exe" `
    "-targetargs:$PSScriptRoot\SharpShell.Tests\bin\Release\SharpShell.Tests.dll" `
    "-filter:+[SharpShell*]* -[SharpShell.Tests*]*" `
    "-register:user" `
    "-output:$coverageReport"

# Create a local report.
reportgenerator "-reports:$coverageReport" "-targetdir:$coverageDir\html"

# Upload the report to codecov. The CODECOV_TOKEN env var must be set.
codecov -f "$coverageReport"

