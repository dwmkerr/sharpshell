using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpShell.Attributes;

namespace SharpShell.ServerRegistration
{
    [Serializable]
    public sealed class SharpShellServerInfo
    {
        internal SharpShellServerInfo(Type type)
        {
            ClassFullName = type.FullName;
            ClassId = GetServerClassId(type);
            ClassName = type.Name;

            var displayNameAttribute = DisplayNameAttribute.GetDisplayNameAttribute(type);
            IsDisplayNameDefined = !string.IsNullOrEmpty(displayNameAttribute?.DisplayName);
            DisplayName = displayNameAttribute?.DisplayName ?? ClassName;

            var registrationNameAttribute = RegistrationNameAttribute.GetRegistrationNameAttribute(type);
            IsRegistrationNameDefined = !string.IsNullOrEmpty(registrationNameAttribute?.RegistrationName);
            RegistrationName = registrationNameAttribute?.RegistrationName ?? ClassName;

            var serverTypeAttribute = ServerTypeAttribute.GetServerTypeAttribute(type);
            ServerType = serverTypeAttribute?.ServerType ?? ServerType.None;
            ShellExtensionType = serverTypeAttribute?.ShellExtensionType ?? ShellExtensionType.None;

            var associationAttributes = COMServerAssociationAttribute.GetAssociationAttributes(type).ToArray();

            if (associationAttributes.Any())
            {
                AssociationClassNamesX32 = associationAttributes.SelectMany(attribute =>
                    attribute.GetAssociationClassNames(RegistrationScope.OS32Bit) ?? new string[0]).ToArray();
                AssociationClassNamesX64 = associationAttributes.SelectMany(attribute =>
                    attribute.GetAssociationClassNames(RegistrationScope.OS64Bit) ?? new string[0]).ToArray();
            }

            CustomRegistrationMethodName = CustomRegisterFunctionAttribute.GetMethodName(type);
            CustomUnRegistrationMethodName = CustomUnregisterFunctionAttribute.GetMethodName(type);

            AssemblyInfo = new ManagedAssemblyInfo(type);

            try
            {
                // Gets the latest SharpServer assembly loaded into memory
                var sharpShellAssembly = AppDomain.CurrentDomain.GetAssemblies().Reverse()
                    .FirstOrDefault(assembly => assembly.GetName().Name == nameof(SharpShell));

                if (sharpShellAssembly != null)
                {
                    SharpShellAssemblyInfo = new ManagedAssemblyInfo(sharpShellAssembly);
                }
            }
            catch
            {
                // ignored
            }
        }

        internal SharpShellServerInfo(Type type, string assemblyPath) : this(type)
        {
            AssemblyInfo = new ManagedAssemblyInfo(type, assemblyPath);
            IsAssemblyExternal = true;
        }

        public ManagedAssemblyInfo AssemblyInfo { get; }
        public string[] AssociationClassNamesX32 { get; } = new string[0];
        public string[] AssociationClassNamesX64 { get; } = new string[0];
        public string ClassFullName { get; }
        public Guid ClassId { get; }
        public string ClassName { get; }
        public string CustomRegistrationMethodName { get; }
        public string CustomUnRegistrationMethodName { get; }
        public string DisplayName { get; }
        public bool IsAssemblyExternal { get; }
        public bool IsDisplayNameDefined { get; }
        public bool IsRegistrationNameDefined { get; }
        public string RegistrationName { get; }
        public ServerType ServerType { get; }

        public ManagedAssemblyInfo SharpShellAssemblyInfo { get; }
        public ShellExtensionType ShellExtensionType { get; }

        public static IEnumerable<SharpShellServerInfo> FromAssembly(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => !type.IsAbstract && typeof(SharpShellServer).IsAssignableFrom(type))
                .Select(type => new SharpShellServerInfo(type));
        }

        public static IEnumerable<SharpShellServerInfo> FromAssembly(ManagedAssemblyInfo assembly)
        {
            // If file address is known load from file
            if (!string.IsNullOrEmpty(assembly.AssemblyPath))
            {
                return FromExternalAssemblyFile(assembly.AssemblyPath);
            }

            // If otherwise load from GAC
            return FromExternalAssembly(assembly.FullName);
        }

        public static IEnumerable<SharpShellServerInfo> FromExternalAssembly(string assemblyName)
        {
            AssemblyName name;

            try
            {
                // Throw if failed to create assembly name
                name = new AssemblyName(assemblyName);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid assembly name provided.", nameof(assemblyName), e);
            }

            return ServerSandBox.FromGAC(name);
        }

        public static IEnumerable<SharpShellServerInfo> FromExternalAssemblyFile(string assemblyPath)
        {
            // Normalize path
            assemblyPath = Path.GetFullPath(assemblyPath);

            var assemblyFile = new FileInfo(assemblyPath);

            // Throw if assembly file not found
            if (!assemblyFile.Exists)
            {
                throw new FileNotFoundException("Assembly file not found.");
            }

            return ServerSandBox.FromAssemblyFile(assemblyFile);
        }

        public static SharpShellServerInfo FromServer<T>() where T : SharpShellServer
        {
            return new SharpShellServerInfo(typeof(T));
        }

        public static Guid GetServerClassId<T>() where T : SharpShellServer
        {
            return GetServerClassId(typeof(T));
        }

        public static Guid GetServerClassId(Type type)
        {
            return type.GUID;
        }

        public void InvokeCustomRegisterMethodIfExists(RegistrationScope registrationScope)
        {
            // Return if there is no custom register method
            if (string.IsNullOrEmpty(CustomRegistrationMethodName))
            {
                return;
            }

            // Ask sandbox to invoke the method if this is an external assembly
            if (IsAssemblyExternal)
            {
                // Throw if assembly file not found
                if (string.IsNullOrWhiteSpace(AssemblyInfo?.AssemblyPath) || !File.Exists(AssemblyInfo.AssemblyPath))
                {
                    throw new FileNotFoundException("Assembly file not found.");
                }

                ServerSandBox.LoadAndInvokeCustomMethodFromFile(
                    new FileInfo(AssemblyInfo.AssemblyPath),
                    ClassId,
                    CustomRegistrationMethodName,
                    registrationScope
                );

                return;
            }

            // Otherwise invoke directly
            InvokeCustomMethod(ClassId, CustomRegistrationMethodName, registrationScope);
        }

        public void InvokeCustomUnRegisterMethodIfExists(RegistrationScope registrationScope)
        {
            // Return if there is no custom unregister method
            if (string.IsNullOrEmpty(CustomUnRegistrationMethodName))
            {
                return;
            }

            // Ask sandbox to invoke the method if this is an external assembly
            if (IsAssemblyExternal)
            {
                // Throw if assembly file not found
                if (string.IsNullOrWhiteSpace(AssemblyInfo?.AssemblyPath) || !File.Exists(AssemblyInfo.AssemblyPath))
                {
                    throw new FileNotFoundException("Assembly file not found.");
                }

                ServerSandBox.LoadAndInvokeCustomMethodFromFile(
                    new FileInfo(AssemblyInfo.AssemblyPath),
                    ClassId,
                    CustomUnRegistrationMethodName,
                    registrationScope
                );

                return;
            }

            // Otherwise invoke directly
            InvokeCustomMethod(ClassId, CustomUnRegistrationMethodName, registrationScope);
        }

        private void InvokeCustomMethod(Guid typeGuid, string methodName, RegistrationScope registrationScope)
        {
            var serverType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch
                    {
                        // ignored
                    }

                    return new Type[0];
                })
                .Where(type => typeof(SharpShellServer).IsAssignableFrom(type) && !type.IsAbstract)
                .FirstOrDefault(type => type.GUID == typeGuid);

            if (serverType == null)
            {
                throw new ArgumentException("Type not found.", nameof(typeGuid));
            }

            // Find method or throw if method was absent
            var methodInfo = serverType.GetMethod(
                methodName,
                BindingFlags.Static |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy
            );

            if (methodInfo == null)
            {
                throw new ArgumentException("Method not found.", nameof(methodInfo));
            }

            methodInfo.Invoke(null, new object[] {serverType, registrationScope});
        }
    }
}