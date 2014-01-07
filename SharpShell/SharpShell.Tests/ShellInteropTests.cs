using System;
using NUnit.Framework;
using SharpShell.Interop;

namespace SharpShell.Tests
{
    [TestFixture]
    public class ShellInteropTests
    {
        [Test]
        public void CanGetKnownFolderPath()
        {
            //  We must be able to get the documents known path without throwing an exception.
            string path;
            Shell32.SHGetKnownFolderPath(KnownFolders.FOLDERID_Documents, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero, out path);
            Assert.IsNotNullOrEmpty(path);
        }

        [Test]
        public void CanGetAndFreeKnownFolderIdList()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Cookies, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero, out pidl);
            Assert.IsTrue(pidl != IntPtr.Zero);
            Assert.DoesNotThrow(() => Shell32.ILFree(pidl));
        }
    }
}
