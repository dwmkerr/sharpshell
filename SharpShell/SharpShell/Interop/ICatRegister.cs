using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// provides methods for registering and unregistering component category 
    /// information in the registry. This includes both the human-readable 
    /// names of categories and the categories implemented or required by a 
    /// given component or class.
    /// </summary>
    [ComImport, Guid("0002E012-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(false)]
    public interface ICatRegister
    {
        /// <summary>
        /// Registers one or more component categories. Each component category 
        /// consists of a CATID and a list of locale-dependent description strings.
        /// </summary>
        /// <param name="cCategories">The number of component categories to register.</param>
        /// <param name="rgCategoryInfo">
        /// The array of cCategories CATEGORYINFO structures. By providing the same 
        /// CATID for multiple CATEGORYINFO structures, multiple locales can be 
        /// registered for the same component category. 
        /// </param>
        [PreserveSig]
        int RegisterCategories(uint cCategories, [In, MarshalAs(UnmanagedType.LPArray)] CATEGORYINFO[] rgCategoryInfo);

        /// <summary>
        /// Removes the registration of one or more component categories. Each component 
        /// category consists of a CATID and a list of locale-dependent description strings.
        /// </summary>
        /// <param name="cCategories">The number of cCategories CATIDs to be removed.</param>
        /// <param name="rgcatid">Identifies the categories to be removed.</param>
        [PreserveSig]
        int UnRegisterCategories(uint cCategories, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        /// <summary>
        /// Registers the class as implementing one or more component categories.
        /// </summary>
        /// <param name="rclsid">The class ID of the relevent class for which category information will be set.</param>
        /// <param name="cCategories">The number of categories to associate as category identifiers for the class.</param>
        /// <param name="rgcatid">The array of cCategories CATIDs to associate as category identifiers for the class.</param>
        [PreserveSig]
        int RegisterClassImplCategories([In] ref Guid rclsid, uint cCategories, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        /// <summary>
        /// Removes one or more implemented category identifiers from a class.
        /// </summary>
        /// <param name="rclsid">The class ID of the relevant class to be manipulated.</param>
        /// <param name="cCategories">The number of category CATIDs to remove.</param>
        /// <param name="rgcatid">The array of cCategories CATID that are to be removed. Only the category IDs specified in this array are removed.</param>
        [PreserveSig]
        int UnRegisterClassImplCategories([In] ref Guid rclsid, uint cCategories, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        /// <summary>
        /// Registers the class as requiring one or more component categories.
        /// </summary>
        /// <param name="rclsid">The class ID of the relevent class for which category information will be set.</param>
        /// <param name="cCategories">The number of category CATIDs to associate as category identifiers for the class.</param>
        /// <param name="rgcatid">The array of cCategories CATID to associate as category identifiers for the class.</param>
        [PreserveSig]
        int RegisterClassReqCategories([In] ref Guid rclsid, uint cCategories, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);

        /// <summary>
        /// Removes one or more required category identifiers from a class.
        /// </summary>
        /// <param name="rclsid">The class ID of the relevent class to be manipulated.</param>
        /// <param name="cCategories">The number of category CATIDs to remove.</param>
        /// <param name="rgcatid">The array of cCategories CATID that are to be removed. Only the category IDs specified in this array are removed.</param>
        [PreserveSig]
        int UnRegisterClassReqCategories([In] ref Guid rclsid, uint cCategories, [In, MarshalAs(UnmanagedType.LPArray)] Guid[] rgcatid);
    }
}