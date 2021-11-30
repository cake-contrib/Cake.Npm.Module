namespace Cake.Npm.Module
{
    /// <summary>
    /// Locations to install into.
    /// </summary>
    public enum ModulesInstallationLocation
    {
        /// <summary>
        /// Install into current WorkDir. This is also the default.
        /// </summary>
        Workdir,

        /// <summary>
        /// Install into tools folder.
        /// </summary>
        Tools,

        /// <summary>
        /// Install globally for the machine.
        /// </summary>
        Global,
    }
}
