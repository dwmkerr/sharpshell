# Run msbuild on the solution, in release mode.
$msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe
$arguments = @("/t:Clean,Restore,Build", "/p:Configuration=Release", "$PSScriptRoot\SharpShell.sln")

# Run the command.
Write-Host "Running: $msbuild $arguments"
Start-Process -NoNewWindow -FilePath "$msbuild" -ArgumentList "$arguments"
