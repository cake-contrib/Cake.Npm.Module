using System;
using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Packaging;
using Cake.Npm.Module;

[assembly: CakeModule(typeof(NpmModule))]

namespace Cake.Npm.Module
{
    public class NpmModule : ICakeModule
    {
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