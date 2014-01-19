using System;
using System.Linq;
using System.Text;
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
            var dt = PidlManager.GetDesktop();
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Cookies, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero, out pidl);
            var idList = PidlManager.Decode(pidl);
            Shell32.ILFree(pidl);

            Assert.Fail();
        }

        [Test]
        public void CanIdenitfyRelativePidl()
        {
            Assert.Fail();
        }

        [Test]
        public void CanDecodeFilesystemPidl()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Documents, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var idList = PidlManager.PidlToIdlist(pidl);
            Shell32.ILFree(pidl);
        }

        [Test]
        public void CanBouncePidl()
        {

            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Documents, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var idList = PidlManager.PidlToIdlist(pidl);
            Shell32.ILFree(pidl);
            pidl = PidlManager.IdListToPidl(idList);
            var pszPath = new StringBuilder();
            var name = Shell32.SHGetPathFromIDList(pidl, pszPath);
            //  todo assert mydocs

        }

        [Test]
        public void CanIdentifyIdListLength()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Downloads, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var idList = PidlManager.PidlToIdlist(pidl);

            //  Compare the shell length to the PidlManager length.
        }


        [Test]
        public void CanFullRoundTripPidl()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Downloads, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var idList = PidlManager.PidlToIdlist(pidl);
            var pidl2 = PidlManager.IdListToPidl(idList);
            var idList2 = PidlManager.PidlToIdlist(pidl2);
            
            Assert.IsTrue(idList.Matches(idList2));

            //  Compare the shell length to the PidlManager length.
        }
    }
}