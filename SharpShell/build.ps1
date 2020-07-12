# Run msbuild on the solution, in release mode.
$msbuild = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
$arguments = "/t:Rebuild /p:Configuration=Release $PSScriptRoot\SharpShell.sln"

# Run the command.
Write-Host "Running: $msbuild $arguments"
Start-Process -NoNewWindow -FilePath "$msbuild" -ArgumentList "$arguments"

