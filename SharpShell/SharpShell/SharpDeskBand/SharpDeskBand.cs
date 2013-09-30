using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Interop;

namespace SharpShell.SharpDeskBand
{
    [ServerType(ServerType.ShellDeskBand)]
    public class SharpDeskBand : SharpShellServer, IDeskBand, IPersistStream, IObjectWithSite
    {
    }
}
