using System;
using Microsoft.Win32;
using NUnit.Framework;
using SharpShell.Registry;
using SharpShell.ServerRegistration;

namespace SharpShell.Tests.ServerRegistration
{
    public class FileExtensionClassTests
    {
        [Test]
        [TestCase(null)]        // null ain't value
        [TestCase("no_dot")]    // missing a '.' at the beginning
        [TestCase(".")]         // no actual extension
        public void Invalid_Exceptions_Throw_On_Get(string extension)
        {
            try
            {
                var registry = new InMemoryRegistry();
                FileExtensionClass.Get(registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default), extension, false);
                Assert.Fail($@"An exception should be thrown for extension '{extension}'");
            }
            catch (Exception exception)
            {
                //  We should have an exception which includes the invalid extension in the message.
                if(!string.IsNullOrEmpty(extension))
                    Assert.That(exception.Message, Contains.Substring(extension));
            }
        }

        [Test]
        public void Get_Returns_Null_For_Missing_Extensions()
        {
            //  Given an empty registry, we should return null for the class of any extension.
            var registry = new InMemoryRegistry();
            var className = FileExtensionClass.Get(registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default), ".some_type", false);
            Assert.That(className, Is.Null);
        }
    }
}
