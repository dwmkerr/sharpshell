using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SharpShell.ServerRegistration
{
    internal class ServerSandBox : MarshalByRefObject
    {
        public static SharpShellServerInfo[] FromAssemblyFile(FileInfo assemblyFile)
        {
            // Get sandbox type
            var sandboxType = typeof(ServerSandBox);

            if (string.IsNullOrEmpty(sandboxType.FullName))
            {
                return null;
            }

            // Creating a new temporary domain
            var domain = AppDomain.CreateDomain("ServerTemporarySandboxedDomain");

            try
            {
                // Adding custom resolve function to load local version of SharpShell and other dependencies
                domain.AssemblyResolve += DomainOnAssemblyResolve;

                // Create and instance of this class inside the newly created domain
                var sandbox = (ServerSandBox) domain.CreateInstanceAndUnwrap(
                    sandboxType.Assembly.FullName,
                    sandboxType.FullName
                );

                // Load assembly
                var assemblyLocation = sandbox.InjectedLoadAssemblyFile(assemblyFile.FullName);

                // Get server information from inside of the newly created domain
                return sandbox.InjectedGetServersInformation(assemblyLocation);
            }
            catch (Exception e)
            {
                throw new BadImageFormatException("File is not a valid .Net library or failed to load the library.", e);
            }
            finally
            {
                // Unload the newly created domain to free up resource and close loaded assembly files
                AppDomain.Unload(domain);
            }
        }

        public static SharpShellServerInfo[] FromGAC(AssemblyName assemblyName)
        {
            // Get sandbox type
            var sandboxType = typeof(ServerSandBox);

            if (string.IsNullOrEmpty(sandboxType.FullName))
            {
                return null;
            }

            // Creating a new temporary domain
            var domain = AppDomain.CreateDomain("ServerTemporarySandboxedDomain");

            try
            {
                // Adding custom resolve function to load local version of SharpShell and other dependencies
                domain.AssemblyResolve += DomainOnAssemblyResolve;

                // Create and instance of this class inside the newly created domain
                var sandbox = (ServerSandBox) domain.CreateInstanceAndUnwrap(
                    sandboxType.Assembly.FullName,
                    sandboxType.FullName
                );

                // Load assembly
                var assemblyLocation = sandbox.InjectedLoadAssemblyGAC(assemblyName);

                // Get server information from inside of the newly created domain
                return sandbox.InjectedGetServersInformation(assemblyLocation);
            }
            catch (Exception e)
            {
                throw new BadImageFormatException("File is not a valid .Net library or failed to load the library.", e);
            }
            finally
            {
                // Unload the newly created domain to free up resource and close loaded assembly files
                AppDomain.Unload(domain);
            }
        }

        public static IEnumerable<object> GetAttributesSafe(
            MemberInfo memberInfo,
            string attributeTypeName,
            bool inherit)
        {
            foreach (var customAttribute in memberInfo.GetCustomAttributes(inherit))
            {
                var customAttributeType = customAttribute?.GetType();

                while (customAttributeType != null && customAttributeType.Name != nameof(Attribute))
                {
                    if (customAttributeType.Name == attributeTypeName)
                    {
                        yield return customAttribute;

                        continue;
                    }

                    customAttributeType = customAttributeType.BaseType;
                }
            }
        }

        public static IEnumerable<object> GetAttributesSafe(Type type, string attributeTypeName, bool inherit)
        {
            foreach (var customAttribute in type.GetCustomAttributes(inherit))
            {
                var customAttributeType = customAttribute?.GetType();

                while (customAttributeType != null && customAttributeType.Name != nameof(Attribute))
                {
                    if (customAttributeType.Name == attributeTypeName)
                    {
                        yield return customAttribute;

                        break;
                    }

                    customAttributeType = customAttributeType.BaseType;
                }
            }
        }

        public static T GetByRefPropertySafe<T>(object obj, string propertyName) where T : class
        {
            if (obj == null)
            {
                return null;
            }

            try
            {
                var property = obj.GetType().GetProperty(propertyName);

                if (property != null)
                {
                    return (T) property.GetValue(obj, null);
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public static T? GetByValPropertySafe<T>(object obj, string propertyName) where T : struct
        {
            if (obj == null)
            {
                return null;
            }

            try
            {
                var property = obj.GetType().GetProperty(propertyName);

                if (property != null)
                {
                    return (T) property.GetValue(obj, null);
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public static string GetStringPropertySafe(object obj, string propertyName)
        {
            if (obj == null)
            {
                return null;
            }

            try
            {
                var property = obj.GetType().GetProperty(propertyName);

                if (property != null)
                {
                    return property.GetValue(obj, null)?.ToString();
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public static void LoadAndInvokeCustomMethodFromFile(
            FileInfo assemblyFile,
            Guid typeGuid,
            string methodName,
            RegistrationScope registrationScope)
        {
            // Get sandbox type
            var sandboxType = typeof(ServerSandBox);

            if (string.IsNullOrEmpty(sandboxType.FullName))
            {
                return;
            }

            // Creating a new temporary domain
            var domain = AppDomain.CreateDomain("ServerTemporarySandboxedDomain");

            try
            {
                // Adding custom resolve function to load local version of SharpShell and other dependencies
                domain.AssemblyResolve += DomainOnAssemblyResolve;

                // Create and instance of this class inside the newly created domain
                var sandbox = (ServerSandBox) domain.CreateInstanceAndUnwrap(
                    sandboxType.Assembly.FullName,
                    sandboxType.FullName
                );

                // Load assembly
                var assemblyLocation = sandbox.InjectedLoadAssemblyFile(assemblyFile.FullName);

                // Get server information from inside of the newly created domain
                sandbox.InjectedInvokeServerCustomStaticMethod(
                    assemblyLocation,
                    typeGuid,
                    methodName,
                    registrationScope
                );
            }
            catch (Exception e)
            {
                throw new BadImageFormatException("File is not a valid .Net library or failed to load the library.", e);
            }
            finally
            {
                // Unload the newly created domain to free up resource and close loaded assembly files
                AppDomain.Unload(domain);
            }
        }

        private static Assembly DomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Extract dll name and check if it is valid
            var dllName = args.Name.Split(',').FirstOrDefault();

            if (string.IsNullOrWhiteSpace(dllName))
            {
                return null;
            }

            // Create an array of search paths
            var possibleDirectories = new[]
            {
                Path.GetDirectoryName(args.RequestingAssembly.Location),
                Path.GetDirectoryName(AppDomain.CurrentDomain.RelativeSearchPath),
                Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)
            }.Where(s => !string.IsNullOrEmpty(s));

            // Create an array of valid file names
            var possibleFile = new[]
            {
                dllName + ".dll",
                Path.Combine(dllName, dllName + ".dll")
            };

            // Load dependency if found
            foreach (var directory in possibleDirectories)
            {
                foreach (var file in possibleFile)
                {
                    try
                    {
                        var fullAddress = Path.Combine(directory, file);

                        if (File.Exists(fullAddress))
                        {
                            return Assembly.LoadFrom(fullAddress);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            // Failed to load dependency
            return null;
        }

        private SharpShellServerInfo[] InjectedGetServersInformation(string assemblyPath)
        {
            // Load and get all server types
            var serverTypes = InjectedGetServerTypes(assemblyPath);

            // Go through server types and extract server info
            var servers = new List<SharpShellServerInfo>();

            foreach (var serverType in serverTypes)
            {
                try
                {
                    servers.Add(new SharpShellServerInfo(serverType, assemblyPath));
                }
                catch
                {
                    // ignored
                }
            }

            return servers.ToArray();
        }

        private Type[] InjectedGetServerTypes(string assemblyPath)
        {
            // Get loaded assembly
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.Location == assemblyPath);

            if (assembly == null)
            {
                return new Type[0];
            }

            // Finding types that inherit from SharpShellServer
            var serverTypes = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                // Ignore invalid types
                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                var baseType = type.BaseType;

                while (baseType != null && baseType.Name != nameof(Object))
                {
                    if (baseType.Name == nameof(SharpShellServer))
                    {
                        serverTypes.Add(type);

                        break;
                    }

                    baseType = baseType.BaseType;
                }
            }

            return serverTypes.ToArray();
        }

        private void InjectedInvokeServerCustomStaticMethod(
            string assemblyPath,
            Guid typeGuid,
            string methodName,
            RegistrationScope registrationScope)
        {
            // Load and get all server types
            var serverTypes = InjectedGetServerTypes(assemblyPath);

            // Find server and throw if server type was absent
            var serverType = serverTypes.FirstOrDefault(type => type.GUID == typeGuid);

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

            // Invoke method with server type and registration info
            methodInfo.Invoke(null, new object[] {serverType, registrationScope});
        }

        private string InjectedLoadAssemblyFile(string assemblyPath)
        {
            // Actually load the assembly
            return Assembly.LoadFile(assemblyPath).Location;
        }

        private string InjectedLoadAssemblyGAC(AssemblyName assemblyName)
        {
            // Actually load the assembly
            return Assembly.Load(assemblyName).Location;
        }
    }
}