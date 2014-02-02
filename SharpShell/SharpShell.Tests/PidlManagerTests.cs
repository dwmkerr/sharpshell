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
        public void CanGetPidlDisplayName()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Documents, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var displayName = PidlManager.GetPidlDisplayName(pidl);
            Shell32.ILFree(pidl);
            Assert.AreEqual(displayName, "Documents");
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
            var displayName = PidlManager.GetPidlDisplayName(pidl);
            Assert.AreEqual(displayName, "Documents");
        }

        [Test]
        public void CanIdentifyIdListLength()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Downloads, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var idList = PidlManager.PidlToIdlist(pidl);
            Assert.AreEqual(idList.Ids.Count, 4);
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
        }
    }
}