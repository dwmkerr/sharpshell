# Install dependencies.
choco install -y --no-progress opencover.portable codecov nunit-console-runner

# Create an artifacts directory and build the report.
New-Item -ItemType Directory -Force -Path .\artifacts\coverage
OpenCover.Console.exe "-target:nunit3-console.exe" "-targetargs:SharpShell.Tests\bin\Release\SharpShell.Tests.dll"  "-filter:+[SharpShell*]* -[SharpShell.Tests   *]*" "-register:user" "-output:artifacts/coverage/coverage.xml"

# Upload the report to codecov. The CODECOV_TOKEN env var must be set.
codecov -f .\artifacts\coverage\coverage.xml
