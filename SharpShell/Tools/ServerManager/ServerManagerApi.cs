using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharpShell;
using SharpShell.SharpPropertySheet;

namespace ServerManager
{
    /// <summary>
    /// Helper class for dealing with servers.
    /// </summary>
    public static class ServerManagerApi
    {
        /// <summary>
        /// Loads all SharpShell servers from an assembly.
        /// </summary>
        /// <param name="path">The path to the assembly.</param>
        /// <returns>A ServerEntry for each SharpShell server in the assembly.</returns>
        public static IEnumerable<ServerEntry> LoadServers(string path)
        {
            //  Storage for the servers.
            var servers = new List<ServerEntry>();

            try
            {
                //  Create an assembly catalog for the assembly and a container from it.
                var catalog = new AssemblyCatalog(Path.GetFullPath(path));
                var container = new CompositionContainer(catalog);

                //  Get all exports of type ISharpShellServer.
                var serverTypes = container.GetExports<ISharpShellServer>();

                //  Go through each servertype (creating the instance from the lazy).
                foreach(var serverTypeInstance in serverTypes.Select(st => st.Value))
                {
                    //  Yield a server entry for the server type.
                    servers.Add(new ServerEntry
                                          {
                                              ServerName = serverTypeInstance.DisplayName, 
                                              ServerPath = path,
                                              ServerType = serverTypeInstance.ServerType,
                                              ClassId = serverTypeInstance.ServerClsid,
                                              Server = serverTypeInstance
                                          });

                }
            }
            catch (Exception)
            {
                //  It's almost certainly not a COM server.
                MessageBox.Show("The file '" + Path.GetFileName(path) + "' is not a SharpShell Server.", "Warning");
            }

            //  Return the servers.
            return servers;
        }
    }
}
