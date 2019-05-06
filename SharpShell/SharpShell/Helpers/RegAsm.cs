using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SharpShell.Diagnostics;

namespace SharpShell.Helpers
{
    /// <summary>
    /// RegAsm provides a simple interface to find and run a locally installed 'regasm.exe' executable.
    /// </summary>
    public class RegAsm
    {
        /// <summary>
        /// The standard output from the most recent execution.
        /// </summary>
        public string StandardOutput { get; private set; }

        /// <summary>
        /// The standard error from the most recent execution.
        /// </summary>
        public string StandardError { get; private set; }

        /// <summary>
        /// Registers the given assembly, as 32 bit.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="codebase">if set to <c>true</c> set the codebase flag.</param>
        /// <returns><c>true</c> if registration succeeded, <c>false</c> otherwise.</returns>
        public bool Register32(string assemblyPath, bool codebase)
        {
            var flags = codebase ? "/codebase" : string.Empty;
            var args = $@"{flags} ""{assemblyPath}""";
            return Execute(FindRegAsmPath32(), args);
        }

        /// <summary>
        /// Registers the given assembly, as 64 bit.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="codebase">if set to <c>true</c> set the codebase flag.</param>
        /// <returns><c>true</c> if registration succeeded, <c>false</c> otherwise.</returns>
        public bool Register64(string assemblyPath, bool codebase)
        {
            var flags = codebase ? "/codebase" : string.Empty;
            var args = $"{flags} \"{assemblyPath}\"";
            return Execute(FindRegAsmPath64(), args);
        }

        /// <summary>
        /// Unregisters the given assembly, as 32 bit.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns><c>true</c> if unregistration succeeded, <c>false</c> otherwise.</returns>
        public bool Unregister32(string assemblyPath)
        {
            var args = $"/u \"{assemblyPath}\"";
            return Execute(FindRegAsmPath32(), args);
        }

        /// <summary>
        /// Unregisters the given assembly, as 64 bit.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns><c>true</c> if unregistration succeeded, <c>false</c> otherwise.</returns>
        public bool Unregister64(string assemblyPath)
        {
            var args = $"/u \"{assemblyPath}\"";
            return Execute(FindRegAsmPath64(), args);
        }

        private bool Execute(string regasmPath, string arguments)
        {
            var regasm = new Process
            {
                StartInfo =
                {
                    FileName = regasmPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            Logging.Log($@"RegAsm: preparing to run {regasmPath} {arguments}");
            regasm.Start();
            regasm.WaitForExit();
            StandardOutput = regasm.StandardOutput.ReadToEnd();
            StandardError = regasm.StandardError.ReadToEnd();
            Logging.Log($@"RegAsm: exited with code {regasm.ExitCode}");
            if(!string.IsNullOrEmpty(StandardOutput)) Logging.Log($@"RegAsm: Output: {StandardOutput}");
            if(!string.IsNullOrEmpty(StandardError)) Logging.Log($@"RegAsm: Error Output: {StandardError}");
            return regasm.ExitCode == 0;
        }

        private static string FindRegAsmPath32()
        {
            return FindRegAsmPath("Framework");
        }

        private static string FindRegAsmPath64()
        {
            return FindRegAsmPath("Framework64");
        }

        /// <summary>
        /// Finds the 'regasm.exe' path, from the given framework folder.
        /// </summary>
        /// <param name="frameworkFolder">The framework folder, which would normally be <code>%WINDIR%\Microsoft.NET\Framework</code> or
        /// <code>%WINDIR%\Microsoft.NET\Framework64</code>.</param>
        /// <returns>The path to the regasm.exe executable, from the most recent .NET Framework installation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a valid regasm path cannot be found.</exception>
        private static string FindRegAsmPath(string frameworkFolder)
        {
            //  This function essentially will set for folders inside the 'Framework' folder, then
            //  build a path to a hypothetical 'regasm.exe' in the folder:
            //
            //  C:\WINDOWS\Microsoft.Net\Framework\v1.0.3705\regasm.exe
            //  C:\WINDOWS\Microsoft.Net\Framework\v1.1.4322\regasm.exe
            //  C:\WINDOWS\Microsoft.Net\Framework\v2.0.50727\regasm.exe
            //  C:\WINDOWS\Microsoft.Net\Framework\v4.0.30319\regasm.exe
            //
            //  It will then sort descending, and pick the first regasm which actually exists, or return null.

            //  Build an array of candidate paths - these are paths which *might* point to a valid regasm executable.
            var searchRoot = Path.Combine(Environment.ExpandEnvironmentVariables("%WINDIR%"), @"Microsoft.Net", frameworkFolder);
            var frameworkDirectories = Directory.GetDirectories(searchRoot, "v*", SearchOption.TopDirectoryOnly);
            var candidates = frameworkDirectories.Select(c => Path.Combine(c, @"regasm.exe")).ToArray();

            //  Sort descending, i.e. we're shooting for the latest framework available.
            var sorted = candidates.OrderByDescending(s => s);

            //  Return the first element which exists, or null.
            var path = sorted.Where(File.Exists).FirstOrDefault();

            //  If we failed to find the path, boot an exception.
            if (path == null)
            {
                throw new InvalidOperationException($"Failed to find regasm in '{searchRoot}'. Checked: {Environment.NewLine + string.Join(Environment.NewLine, candidates)}");
            }

            return path;
        }
    }
}
