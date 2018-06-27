namespace Cake.Npm.Module
{
    /// <summary>
    /// Constant values used in the module.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The scheme name used in <c>#tools</c> directives supported by this module.
        /// </summary>
        internal const string PackageScheme = "npm";

        /// <summary>
        /// The key path for the configuration value used to control the registry used.
        /// </summary>
        internal const string ConfigKey = "NPM_Source";
    }
}
