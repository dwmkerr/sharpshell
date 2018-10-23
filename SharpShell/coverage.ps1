# Please ensure you have run:
choco install -y opencover.portable codecov

# Eun OpenCover to generate a report.
New-Item -ItemType Directory -Force -Path .\artifacts\coverage
OpenCover.Console.exe "-target:packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe" "-targetargs:SharpShell.Tests\bin\Release\SharpShell.Tests.dll"  "-filter:+[SharpShell*]* -[SharpShell.Tests   *]*" "-register:user" "-output:artifacts/coverage/coverage.xml"

# Upload the report to codecov. The CODECOV_TOKEN env var must be set.
codecov -f .\artifacts\coverage\coverage.xml