using System;
using System.Runtime.InteropServices;
using SharpShell.Interop;

// ReSharper disable InconsistentNaming

namespace SharpShell.Components
{
    /// <summary>
    /// The CategoryManager class provides methods which allow us to register COM categories for components. It manages the
    /// instantiation of an <see cref="ICatRegister"/> type.
    /// </summary>
    internal static class CategoryManager
    {
        /// <summary>
        /// Registers a COM category for a given type.
        /// </summary>
        /// <param name="clsid">The CLSID.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <exception cref="ExternalException"></exception>
        public static void RegisterComCategory(Guid clsid, Guid categoryId)
        {
            var catRegister = CreateCategoryRegistrationManager();
            var result = catRegister.RegisterClassImplCategories(ref clsid, 1, new [] {categoryId});
            if (result != WinError.S_OK)
            {
                throw new ExternalException($"An exception occurred registering class {clsid} as an implementation of category {categoryId}");
            }
        }

        /// <summary>
        /// Unregisters a COM category for a given type.
        /// </summary>
        /// <param name="clsid">The CLSID.</param>
        /// <param name="categoryId">The category identifier.</param>
        public static void UnregisterComCategory(Guid clsid, Guid categoryId)
        {
            var catRegister = CreateCategoryRegistrationManager();
            var result = catRegister.UnRegisterClassImplCategories(ref clsid, 1, new [] { categoryId });
            if (result != WinError.S_OK)
            {
                throw new ExternalException($"An exception occurred registering class {clsid} as an implementation of category {categoryId}");
            }
        }

        /// <summary>
        /// Creates the category registration manager object.
        /// </summary>
        /// <returns>An <see cref="ICatRegister"/> instance.</returns>
        private static ICatRegister CreateCategoryRegistrationManager()
        {
            var type = Type.GetTypeFromCLSID(CLSID_StdComponentCategoriesMgr);
            return (ICatRegister) Activator.CreateInstance(type);
        }

        /// <summary>
        /// The CLSID for the standard component category manager.
        /// </summary>
        private static readonly Guid CLSID_StdComponentCategoriesMgr = new Guid(0x0002E005, 0x0000, 0x0000, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);

        /// <summary>
        /// Browsable shell extensions category id.
        /// </summary>
        public static readonly Guid CATID_BrowsableShellExt  = new Guid(0x00021490, 0, 0, 0xC0,0,0,0,0,0,0,0x46);

        /// <summary>
        /// Browse in place category id.
        /// </summary>
        public static readonly Guid CATID_BrowseInPlace      = new Guid(0x00021491, 0, 0, 0xC0,0,0,0,0,0,0,0x46);

        /// <summary>
        /// The desk band category id.
        /// </summary>
        public static readonly Guid CATID_DeskBand           = new Guid(0x00021492, 0, 0, 0xC0,0,0,0,0,0,0,0x46);

        /// <summary>
        /// The info band band category id.
        /// </summary>
        public static readonly Guid CATID_InfoBand           = new Guid(0x00021493, 0, 0, 0xC0,0,0,0,0,0,0,0x46);

        /// <summary>
        /// The Comm band category id.
        /// </summary>
        public static readonly Guid CATID_CommBand           = new Guid(0x00021494, 0, 0, 0xC0,0,0,0,0,0,0,0x46);
    }
}
