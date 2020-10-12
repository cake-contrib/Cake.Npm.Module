using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Npm.Module;
using Cake.Core.Packaging;
using Cake.Core.Configuration;
using Cake.Testing;
using NSubstitute;
using System.Collections.Generic;
using Cake.Core.Tooling;
using System;

namespace Cake.Npm.Module.Tests
{
    /// <summary>
    /// Fixture used for testing <see cref="NpmPackageInstaller"/>
    /// </summary>
    internal sealed class NpmPackageInstallerFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IFileSystem FileSystem { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public INpmContentResolver ContentResolver { get; set; }
        public ICakeLog Log { get; set; }

        public PackageReference Package { get; set; }
        public PackageType PackageType { get; set; }
        public DirectoryPath InstallPath { get; set; }

        public ICakeConfiguration Config { get; private set; }

        public IToolLocator ToolLocator { get; }
        
        public Action<IToolLocator> ToolLocatorSetup { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="NpmPackageInstallerFixture"/> class.
        /// </summary>
        internal NpmPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ContentResolver = Substitute.For<INpmContentResolver>();
            Log = new FakeLog();
            Config = Substitute.For<ICakeConfiguration>();
            Package = new PackageReference("npm:?package=yo");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./fake-path");
            ToolLocator = Substitute.For<IToolLocator>();
        }

        /// <summary>
        /// Create the installer.
        /// </summary>
        /// <returns>The Npm package installer.</returns>
        internal NpmPackageInstaller CreateInstaller()
        {
            var setup = ToolLocatorSetup;
            if(setup == null)
            {
                setup = l =>
                {
                    l.Resolve("npm.cmd").Returns((FilePath)null);
                    l.Resolve("npm").Returns(new FilePath("npm"));
                };
            }

            setup(ToolLocator);

            return new NpmPackageInstaller(Environment, ProcessRunner, Log, ContentResolver, Config, ToolLocator);
        }

        /// <summary>
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <returns>The installed files.</returns>
        internal IReadOnlyCollection<IFile> Install()
        {
            var installer = CreateInstaller();
            return installer.Install(Package, PackageType, InstallPath);
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <returns><c>true</c> if this installer can install the specified resource; otherwise <c>false</c>.</returns>
        internal bool CanInstall()
        {
            var installer = CreateInstaller();
            return installer.CanInstall(Package, PackageType);
        }
    }
}
