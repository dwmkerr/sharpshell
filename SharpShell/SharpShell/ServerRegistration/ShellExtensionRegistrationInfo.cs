using System;
using System.Collections.Generic;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    ///     Represents registration info for a server.
    /// </summary>
    public class ShellExtensionRegistrationInfo
    {
        public ShellExtensionRegistrationInfo(
            Guid serverClassId,
            bool isApproved,
            IEnumerable<ShellExtensionRegisteredAssociationInfo> associations)
        {
            ServerClassId = serverClassId;
            IsApproved = isApproved;
            Associations = associations;
        }

        /// <summary>
        ///     Gets the server registered associations
        /// </summary>
        public IEnumerable<ShellExtensionRegisteredAssociationInfo> Associations { get; }


        /// <summary>
        ///     Gets a value indicating whether this extension is on the approved list.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is approved; otherwise, <c>false</c>.
        /// </value>
        public bool IsApproved { get; }

        /// <summary>
        ///     Gets the server class id.
        /// </summary>
        public Guid ServerClassId { get; }
    }
}