# Create an artifacts directory and build the report.
New-Item -ItemType Directory -Force -Path "$PSScriptRoot\artifacts\coverage"
OpenCover.Console.exe "-target:nunit3-console.exe" `
    "-targetargs:$PSScriptRoot\SharpShell.Tests\bin\Release\SharpShell.Tests.dll" `
    "-filter:+[SharpShell*]* -[SharpShell.Tests*]*" `
    "-register:user" `
    "-output:$PSScriptRoot\artifacts\coverage\coverage.xml"

# Upload the report to codecov. The CODECOV_TOKEN env var must be set.
codecov -f "$PSScriptRoot\artifacts\coverage\coverage.xml"
