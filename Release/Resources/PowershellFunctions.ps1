# Copy items to a destination folder, creating the folder if needed.
function CopyItems($source, $destinationFolder) {
    
    # Create the any folders or subfolders up to the destination that don't exist.
    EnsureFolderExists($destinationFolder)

    # Now copy the items.
    Copy-Item $source -Destination $destinationFolder
}

# Ensures that a folder exists.
function EnsureFolderExists($folder) {

    # Create the any folders or subfolders up to the destination that don't exist.
    if (!(Test-Path -path $folder)) {
        New-Item $folder -Type Directory
    }
}

# Ensures that a folder exists and deletes anything in it.
function EnsureEmptyFolderExists($folder) {
    EnsureFolderExists($folder)
    Remove-Item -Recurse -Force $folder
    EnsureFolderExists($folder)
}