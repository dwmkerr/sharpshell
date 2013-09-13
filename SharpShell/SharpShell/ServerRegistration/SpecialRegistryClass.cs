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
        [Description("Not a special class")]
        None,

        [Description("All files")]
        [SpecialClassKey(@"*")]
        AllFiles,
        
        [Description("All files and file folders")]
        [SpecialClassKey(@"AllFileSystemObjects")]
        AllFileSystemObjects,
        
        [Description("All folders")]
        [SpecialClassKey(@"Folder")]
        Folder,

        [Description("File folders")]
        [SpecialClassKey(@"Directory")]
        Directory,
        
        [Description("File folder background")]
        [SpecialClassKey(@"Directory\Background")]
        DirectoryBackground,
        
        [Description("All drives in MyComputer")]
        [SpecialClassKey(@"Drive")]
        Drive,
        
        [Description("Entire network (under My Network Places)")]
        [SpecialClassKey(@"Network")]
        Network,

        //Network\Type\#	All objects of type # (see below)	Shortcut menu, Property Sheet, Verbs	4.71
        
        [Description("All network shares")]
        [SpecialClassKey(@"NetShare")]
        NetworkShares,
        
        [Description("All network servers")]
        [SpecialClassKey(@"NetServer")]
        NetworkServers,

        // network_provider_name	All objects provided by network provider "network_provider_name"	Shortcut menu, Property Sheet, Verbs	All
        
        [Description("All printers")]
        [SpecialClassKey(@"Printers")]
        Printers,
        
        [Description("Audio CD in CD drive")]
        [SpecialClassKey(@"AudioCD")]
        AudioCD,
        
        [Description("DVD drive")]
        [SpecialClassKey(@"DVD")]
        DVD
    }
}
