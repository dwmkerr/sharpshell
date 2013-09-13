using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShell.Helpers
{
    /// <summary>
    /// Helper for Win32.
    /// </summary>
    public static class Win32Helper
    {
        /// <summary>
        /// Gets the LoWord of an IntPtr.
        /// </summary>
        /// <param name="ptr">The int pointer.</param>
        /// <returns>The LoWord of an IntPtr.</returns>
        public static int LoWord(IntPtr ptr)
        {
            var @int = IntPtr.Size == 8 ? unchecked((int)ptr.ToInt64()) : ptr.ToInt32();
            return unchecked((short)(long)@int);
        }

        /// <summary>
        /// Gets the HiWord of an IntPtr.
        /// </summary>
        /// <param name="ptr">The int pointer.</param>
        /// <returns>The HiWord of an IntPtr.</returns>
        public static int HiWord(IntPtr ptr)
        {
            var @int = IntPtr.Size == 8 ? unchecked((int)ptr.ToInt64()) : ptr.ToInt32();
            return unchecked((short)((long)@int >> 16));
        }
    }
}
