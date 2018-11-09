using System;
using Microsoft.Win32;
using NUnit.Framework;
using SharpShell.Registry;

namespace SharpShell.Tests.Registry
{
    public class InMemoryRegistryKeyTests
    {
        [Test]
        public void Can_Get_And_Set_Default_Values()
        {
            //  Bounce a default value, which has slightly different treatment in the code.
            var registry = new InMemoryRegistry();
            using (var key = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
            {
                key.SetValue(null, @"Default");
                Assert.That(key.GetValue(null), Is.EqualTo(@"Default"));
                Assert.That(key.GetValue(null, "(This is used if missing)"), Is.EqualTo(@"Default"));
            }
        }

        [Test]
        public void Open_SubKey_Returns_Null_For_Missing_Keys()
        {
            var registry = new InMemoryRegistry();
            using (var key = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
            {
                var subkey = key.OpenSubKey("Missing");
                Assert.That(subkey, Is.Null);
            }
        }

        [Test]
        public void Can_Create_Deep_SubKey()
        {
            var registry = new InMemoryRegistry();
            using (var key = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
            {
                var subkey = key.OpenSubKey(@"Deep\Subkey");
                Assert.That(subkey, Is.Null);
            }
        }
    }
}