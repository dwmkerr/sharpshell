using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using SharpShell.Interop;

namespace SharpShell.Tests
{
    [TestFixture]
    public class SharpShellServerTests
    {
        [Test]

        public void InvokeCOMRegisterFunctions()
        {
            // SharpShellServer implements ComRegisterFunction and ComUnregisterFunction as internal static.
            // In order to test the refactored implementation, access to the methods needs to be obtained via reflection.

            Type serverType = typeof(SharpShellServer);
            MethodInfo registerMethod   = serverType.GetMethod( "Register", BindingFlags.Static | BindingFlags.NonPublic );
            MethodInfo unregisterMethod = serverType.GetMethod( "Unregister", BindingFlags.Static | BindingFlags.NonPublic );

            // Invoke COMRegisterFunction and ComUnregisterFunction for the SharpContextMenu type.
            // The SharpContextMenu type does not define any associations.
            // Using it for invoking the registration metthods does not change anything in the Windows Registry,
            // but proofs, that the refactored registration functions are invoked and do not throw exceptions.

            Type extensionType = typeof(SharpContextMenu.SharpContextMenu);

            registerMethod.Invoke( null, new object[] { extensionType } );

            unregisterMethod.Invoke( null, new object[] { extensionType } );
        }
    }
}
