using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using SharpShell.Helpers;
using SharpShell.Interop;

// ReSharper disable IdentifierTypo

namespace SharpShell.Tests.Helpers
{
    public class Win32HelperTests
    {
        [Test]
        public void LoWord_Should_Correctly_Unpack_Low_Word()
        {
            //  Pack 1024x768 into a width/height dword.
            var dimensions = new IntPtr(1024 + (768 << 16));
            
            //  Assert we can grab the loword.
            Assert.That(Win32Helper.LoWord(dimensions), Is.EqualTo(1024));
        }

        [Test]
        public void HiWord_Should_Correctly_Unpack_High_Word()
        {
            //  Pack 1024x768 into a width/height dword.
            var dimensions = new IntPtr(1024 + (768 << 16));

            //  Assert we can grab the loword.
            Assert.That(Win32Helper.HiWord(dimensions), Is.EqualTo(768));
        }

        [Test]
        public void IS_INTRESOURCE_Should_Correctly_Identify_An_Int_Resource()
        {
            //  Anything which has a zero high-word is assumed to be an int.
            var resource = new IntPtr((int)Math.Pow(2, 16) - 1);
            Assert.That(Win32Helper.IS_INTRESOURCE(resource), Is.True);
        }

        [Test]
        public void IS_INTRESOURCE_Should_Correctly_Identify_A_Non_Int_Resource()
        {
            //  Anything which has any bit set in the high word is assumed to not be an int.
            var resource = new IntPtr((int)Math.Pow(2, 16));
            Assert.That(Win32Helper.IS_INTRESOURCE(resource), Is.False);
        }

        [Test]
        public void IS_INTRESOURCE_Should_Correctly_Identify_The_Address_Of_A_String_As_A_Non_Int_Resource()
        {
            //  A string pointer should definitely be identified as an int resource.
            var resource = Marshal.StringToHGlobalUni("Resource Name");
            Assert.That(Win32Helper.IS_INTRESOURCE(resource), Is.False);
            Marshal.FreeHGlobal(resource);
        }
    }
}
