using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using SharpShell.Attributes;
using SharpShell.Extensions;
using SharpShell.Registry;
using SharpShell.ServerRegistration;

namespace SharpShell.Tests.ServerRegistration
{
    public class ServerRegistrationManagerTests
    {
        private InMemoryRegistry _registry;

        [SetUp]
        public void SetUp()
        {
            //  Create a test registry, register in the service registry.
            _registry = new InMemoryRegistry();
            SharpShell.ServiceRegistry.ServiceRegistry.RegisterService<IRegistry>(() => _registry);
        }

        [TearDown]
        public void TearDown()
        {
            //  Reset the service registry.
            SharpShell.ServiceRegistry.ServiceRegistry.Reset();
        }

        [Test]
        public void Register_Server_Associations_Uses_Appropriate_Class_Id_For_Class_Of_Extension()
        {
            //  Pretty important test. Given we have a file extension in the registry, assert that we
            //  register an extension with the appropriate ProgID.

            //  Prime the registry with a progid for *.exe files.
            _registry.AddStructure(RegistryView.Registry64, string.Join(Environment.NewLine, 
                @"HKEY_CLASSES_ROOT",
                @"   .exe",
                @"      (Default) = exefile",
                @"      Content Type = application/x-msdownload",
                @"   exefile",
                @"      (Default) = Application"
                ));

            //  Register a context menu with an *.exe association.
            var clsid = new Guid("00000000-1111-2222-3333-444444444444");
            var serverType = ServerType.ShellContextMenu;
            var serverName = "TestContextMenu";
            var associations = new[] { new COMServerAssociationAttribute(AssociationType.ClassOfExtension, ".exe") };
            var registrationType = RegistrationType.OS64Bit;
            ServerRegistrationManager.RegisterServerAssociations(clsid, serverType, serverName, associations, registrationType);

            //  Assert we have the new extention.
            var print = _registry.Print(RegistryView.Registry64);
            Assert.That(print, Is.EqualTo(string.Join(Environment.NewLine,
                @"HKEY_CLASSES_ROOT",
                @"   .exe",
                @"      (Default) = exefile",
                @"      Content Type = application/x-msdownload",
                @"   exefile",
                @"      (Default) = Application",
                @"      ShellEx",
                @"         ContextMenuHandlers",
                @"            TestContextMenu",
                @"               (Default) = {00000000-1111-2222-3333-444444444444}")
            ));
        }

        [Test]
        public void Register_Server_Associations_Creates_Class_Ids_For_Extension_Of_Class_If_Needed()
        {
            //  Prime the registry with a *.myfile extension but with no class id.
            _registry.AddStructure(RegistryView.Registry64, string.Join(Environment.NewLine,
                @"HKEY_CLASSES_ROOT",
                @"   .myfile",                    // note that myfile has no class id...
                @"      Content Type = text"
            ));

            //  Register a file association. Given that this registry is empty, it a new program id and then set an association.
            var clsid = new Guid("00000000-1111-2222-3333-444444444444");
            var serverType = ServerType.ShellContextMenu;
            var serverName = "TestContextMenu";
            var associations = new[] { new COMServerAssociationAttribute(AssociationType.ClassOfExtension, ".myfile") };
            var registrationType = RegistrationType.OS64Bit;
            ServerRegistrationManager.RegisterServerAssociations(clsid, serverType, serverName, associations, registrationType);

            //  Assert we have the new extention.
            var print = _registry.Print(RegistryView.Registry64);
            Assert.That(print, Is.EqualTo(string.Join(Environment.NewLine,
                @"HKEY_CLASSES_ROOT",
                @"   .myfile",
                @"      (Default) = myfile.1",
                @"      Content Type = text",
                @"   myfile.1",
                @"      (Default) = myfile Application",
                @"      ShellEx",
                @"         ContextMenuHandlers",
                @"            TestContextMenu",
                @"               (Default) = {00000000-1111-2222-3333-444444444444}")
            ));
        }

        [Test]
        public void Register_Server_Associations_Fails_If_Class_Ids_For_Extension_Of_Class_Is_Already_In_Use()
        {
            //  Prime the registry with a *.myfile extension but with no class id. Also set up a value
            //  for the class id we would like to use - i.e. prepare for a clash. This is a pretty specific
            //  test but covers a potentially nasty edge case. We *could* try and dynamically generate a new
            //  class id (myfile.2, myfile.3) and so on, but this would be pretty confusing and makes some
            //  assumptions. Seems safer to fail with a clear reason in this case and ask the user to check
            //  their registry. Developers might need to build a custom installer action if this actually hits
            //  them, but it seemse pretty unlikely.
            _registry.AddStructure(RegistryView.Registry64, string.Join(Environment.NewLine,
                @"HKEY_CLASSES_ROOT",
                @"   .myfile",                    // note that myfile has no class id, we will want to use myfile.1...
                @"      Content Type = text",
                @"   myfile.1",                   // ...uh-oh - we will want to create myfile.1, but someone else is using it.
                @"      (Default) = Clashing Application"
            ));
            
            try
            {
                //  Register a server.
                var clsid = new Guid("00000000-1111-2222-3333-444444444444");
                var serverType = ServerType.ShellContextMenu;
                var serverName = "TestContextMenu";
                var associations = new[] { new COMServerAssociationAttribute(AssociationType.ClassOfExtension, ".myfile") };
                var registrationType = RegistrationType.OS64Bit;
                ServerRegistrationManager.RegisterServerAssociations(clsid, serverType, serverName, associations, registrationType);
                Assert.Fail(@"Server registration should fail");
            }
            catch (Exception exception)
            {
                Assert.That(exception.Message, Contains.Substring("myfile.1"), @"The exception message must contain the failing class id.");

                //  TODO: I would like to see a log message asserted here, such as:
                //  Assert.That(mostRecentLog, Contains.Substring("myfile.1"));
                //  It might be worth considering how ILogger can work with ServiceRegistry, to simplify logging
                //  code and make testing like this easier.
            }
        }

        [Test]
        public void Register_Server_Associations_Creates_File_Id_And_Class_Ids_For_Extension_Of_Class_If_Needed()
        {
            //  Register a file association. Given that this registry is empty, it a new program id and then set an association.
            var clsid = new Guid("00000000-1111-2222-3333-444444444444");
            var serverType = ServerType.ShellContextMenu;
            var serverName = "TestContextMenu";
            var associations = new[] {new COMServerAssociationAttribute(AssociationType.ClassOfExtension, ".myfile")};
            var registrationType = RegistrationType.OS64Bit;
            ServerRegistrationManager.RegisterServerAssociations(clsid, serverType, serverName, associations, registrationType);

            //  Assert we have the new extention.
            var print = _registry.Print(RegistryView.Registry64);
            Assert.That(print, Is.EqualTo(string.Join(Environment.NewLine,
                @"HKEY_CLASSES_ROOT",
                @"   .myfile",
                @"      (Default) = myfile.1",
                @"   myfile.1",
                @"      (Default) = myfile Application",
                @"      ShellEx",
                @"         ContextMenuHandlers",
                @"            TestContextMenu",
                @"               (Default) = {00000000-1111-2222-3333-444444444444}")
            ));
        }
    }
}
