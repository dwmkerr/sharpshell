using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// TODO: Functions not yet defined.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E2-0000-0000-C000-000000000046")]
    public interface IShellBrowser : IOleWindow
    {}
}