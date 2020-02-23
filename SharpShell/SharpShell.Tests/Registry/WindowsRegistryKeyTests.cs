using System;
using Microsoft.Win32;
using NUnit.Framework;
using SharpShell.Registry;

namespace SharpShell.Tests.Registry
{
    /// <summary>
    /// These are functional tests for the Windows registry. They *do* have side effects, they'll manipulate registry
    /// keys under:
    ///   HKEY_CURRENT_USER\SharpShellTests
    /// This is a little unavoidable. However, the rest of the SharpShell tests use an InMemoryRegistry to avoid
    /// messing with the user's registry.
    /// </summary>
    public class WindowsRegistryKeyTests
    {
        [TearDown]
        public void TearDown()
        {
            //  Clean up the test key.
            var registry = new WindowsRegistry();
            using (var usersKey = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                usersKey.DeleteSubKeyTree("SharpShellTests", false); // don't throw if the key is already deleted.
            }
        }

        [Test]
        public void Can_Create_And_Delete_A_Subkey()
        {
            //  Open the unit test key.
            var registry = new WindowsRegistry();
            using (var usersKey = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            using (var subkey = usersKey.CreateSubKey("SharpShellTests"))
            {
                Assert.That(subkey.Name, Is.EqualTo(@"HKEY_CURRENT_USER\SharpShellTests"));
                usersKey.DeleteSubKeyTree("SharpShellTests");
                Assert.That(usersKey.GetSubKeyNames(), Does.Not.Contain("SharpShellTests"));
            }
        }

        [Test]
        public void Can_Set_Get_And_Delete_Values()
        {
            var registry = new WindowsRegistry();
            using (var usersKey = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            using (var subkey = usersKey.CreateSubKey("SharpShellTests"))
            {
                //  Set a value, check it is set, check unset values are not set.
                var testValue = new Guid().ToString();
                subkey.SetValue("TestString", testValue);
                Assert.That(subkey.GetValue("TestString"), Is.EqualTo(testValue));
                subkey.SetValue("TestExpandString", testValue, RegistryValueKind.ExpandString);

                //  Delete the value, and make sure it is no longer present.
                subkey.DeleteValue("TestString");
                Assert.That(subkey.GetValueNames(), Does.Not.Contain("TestString"));
                subkey.DeleteValue("TestExpandString");
                Assert.That(subkey.GetValueNames(), Does.Not.Contain("TestExpandString"));

                //  Make sure missing values are returned properly.
                Assert.That(subkey.GetValue("TestString"), Is.Null);
                Assert.That(subkey.GetValue("TestExpandString", "DefaultValue"), Is.EqualTo("DefaultValue"));

                //  If we delete a value which is missing, we should be able to choose whether to throw or now.
                Assert.DoesNotThrow(() => subkey.DeleteValue("TestString", false));
                Assert.Throws<ArgumentException>(() => subkey.DeleteValue("TestString", true));
            }
        }

        [Test]
        public void Open_Subkey_Returns_Null_For_Missing_Subkeys()
        {
            var registry = new WindowsRegistry();
            using (var usersKey = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            using (var subkey = usersKey.CreateSubKey("SharpShellTests"))
            {
                Assert.That(subkey.OpenSubKey("MissingSubkey"), Is.Null);
            }
        }
    }
}