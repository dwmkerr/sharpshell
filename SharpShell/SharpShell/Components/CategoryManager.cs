using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.Interop;
using SharpShell.ServerRegistration;

namespace SharpShell.Components
{
    public static class CategoryManager
    {
        public static void RegisterComCategory(Guid clsid, Guid categoryId)
        {
            var catRegister = CreateCategoryRegistrationManager();
            catRegister.RegisterClassImplCategories(ref clsid, 1, new Guid[] {categoryId});
        }

        public static void RegisterComCategory(Guid clsid, Guid categoryId, RegistrationType registrationType)
        {
            //  Open the class ID key.
        }

        public static void UnregisterComCategory(Guid clsid, Guid categoryId)
        {
            var catRegister = CreateCategoryRegistrationManager();
            catRegister.UnRegisterClassImplCategories(ref clsid, 1, new Guid[] { categoryId });
        }

        private static ICatRegister CreateCategoryRegistrationManager()
        {
            var type = Type.GetTypeFromCLSID(CLSID_StdComponentCategoriesMgr);
            return (ICatRegister) Activator.CreateInstance(type);
        }

        private static Guid CLSID_StdComponentCategoriesMgr = new Guid(0x0002E005, 0x0000, 0x0000, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);

        public static Guid CATID_BrowsableShellExt  = new Guid(0x00021490, 0, 0, 0xC0,0,0,0,0,0,0,0x46);
        public static Guid CATID_BrowseInPlace      = new Guid(0x00021491, 0, 0, 0xC0,0,0,0,0,0,0,0x46);
        public static Guid CATID_DeskBand           = new Guid(0x00021492, 0, 0, 0xC0,0,0,0,0,0,0,0x46);
        public static Guid CATID_InfoBand           = new Guid(0x00021493, 0, 0, 0xC0,0,0,0,0,0,0,0x46);
        public static Guid CATID_CommBand           = new Guid(0x00021494, 0, 0, 0xC0,0,0,0,0,0,0,0x46);
    }
}
