using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpShell.ServerRegistration
{
    public enum FileType
    {
        /// <summary>
        /// The file is not an assembly or native dll.
        /// </summary>
        Unknown,

        /// <summary>
        /// The file is a native Windows DLL.
        /// </summary>
        NativeDll,

        /// <summary>
        /// The file is an assembly which targets the .NET Framework.
        /// </summary>
        DotNetFrameworkAssembly,

        /// <summary>
        /// The file is an assembly which targets .NET Core.
        /// </summary>
        DotNetCoreAssembly
    }

    /// <summary>
    /// Provides details on a file and the shell extensions it contains.
    /// </summary>
    public class FileShellExtensions
    {
        public FileShellExtensions(FileType fileType, Version version, ProcessorArchitecture processorArchitecture, string frameworkName)
        {
            FileType = fileType;
            Version = version;
            ProcessorArchitecture = processorArchitecture;
            FrameworkName = frameworkName;
        }

        /// <summary>
        /// The file type.
        /// </summary>
        public FileType FileType { get; }

        /// <summary>
        /// The assembly version. Only set if the <see cref="FileType"/> is an assembly.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// The processor architecture. If the file is not an assembly, this is set to <see cref="ProcessorArchitecture.None"/>. 
        /// </summary>
        public ProcessorArchitecture ProcessorArchitecture { get; }

        /// <summary>
        /// The frameowork name. Only set if the <see cref="FileType"/> is an assembly.
        /// </summary>
        public string FrameworkName { get; }
    }
}
