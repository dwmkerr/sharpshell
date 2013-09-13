using System;
using System.Runtime.InteropServices.ComTypes;

namespace SharpShell 
{
    /// <summary>
    /// PersistFileServer provides a base for SharpShell Servers that must implement
    /// IPersistFile (icon handlers, info tip handlers, etc).
    /// </summary>
    public abstract class PersistFileServer : SharpShellServer, IPersistFile
    {
        #region Implementation of IPersistFile

        /// <summary>
        /// Retrieves the class identifier (CLSID) of an object.
        /// </summary>
        /// <param name="pClassID">When this method returns, contains a reference to the CLSID. This parameter is passed uninitialized.</param>
        void IPersistFile.GetClassID(out Guid pClassID)
        {
            //  The shell is asking for the CLSID of the COM server - this is the
            //  GUID of the object, and typically defined as a GUID attribute on
            //  the derived class.
            pClassID = GetType().GUID;
        }

        /// <summary>
        /// Checks an object for changes since it was last saved to its current file.
        /// </summary>
        /// <returns>
        /// S_OK if the file has changed since it was last saved; S_FALSE if the file has not changed since it was last saved.
        /// </returns>
        int IPersistFile.IsDirty()
        {
            //  Not needed for shell extensions.
            return 0;
        }

        /// <summary>
        /// Opens the specified file and initializes an object from the file contents.
        /// </summary>
        /// <param name="pszFileName">A zero-terminated string containing the absolute path of the file to open.</param>
        /// <param name="dwMode">A combination of values from the STGM enumeration to indicate the access mode in which to open <paramref name="pszFileName"/>.</param>
        void IPersistFile.Load(string pszFileName, int dwMode)
        {
            //  Store the file path.
            SelectedItemPath = pszFileName;
        }

        /// <summary>
        /// Saves a copy of the object into the specified file.
        /// </summary>
        /// <param name="pszFileName">A zero-terminated string containing the absolute path of the file to which the object is saved.</param>
        /// <param name="fRemember">true to used the <paramref name="pszFileName"/> parameter as the current working file; otherwise false.</param>
        void IPersistFile.Save(string pszFileName, bool fRemember)
        {
            //  Not needed for shell extensions.
        }

        /// <summary>
        /// Notifies the object that it can write to its file.
        /// </summary>
        /// <param name="pszFileName">The absolute path of the file where the object was previously saved.</param>
        void IPersistFile.SaveCompleted(string pszFileName)
        {
            //  Not needed for shell extensions.
        }

        /// <summary>
        /// Retrieves either the absolute path to the current working file of the object or, if there is no current working file, the default file name prompt of the object.
        /// </summary>
        /// <param name="ppszFileName">When this method returns, contains the address of a pointer to a zero-terminated string containing the path for the current file, or the default file name prompt (such as *.txt). This parameter is passed uninitialized.</param>
        void IPersistFile.GetCurFile(out string ppszFileName)
        {
            //  Not needed for shell extensions.
            ppszFileName = null;
        }

        #endregion
        
        /// <summary>
        /// Gets the selected item path.
        /// </summary>
        public string SelectedItemPath { get; private set; }
    }
}