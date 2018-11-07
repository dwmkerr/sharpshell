using Microsoft.Win32;
using NUnit.Framework;
using SharpShell.Registry;

namespace SharpShell.Tests.Registry
{
    public class WindowsRegistryTests
    {
        [Test]
        public void Can_Open_A_Base_Key()
        {
            //  Try and open the users base key.
            var registry = new WindowsRegistry();
            using (var usersKey = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                Assert.That(usersKey.Name, Is.EqualTo("HKEY_CURRENT_USER"));
            }
        }
    }
}