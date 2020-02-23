using System;
using Microsoft.Win32;
using NUnit.Framework;
using SharpShell.Registry;

namespace SharpShell.Tests.Registry
{
    public class InMemoryRegistryTests
    {
        [Test]
        public void Can_Print_Simple_Structure()
        {
            //  Create an in-memory registry, some simple structure.
            var registry = new InMemoryRegistry();
            using (var key = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
            {
                var user = key.CreateSubKey("User");
                user.SetValue("Name", "Dave");
                user.SetValue("Number", 34);
                var address = key.CreateSubKey("Address");
                address.SetValue("City", "Singapore");
            }

            var print = registry.Print(RegistryView.Registry64);
            Assert.That(print, Is.EqualTo(
@"HKEY_CURRENT_USER
   Address
      City = Singapore
   User
      Name = Dave
      Number = 34"));
        }

        [Test]
        public void Can_assert_structure()
        {
            //  Create an in-memory registry, with a given structure.
            var registry = new InMemoryRegistry();
            registry.AddStructure(RegistryView.Registry32,
@"HKEY_CLASSES_ROOT
   .myp
      (Default) = MyProgram.1
   CLSID
      {00000000-1111-2222-3333-444444444444}
         InProcServer32
            (Default) = C:\MyDir\MyCommand.dll
            ThreadingModel = Apartment
      {11111111-2222-3333-4444-555555555555}
         InProcServer32
            (Default) = C:\MyDir\MyPropSheet.dll
            ThreadingModel = Apartment
   MyProgram.1
      (Default) = MyProgram Application
      ShellEx
         ContextMenuHandler
            MyCommand
               (Default) = {00000000-1111-2222-3333-444444444444}
         PropertySheetHandlers
            MyPropSheet
               (Default) = {11111111-2222-3333-4444-555555555555}");

            var print = registry.Print(RegistryView.Registry32);
            Assert.That(print, Is.EqualTo(
@"HKEY_CLASSES_ROOT
   .myp
      (Default) = MyProgram.1
   CLSID
      {00000000-1111-2222-3333-444444444444}
         InProcServer32
            (Default) = C:\MyDir\MyCommand.dll
            ThreadingModel = Apartment
      {11111111-2222-3333-4444-555555555555}
         InProcServer32
            (Default) = C:\MyDir\MyPropSheet.dll
            ThreadingModel = Apartment
   MyProgram.1
      (Default) = MyProgram Application
      ShellEx
         ContextMenuHandler
            MyCommand
               (Default) = {00000000-1111-2222-3333-444444444444}
         PropertySheetHandlers
            MyPropSheet
               (Default) = {11111111-2222-3333-4444-555555555555}"));
        }

        [Test]
        public void Set_Structure_Throws_With_Invalid_Indentation()
        {
            try
            {
                //  Indentation on line two is not a multiple of three.
                (new InMemoryRegistry()).AddStructure(RegistryView.Registry32,
@"HKEY_CLASSES_ROOT
    .myp
      (Default) = MyProgram.1");
                Assert.Fail("SetStructure should throw.");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Contains.Substring("multiple of three"));
            }
        }

        [Test]
        public void Set_Structure_Throws_With_Invalid_Hive()
        {
            try
            {
                //  Indentation on line two is not a multiple of three.
                (new InMemoryRegistry()).AddStructure(RegistryView.Registry32,
@"HKEY_USERS_ROOT
   .myp
      (Default) = MyProgram.1");
                Assert.Fail("SetStructure should throw.");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Contains.Substring("not a known registry hive"));
            }
        }

        [Test]
        public void Set_Structure_Throws_With_Invalid_Depth()
        {
            try
            {
                //  Indentation on line two is not a multiple of three.
                (new InMemoryRegistry()).AddStructure(RegistryView.Registry32,
@"HKEY_CLASSES_ROOT
   .myp
            (Default) = MyProgram.1");
                Assert.Fail("SetStructure should throw.");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Contains.Substring("invalid depth"));
            }
        }
    }
}