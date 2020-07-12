# Create the artifacts packages folder.
$artifactsFolder = "$PSScriptRoot/artifacts/packages"

if (Test-Path $artifactsFolder) { Remove-Item $artifactsFolder -Recurse -Force }
New-Item -Path $artifactsFolder -ItemType directory

# Package each of our projects. This must be run *after* ./build.ps1.
dotnet pack --no-restore --no-build "$PSScriptRoot/SharpShell.csproj" -c:Release

# Copy over the packages.
Get-ChildItem "$PSScriptRoot" -Include SharpGL*.nupkg -Recurse | Copy-Item -Destination $artifactsFolder 
