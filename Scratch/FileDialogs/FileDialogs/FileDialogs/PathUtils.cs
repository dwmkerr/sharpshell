using System;
using System.IO;

namespace FileDialogs
{
    internal static class PathUtils
    {
        /// <summary>
        /// Checks the specified path for invalid characters.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>true if the path contains invalid characters; otherwise false.</returns>
        internal static bool CheckInvalidPathChars(string path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                char ch = path[i];
                if (ch == '"' || ch == '<' || ch == '>' || ch == '|' || ch < 32)
                    return true;
            }

            return false;
        }

        internal static bool IsDirectorySeparator(char c)
        {
            if (c != Path.DirectorySeparatorChar)
                return (c == Path.AltDirectorySeparatorChar);

            return true;
        }

        internal static int GetRootLength(string path)
        {
            int rootLength = 0;
            int length = path.Length;
            if (length >= 1 && IsDirectorySeparator(path[0]))
            {
                rootLength = 1;
                if (length >= 2 && IsDirectorySeparator(path[1]))
                {
                    rootLength = 2;
                    int i = 2;
                    while (rootLength < length && ((
                        path[rootLength] != Path.DirectorySeparatorChar &&
                        path[rootLength] != Path.AltDirectorySeparatorChar) || (--i > 0)))
                    {
                        rootLength++;
                    }
                }
                return rootLength;
            }

            if (length >= 2 && path[1] == Path.VolumeSeparatorChar)
            {
                rootLength = 2;
                if (length >= 3 && IsDirectorySeparator(path[2]))
                    rootLength++;
            }

            return rootLength;
        }

        // Check if the specified path root is a drive
        internal static bool PathRootIsDrive(string path, out string driveName)
        {
            driveName = string.Empty;
            if (path == null || path.Length < 2)
                return false;

            if (CheckInvalidPathChars(path))
                return false;

            driveName = path.Substring(0, GetRootLength(path));
            if (driveName == null || driveName.Length == 0 || driveName.StartsWith(@"\\", StringComparison.Ordinal))
                return false;

            if (driveName.Length == 2 && driveName[1] == ':')
                driveName += @"\";

            char driveLetter = driveName[0];
            if ((driveLetter < 'A' || driveLetter > 'Z') && (driveLetter < 'a' || driveLetter > 'z'))
                return false;

            return true;
        }
    }
}
