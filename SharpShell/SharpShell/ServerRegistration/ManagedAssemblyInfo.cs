using System;
using System.IO;
using System.Reflection;

namespace SharpShell.ServerRegistration
{
    [Serializable]
    public sealed class ManagedAssemblyInfo
    {
        private readonly string _assemblyPath;

        public ManagedAssemblyInfo(Assembly assembly)
        {
            FullName = assembly.FullName;
            Version = assembly.GetName().Version.ToString();
            RuntimeVersion = assembly.ImageRuntimeVersion;
            CodeBase = assembly.CodeBase;
            IsSigned = assembly.GetName().GetPublicKey()?.Length > 0;
        }

        public ManagedAssemblyInfo(Type type) : this(type.Assembly)
        {
        }

        internal ManagedAssemblyInfo(string fullName, string version, string runtimeVersion, string codeBase)
        {
            FullName = fullName;
            Version = version;
            RuntimeVersion = runtimeVersion;
            CodeBase = codeBase;

            try
            {
                if (!string.IsNullOrEmpty(AssemblyPath) && File.Exists(AssemblyPath))
                {
                    var assemblyName = AssemblyName.GetAssemblyName(AssemblyPath);
                    IsSigned = assemblyName.GetPublicKey().Length > 0;
                }
            }
            catch
            {
                // ignored
            }
        }

        internal ManagedAssemblyInfo(
            string fullName,
            string version,
            string runtimeVersion,
            string codeBase,
            bool isSigned)
        {
            FullName = fullName;
            Version = version;
            RuntimeVersion = runtimeVersion;
            CodeBase = codeBase;
            IsSigned = isSigned;
        }

        internal ManagedAssemblyInfo(
            string fullName,
            string version,
            string runtimeVersion,
            string codeBase,
            string assemblyPath)
        {
            FullName = fullName;
            Version = version;
            RuntimeVersion = runtimeVersion;
            CodeBase = codeBase;
            _assemblyPath = assemblyPath;

            try
            {
                if (!string.IsNullOrEmpty(AssemblyPath) && File.Exists(AssemblyPath))
                {
                    var assemblyName = AssemblyName.GetAssemblyName(AssemblyPath);
                    IsSigned = assemblyName.GetPublicKey().Length > 0;
                }
            }
            catch
            {
                // ignored
            }
        }

        internal ManagedAssemblyInfo(
            string fullName,
            string version,
            string runtimeVersion,
            string codeBase,
            bool isSigned,
            string assemblyPath) : this(fullName, version, runtimeVersion, codeBase, isSigned)
        {
            _assemblyPath = assemblyPath;
        }

        internal ManagedAssemblyInfo(string assemblyPath)
        {
            _assemblyPath = assemblyPath;

            try
            {
                if (!string.IsNullOrEmpty(AssemblyPath) && File.Exists(AssemblyPath))
                {
                    var assemblyName = AssemblyName.GetAssemblyName(AssemblyPath);
                    FullName = assemblyName.FullName;
                    Version = assemblyName.Version.ToString();
                    CodeBase = assemblyName.CodeBase;
                    IsSigned = assemblyName.GetPublicKey().Length > 0;
                    RuntimeVersion = "";
                }
            }
            catch
            {
                // ignored
            }
        }

        internal ManagedAssemblyInfo(Type type, string assemblyPath) : this(type)
        {
            _assemblyPath = assemblyPath;
        }

        public string AssemblyPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_assemblyPath))
                {
                    return _assemblyPath;
                }

                if (!string.IsNullOrEmpty(CodeBase))
                {
                    try
                    {
                        return new Uri(CodeBase).LocalPath;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                return null;
            }
        }

        public string CodeBase { get; }
        public string FullName { get; }
        public bool IsSigned { get; }
        public string RuntimeVersion { get; }
        public string Version { get; }
    }
}