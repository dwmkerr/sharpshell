# Run msbuild on the solution, in release mode.
$msbuild ="${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
$args = "SharpShell.sln /t:Rebuild /p:Configuration=Release"

# Run the command.
Write-Host "Running: ""$msbuild"" $args"
& "$msbuild" /p:Configuration=Release /t:Clean,Build SharpShell.sln