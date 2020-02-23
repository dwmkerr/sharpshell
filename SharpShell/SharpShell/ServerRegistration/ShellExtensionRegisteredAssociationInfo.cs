using System;
using System.Collections.Generic;
using System.Linq;
using SharpShell.Attributes;
using SharpShell.Extensions;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    ///     Represents association info
    /// </summary>
    public class ShellExtensionRegisteredAssociationInfo
    {
        private static readonly Dictionary<string, SpecialRegistryClass> SpecialRegistryClassNameMap =
            Enum.GetValues(typeof(SpecialRegistryClass))
                .OfType<SpecialRegistryClass>()
                .Select(enumValue =>
                    new
                    {
                        SpecialClass = enumValue,
                        ClassName = enumValue.GetAttribute<SpecialClassKeyAttribute>()?.SpecialClassKey
                    }
                )
                .Where(pair => pair.ClassName != null)
                .ToDictionary(pair => pair.ClassName, pair => pair.SpecialClass);


        internal ShellExtensionRegisteredAssociationInfo(
            ShellExtensionType extensionType,
            Guid serverClassId,
            string associationClassName)
        {
            ShellExtensionType = extensionType;
            ServerClassId = serverClassId;
            AssociationClassName = associationClassName;
            RegistrationName = $"{associationClassName}'s {ShellExtensionType} ({serverClassId:B})";
        }

        internal ShellExtensionRegisteredAssociationInfo(
            ShellExtensionType extensionType,
            Guid serverClassId,
            string associationClassName,
            string registrationName) : this(extensionType, serverClassId, associationClassName)
        {
            RegistrationName = registrationName ?? RegistrationName;
        }

        /// <summary>
        ///     Gets the target class name
        /// </summary>
        /// <value>
        ///     The target class name.
        /// </value>

        public string AssociationClassName { get; }

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string RegistrationName { get; }

        /// <summary>
        ///     Gets the server class id.
        /// </summary>
        public Guid ServerClassId { get; }

        /// <summary>
        ///     Gets the type of the shell extension.
        /// </summary>
        /// <value>
        ///     The type of the shell extension.
        /// </value>
        public ShellExtensionType ShellExtensionType { get; }

        /// <summary>
        ///     Gets the target special class value
        /// </summary>
        /// <value>
        ///     The target special class value
        /// </value>
        public SpecialRegistryClass SpecialRegistryClass
        {
            get => !string.IsNullOrEmpty(AssociationClassName) &&
                   SpecialRegistryClassNameMap.ContainsKey(AssociationClassName)
                ? SpecialRegistryClassNameMap[AssociationClassName]
                : SpecialRegistryClass.None;
        }
    }
}