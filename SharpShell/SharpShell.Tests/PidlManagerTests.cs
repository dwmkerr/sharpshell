using System;
using NUnit.Framework;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace SharpShell.Tests
{
    public class PidlManagerTests
    {
        [Test]
        public void CanDecodePidl()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Cookies, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero, out pidl);
            var idList = PidlManager.Decode(pidl);
            Shell32.ILFree(pidl);

            //  TODO: need to validate the pidl, a bounce would do it.
        }

        [Test]
        public void CanIdentifyAbsolutePidl()
        {
            Assert.Fail();
        }

        [Test]
        public void CanIdenitfyRelativePidl()
        {
            Assert.Fail();
        }
    }
}