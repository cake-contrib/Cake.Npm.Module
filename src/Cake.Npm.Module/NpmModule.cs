using System;
using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Packaging;
using Cake.Npm.Module;

[assembly: CakeModule(typeof(NpmModule))]

namespace Cake.Npm.Module
{
    /// <summary>
    /// Module type to add support for the npm package manager.
    /// </summary>
    public class NpmModule : ICakeModule
    {
        /// <inheritdoc />
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null) {
                throw new ArgumentNullException(nameof(registrar));
            }
            registrar.RegisterType<NpmContentResolver>().As<INpmContentResolver>();
            registrar.RegisterType<NpmPackageInstaller>().As<IPackageInstaller>();
        }
    }
}
