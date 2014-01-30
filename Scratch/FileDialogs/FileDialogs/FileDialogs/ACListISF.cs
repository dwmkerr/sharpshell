using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace FileDialogs
{
    /// <summary>
    /// Custom implementation of the ACListISF.
    /// Adds a / to each folder.
    /// </summary>
    internal class ACListISF : NativeMethods.IEnumString, NativeMethods.IACList
    {
        #region Member Fields

        private int m_size;
        private int m_current;
        private string m_currentPath;
        private string m_currentWorkingDirectory;
        private bool m_excludeFiles;

        private List<string> m_strings;

        #endregion

        #region IEnumString Members

        int NativeMethods.IEnumString.Next(int celt, string[] rgelt, IntPtr pceltFetched)
        {
            int index = 0;
            while ((m_current < m_size) && (celt > 0))
            {
                rgelt[index] = m_strings[m_current];
                m_current++;
                index++;
                celt--;
            }

            if (pceltFetched != IntPtr.Zero)
                Marshal.WriteInt32(pceltFetched, index);

            if (celt != 0)
                return NativeMethods.S_FALSE;

            return NativeMethods.S_OK;
        }

        int NativeMethods.IEnumString.Skip(int celt)
        {
            m_current += celt;
            if (m_current >= m_size)
                return NativeMethods.S_FALSE;

            return NativeMethods.S_OK;
        }

        void NativeMethods.IEnumString.Reset()
        {
            m_current = 0;

            if (m_strings == null)
                m_strings = new List<string>();

            m_strings.Clear();
            m_size = 0;

            if (!string.IsNullOrEmpty(m_currentWorkingDirectory) &&
                m_currentWorkingDirectory.IndexOfAny(Path.GetInvalidPathChars()) < 0 &&
                string.IsNullOrEmpty(m_currentPath))
            {
                string[] dirs = Directory.GetDirectories(m_currentWorkingDirectory);
                string[] files = Directory.GetFiles(m_currentWorkingDirectory);

                m_size = dirs.Length + files.Length;
                for (int i = 0; i < m_size; i++)
                {
                    if (i < dirs.Length)
                        m_strings.Add(GetRelativePath(dirs[i]) + @"\");
                    else
                        m_strings.Add(GetRelativePath(files[i - dirs.Length]));
                }
            }

            m_currentPath = string.Empty;
        }

        private string GetRelativePath(string path)
        {
            if (!m_currentWorkingDirectory.EndsWith(@"\"))
                return path.Replace(m_currentWorkingDirectory + @"\", "");

            return path.Replace(m_currentWorkingDirectory, "");
        }

        void NativeMethods.IEnumString.Clone(out NativeMethods.IEnumString ppenum)
        {
            ACListISF acl = new ACListISF();
            acl.m_current = m_current;
            acl.m_currentPath = m_currentPath;
            acl.m_currentWorkingDirectory = m_currentWorkingDirectory;
            acl.m_excludeFiles = m_excludeFiles;

            ppenum = acl;
        }

        #endregion

        #region IACList Members

        int NativeMethods.IACList.Expand(string pszExpand)
        {
            m_currentPath = pszExpand;

            bool pathIsRooted = Path.IsPathRooted(m_currentPath);
            if (!pathIsRooted)
                m_currentPath = Path.Combine(m_currentWorkingDirectory, m_currentPath);

            // TODO: Add support for ..\ in GetRealPath
            //if (m_currentPath.Contains(".."))
            //    m_currentPath = Path.GetFullPath(m_currentPath);

            m_strings.Clear();


            string[] dirs = Directory.GetDirectories(m_currentPath);
            string[] files = Directory.GetFiles(m_currentPath);

            m_size = dirs.Length + files.Length;
            for (int i = 0; i < m_size; i++)
            {
                if (i < dirs.Length)
                {
                    string path = dirs[i];
                    if (!pathIsRooted) path = GetRelativePath(path);
                    m_strings.Add(path + @"\");
                }
                else
                {
                    string path = files[i - dirs.Length];
                    if (!pathIsRooted) path = GetRelativePath(path);
                    m_strings.Add(path);
                }

                if (m_excludeFiles && i == dirs.Length - 1)
                    break;
            }

            return NativeMethods.S_OK;
        }

        #endregion

        #region Properties

        public string CurrentWorkingDirectory
        {
            get { return m_currentWorkingDirectory; }
            set { m_currentWorkingDirectory = value; }
        }

        public bool ExcludeFiles
        {
            get { return m_excludeFiles; }
            set { m_excludeFiles = value; }
        }

        #endregion
    }
}
