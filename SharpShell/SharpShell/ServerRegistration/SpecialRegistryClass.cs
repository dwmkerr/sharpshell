using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SharpShell.Attributes;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// Defines and represents the special registry classes.
    /// </summary>
    public enum SpecialRegistryClass
    {
        /// <summary>
        /// Not a special class.
        /// </summary>
        [Description("Not a special class")]
        None,

        /// <summary>
        /// All files class.
        /// </summary>
        [Description("All files")]
        [SpecialClassKey(@"*")]
        AllFiles,

        /// <summary>
        /// All file system objects class.
        /// </summary>
        [Description("All files and file folders")]
        [SpecialClassKey(@"AllFileSystemObjects")]
        AllFileSystemObjects,

        /// <summary>
        /// The folder class.
        /// </summary>
        [Description("All folders")]
        [SpecialClassKey(@"Folder")]
        Folder,

        /// <summary>
        /// The directory class.
        /// </summary>
        [Description("File folders")]
        [SpecialClassKey(@"Directory")]
        Directory,

        /// <summary>
        /// The directory background class.
        /// </summary>
        [Description("File folder background")]
        [SpecialClassKey(@"Directory\Background")]
        DirectoryBackground,

        /// <summary>
        /// The drive class.
        /// </summary>
        [Description("All drives in MyComputer")]
        [SpecialClassKey(@"Drive")]
        Drive,

        /// <summary>
        /// The network class.
        /// </summary>
        [Description("Entire network (under My Network Places)")]
        [SpecialClassKey(@"Network")]
        Network,

        //Network\Type\#	All objects of type # (see below)	Shortcut menu, Property Sheet, Verbs	4.71

        /// <summary>
        /// The network shares class.
        /// </summary>
        [Description("All network shares")]
        [SpecialClassKey(@"NetShare")]
        NetworkShares,

        /// <summary>
        /// The network servers class.
        /// </summary>
        [Description("All network servers")]
        [SpecialClassKey(@"NetServer")]
        NetworkServers,

        // network_provider_name	All objects provided by network provider "network_provider_name"	Shortcut menu, Property Sheet, Verbs	All

        /// <summary>
        /// The printers class.
        /// </summary>
        [Description("All printers")]
        [SpecialClassKey(@"Printers")]
        Printers,

        /// <summary>
        /// The audio cd class.
        /// </summary>
        [Description("Audio CD in CD drive")]
        [SpecialClassKey(@"AudioCD")]
        AudioCD,

        /// <summary>
        /// The DVD class.
        /// </summary>
        [Description("DVD drive")]
        [SpecialClassKey(@"DVD")]
        DVD
    }
}
