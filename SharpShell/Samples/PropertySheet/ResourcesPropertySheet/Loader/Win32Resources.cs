namespace ResourcesPropertySheet.Loader
{
    class Win32Resources
    {
        public Win32ResourceType ResourceType { get; }
        public Win32Resource[] Resouces { get; }

        public Win32Resources(Win32ResourceType resourceType, Win32Resource[] resouces)
        {
            ResourceType = resourceType;
            Resouces = resouces;
        }
    }
}