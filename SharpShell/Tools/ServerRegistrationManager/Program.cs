using System;
using System.Reflection;
using ServerRegistrationManager.OutputService;

namespace ServerRegistrationManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //  To keep things simple, we actually include the SharpShell assembly
            //  as a resource in the exe, then we can load it as required from the 
            //  resource. This leave us with one exe. Ilmerge won't work in this case
            //  because we'd lose the identity of SharpShell.dll (public key, names etc).
            HandleEmbeddedReferences();

            //  Create and run the application, use the ConsoleOutputService for output.
            var app = new Application(new ConsoleOutputService());
            app.Run(args);
        }

        /// <summary>
        /// Handles the embedded references.
        /// </summary>
        private static void HandleEmbeddedReferences()
        {
            //  When resolving an assembly...
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                //  ...load it from the EmbeddedReferences.

                //  Get the assembly name.
                var assemblyName = new AssemblyName(args.Name).Name;
                var executableName = typeof (Program).Assembly.GetName().Name;
                var resourceName = executableName + ".EmbeddedReferences." + assemblyName + ".dll";
                
                //  Load the resource.
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    //  If we cannot load the resource, the best we can do at this stage is to throw as informative exception as possible.
                    if (stream == null)
                    {
                        throw new InvalidOperationException($"Unable to load embedded assembly '{assemblyName}' from '{resourceName}'");
                    }

                    var assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }
    }
}
