using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SharpShell.Pidl
{
    /// <summary>
    /// Reprents a Shell ID list. Used with the <see cref="PidlManager" />.
    /// </summary>
    public sealed class IdList
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="IdList"/> class from being created.
        /// </summary>
        /// <param name="ids">The ids.</param>
        private IdList(List<ShellId> ids)
        {
            this.ids = ids;
        }

        /// <summary>
        /// Creates an IdList.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        internal static IdList Create(List<ShellId> ids)
        {
            return new IdList(ids);
        }

        /// <summary>
        /// Converts an idlist to a parsing string.
        /// </summary>
        /// <returns>The id list in parsing string format.</returns>
        public string ToParsingString()
        {
            var sb = new StringBuilder(ids.Sum(id => id.Length*2 + 4));
            foreach (var id in ids)
            {
                sb.AppendFormat("{0:x4}", (short)id.Length);
                foreach(var idi in id.RawId)
                    sb.AppendFormat("{0:x2}", idi);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates an idlist from parsing string format.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The idlist represented by the string.</returns>
        public static IdList FromParsingString(string str)
        {
            //  Create the id storage.
            var ids = new List<ShellId>();

            //  Repeatedly read a short length then the data.
            int index = 0;
            while (index < str.Length)
            {
                var length = Convert.ToInt16(str.Substring(index, 4), 16);
                var id = new byte[length];
                index += 4;
                for (var i = 0; i < length; i++, index += 2)
                    id[i] = Convert.ToByte(str.Substring(index, 2), 16);
                ids.Add(ShellId.FromData(id));
            }

            //  Return the list.
            return new IdList(ids);
        }

        /// <summary>
        /// Determines whether two idlists are equal.
        /// </summary>
        /// <param name="idList">The ID list to compare against.</param>
        /// <returns>True if the id lists are equal, false otherwise.</returns>
        public bool Matches(IdList idList)
        {
            //  We must have a valid set to match against.
            if (idList == null || idList.ids == null || idList.ids.Count != ids.Count)
                return false;
            
            //  If there is any id that doesn't match, we return false.
            return !ids.Where((t, i) => !t.Equals(idList.ids[i])).Any();
        }

        /// <summary>
        /// The ids.
        /// </summary>
        private readonly List<ShellId> ids;

        /// <summary>
        /// Gets the ids.
        /// </summary>
        public ReadOnlyCollection<ShellId> Ids { get { return ids.AsReadOnly(); } }
    }
}