# IMPORTANT: Make sure that the path to msbuild is correct!  
$msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
if ((Test-Path $msbuild) -eq $false) {
    Write-Host "Cannot find msbuild at '$msbuild'."
    Break
}

# Load useful functions.
. .\Resources\PowershellFunctions.ps1

# Keep track of the 'release' folder location - it's the root of everything else.
# We can also build paths to the key locations we'll use.
$scriptParentPath = Split-Path -parent $MyInvocation.MyCommand.Definition
$folderReleaseRoot = $scriptParentPath
$folderSourceRoot = Split-Path -parent $folderReleaseRoot
$folderSolutionsRoot = Join-Path $folderSourceRoot "SharpShell"
$folderNuspecRoot = Join-Path $folderSourceRoot "release\nuspec"

# Part 1 - Build the solution
Write-Host "Preparing to build the SharpShell solution..."
$solutionCoreLibraries = Join-Path $folderSolutionsRoot "SharpShell.sln"
. $msbuild $solutionCoreLibraries /p:Configuration=Release /verbosity:minimal

# Part 2 - Get the version number of the core library, use this to build the destination release folder.
$folderBuild = Join-Path $folderSourceRoot "Build"
$releaseVersion = [Reflection.Assembly]::LoadFile((Join-Path $folderBuild "Core\SharpShell.dll")).GetName().Version
Write-Host "Built SharpShell. Release Version: $releaseVersion"

# Part 3 - Copy the core, tools and samples to the release.
$folderRelease = Join-Path $folderReleaseRoot $releaseVersion
Copy-Item "$folderBuild\Core" "$folderRelease\Core" -Force -Recurse
Copy-Item "$folderBuild\Samples" "$folderRelease\Samples" -Force -Recurse
Copy-Item "$folderBuild\Tools" "$folderRelease\Tools" -Force -Recurse

# Part 4 - Build the SharpShell Nuget Package
Write-Host "Preparing to build the SharpShell Nuget Package..."
$folderReleasePackage = Join-Path $folderRelease "Package"
EnsureEmptyFolderExists $folderReleasePackage
$nuget = Join-Path $scriptParentPath "Resources\nuget.exe"
CopyItems (Join-Path "$folderRelease\Core" "*.*") (Join-Path $folderNuspecRoot "SharpShell\lib\net40")
. $nuget pack (Join-Path $folderNuspecRoot "SharpShell\SharpShell.nuspec") -Version $releaseVersion -OutputDirectory $folderReleasePackage
$packagePath = (Join-Path $folderReleasePackage "SharpShell.$releaseVersion.nupkg")

# Part 5 - Build the SharpShell Tools Nuget Package
Write-Host "Preparing to build the SharpShell Tools Nuget Package..."
CopyItems (Join-Path "$folderRelease\Tools" "*.*") (Join-Path $folderNuspecRoot "SharpShellTools\lib")
. $nuget pack (Join-Path $folderNuspecRoot "SharpShellTools\SharpShellTools.nuspec") -Version $releaseVersion -OutputDirectory $folderReleasePackage
$packagePathTools = (Join-Path $folderReleasePackage "SharpShell.$releaseVersion.nupkg")

# Part 6 - Zip up the Core.
# TODO

# Part 7 - Zip up the Server Manager.
# TODO 

# Part 8 - Zip up the SRM.
# TODO

# We're done!
Write-Host "Successfully built version: $releaseVersion"

# If the user wants, we can also publish.
# . $nuget push $packagePath
# . $nuget push $packagePathTools