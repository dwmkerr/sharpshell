# Define build parameters:
$msbuild = "MSBuild.exe"
$solutionFile = "SharpShellNativeBridge.sln"

# Get the 'MSBuild.exe' path. On CI platforms (e.g. GitHub actions) it will
# already be in the path. If it's not, use vswhere to find it.
$msbuildPath = If (Get-Command $msbuild -ErrorAction SilentlyContinue) {
    # MSBuild.exe is avaialble from %PATH% - we are probably in CI...
    $msbuild
} Else {
    # Find MSBuild.exe using 'vswhere'...
    & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe
}
$arguments = @("/t:Clean,Restore,Build", "/p:Configuration=Release", "$PSScriptRoot\$solutionFile")

# Run the command.
Write-Host "Running: $msbuild $arguments"
Start-Process -Wait -NoNewWindow -FilePath "$msbuildPath" -ArgumentList "$arguments"