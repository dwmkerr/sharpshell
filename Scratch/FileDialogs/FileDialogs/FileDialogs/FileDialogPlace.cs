using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using FileDialogs.Design;

namespace FileDialogs
{
    /// <summary>
    /// Represents a place in a FileDialog place bar.
    /// </summary>
    [ToolboxItem(false)]
    public abstract class FileDialogPlaceBase
    {
        #region Member Fields

        private IntPtr m_pidl;
        private string m_originalText;
        private string m_customText;

        #endregion

        #region Methods

        protected abstract IntPtr ConvertToPidl();

        protected void Initialize()
        {
            m_pidl = ConvertToPidl();
            NativeMethods.SHFILEINFO sfi = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(m_pidl, 0, ref sfi, NativeMethods.cbFileInfo,
                                                NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_DISPLAYNAME);

            m_originalText = sfi.szDisplayName;
            m_customText = string.Empty;
        }

        private bool ShouldSerializeText()
        {
            return !string.IsNullOrEmpty(m_customText);
        }

        #endregion

        #region Properties

        internal IntPtr PIDL
        {
            get { return m_pidl; }
        }

        internal string OriginalText
        {
            get { return m_originalText; }
        }

        internal string CustomText
        {
            get { return m_customText; }
        }
        
        /// <summary>
        /// Gets or sets the text of the place.
        /// </summary>
        [Category("Appearance")]
        [Description("The text of the place.")]
        public string Text
        {
            get { return string.IsNullOrEmpty(m_customText) ? m_originalText : m_customText; }
            set { m_customText = value; }
        }

        #endregion
    }

    public class FileDialogPlace : FileDialogPlaceBase
    {
        #region Member Fields

        private SpecialFolder m_specialFolder = SpecialFolder.None;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the FileDialogPlace class.
        /// </summary>
        public FileDialogPlace()
        {   
        }

        /// <summary>
        /// Initializes a new instance of the FileDialogPlace class identified by the specified system special folder.
        /// </summary>
        /// <param name="specialFolder">The system special folder identifying the place.</param>
        public FileDialogPlace(SpecialFolder specialFolder)
        {
            m_specialFolder = specialFolder;
            Initialize();
        }

        #endregion

        #region Methods

        protected override IntPtr ConvertToPidl()
        {
            IntPtr pidl;
            NativeMethods.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, (int)m_specialFolder, out pidl);

            return pidl;
        }

        private bool ShouldSerializeSpecialFolder()
        {
            return (SpecialFolder != SpecialFolder.None);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the system special folder identifying the place.
        /// </summary>
        [Browsable(false)]
        [Category("Data")]
        [Description("The system special folder identifying the place.")]
        public SpecialFolder SpecialFolder
        {
            get { return m_specialFolder; }
            set
            {
                m_specialFolder = value;
                Initialize();
            }
        }

        #endregion
    }

    public class CustomFileDialogPlace : FileDialogPlaceBase
    {
        #region Member Fields

        private string m_path;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the CustomFileDialogPlace class.
        /// </summary>
        public CustomFileDialogPlace()
        {   
        }

        /// <summary>
        /// Initializes a new instance of the CustomFileDialogPlace class with the specified folder path.
        /// </summary>
        /// <param name="path">The folder path to the place.</param>
        public CustomFileDialogPlace(string path)
        {
            m_path = path;
            Initialize();
        }

        #endregion

        #region Methods

        protected override IntPtr ConvertToPidl()
        {
            IntPtr desktopFolderPtr;
            NativeMethods.Shell32.SHGetDesktopFolder(out desktopFolderPtr);
            NativeMethods.IShellFolder desktopFolder = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(desktopFolderPtr);

            IntPtr pidl;
            desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, m_path, IntPtr.Zero,
                                           out pidl, 0);

            Marshal.ReleaseComObject(desktopFolder);
            Marshal.Release(desktopFolderPtr);

            return pidl;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the folder path to the place.
        /// </summary>
        [Category("Data")]
        [Description("The folder path to the place.")]
        [DefaultValue("")]
        [Editor(typeof(PathEditor), typeof(UITypeEditor))]
        public string Path
        {
            get { return m_path; }
            set
            {
                if (!Directory.Exists(value))
                    throw new DirectoryNotFoundException(value);

                m_path = value;
                Initialize();
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a collection of places for the FileDialog class.
    /// </summary>
    [ListBindable(false)]
    public class FileDialogPlacesCollection : Collection<FileDialogPlaceBase>
    {
        #region Methods

        /// <summary>
        /// Adds a place to to the FileDialogPlacesCollection collection.
        /// </summary>
        /// <param name="specialFolder">The system special folder identifying the place.</param>
        public void Add(SpecialFolder specialFolder)
        {
            Add(new FileDialogPlace(specialFolder));
        }

        /// <summary>
        /// Adds a place to to the FileDialogPlacesCollection collection.
        /// </summary>
        /// <param name="path">The folder path to the place.</param>
        public void Add(string path)
        {
            Add(new CustomFileDialogPlace(path));
        }

        #endregion
    }
}
