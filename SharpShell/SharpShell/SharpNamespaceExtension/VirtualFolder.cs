namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// A VirtualFolder is a location that a Shell Namespace Extension can be hosted.
    /// </summary>
    public enum VirtualFolder
    {
        /// <summary>
        /// The control panel
        /// </summary>
        [RegistryKey(@"ControlPanel")]
        ControlPanel,

        /// <summary>
        /// The desktop
        /// </summary>
        [RegistryKey(@"Desktop")]
        Desktop,
        
        /// <summary>
        /// The entire network
        /// </summary>
        [RegistryKey(@"NetworkNeighborhood\EntireNetwork")]
        EntireNetwork,
        
        /// <summary>
        /// My computer
        /// </summary>
        [RegistryKey(@"MyComputer")]
        MyComputer,
        
        /// <summary>
        /// My network places
        /// </summary>
        [RegistryKey(@"NetworkNeighborhood")]
        MyNetworkPlaces,
        
        /// <summary>
        /// The remote computer
        /// </summary>
        [RegistryKey(@"RemoteComputer")]
        RemoteComputer,
        
        /// <summary>
        /// The users files
        /// </summary>
        [RegistryKey(@"UsersFiles")]
        UsersFiles
    }
}