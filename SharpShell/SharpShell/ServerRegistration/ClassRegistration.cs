namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// Helper class to determine the registration info of specific class.
    /// </summary>
    public class ClassRegistration
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRegistration"/> class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        public ClassRegistration(string className)
        {
            ClassName = className;
        }


        public SpecialRegistryClass SpecialRegistryClass { get; }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; set; }
    }
}
