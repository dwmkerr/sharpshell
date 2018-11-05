namespace ResourcesPropertySheet.Loader
{
    class Win32Resources
    {
        public Win32ResourceType ResourceType { get; }
        public Win32Resource[] ResourceNames { get; }

        public Win32Resources(Win32ResourceType resourceType, Win32Resource[] resourceNames)
        {
            ResourceType = resourceType;
            ResourceNames = resourceNames;
        }
    }
}