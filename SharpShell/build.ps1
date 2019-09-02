$agentPath = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe"
$devPath = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe"
$proPath = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe"
$communityPath = "${env:ProgramFiles(x86)}Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"

$msbuild = ""

If (Test-Path $agentPath) {
	$msbuild = $agentPath
} ElseIf (Test-Path $devPath) {
	$msbuild = $devPath
} ElseIf (Test-Path $proPath) {
	$msbuild = $proPath
} ElseIf (Test-Path $communityPath) {
	$msbuild = $communityPath
} Else {
	throw "Unable to find msbuild"
}

# Run msbuild on the solution, in release mode.
$args = "/p:Configuration=Release /t:Clean,Build SharpShell.sln"

# Run the command.
Write-Host "Running: ""$msbuild"" $args"
& "$msbuild" /p:Configuration=Release /t:Clean,Build SharpShell.sln
