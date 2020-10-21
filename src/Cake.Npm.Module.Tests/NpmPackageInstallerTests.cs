using System;
using System.IO;

using Cake.Core.IO;
using Cake.Core.Packaging;
using JetBrains.Annotations;
using NSubstitute;
using Xunit;

namespace Cake.Npm.Module.Tests
{
    /// <summary>
    /// <see cref="NpmPackageInstaller" /> unit tests
    /// </summary>
    [UsedImplicitly]
    public sealed class NpmPackageInstallerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {ProcessRunner = null};

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("processRunner", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Content_Resolver_Is_Null()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {ContentResolver = null};

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("contentResolver", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {Log = null};

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("log", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheCanInstallMethod
        {
            private string NPM_CONFIGKEY = "NPM_Source";

            [Fact]
            public void Should_Throw_If_URI_Is_Null()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {Package = null};

                // When
                var result = Record.Exception(() => fixture.CanInstall());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("package", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Be_Able_To_Install_If_Scheme_Is_Correct()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {Package = new PackageReference("npm:?package=yo")};

                // When
                var result = fixture.CanInstall();

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Not_Be_Able_To_Install_If_Scheme_Is_Incorrect()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {Package = new PackageReference("dnf:?package=glxgears")};

                // When
                var result = fixture.CanInstall();

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Ignore_Custom_Source_If_AbsoluteUri_Is_Used()
            {
                var fixture = new NpmPackageInstallerFixture
                {
                    Package = new PackageReference("npm:http://absolute/?package=windirstat")
                };

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.Null(result);
                fixture.Config.DidNotReceive().GetValue(NPM_CONFIGKEY);
            }

            [Fact]
            public void Should_Use_Custom_Source_If_RelativeUri_Is_Used()
            {
                var fixture = new NpmPackageInstallerFixture {Package = new PackageReference("npm:?package=yo")};

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.Null(result);
                fixture.Config.Received().GetValue(NPM_CONFIGKEY);
            }
        }

        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Uri_Is_Null()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {Package = null};

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("package", ((ArgumentNullException)result).ParamName);
            }


            ///<summary>
            /// This test is the inverse of the normal one since the install path is ignored.
            ///</summary>
            ///<remarks>
            ///An install path makes no sense in a npm context
            ///</remarks>
            [Fact]
            public void Should_Not_Throw_If_Install_Path_Is_Null()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture {InstallPath = null};

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheToolLocator
        {
            [Fact]
            public void Should_Throw_If_Npm_Can_Not_Be_Found()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture
                {
                    ToolLocatorSetup = l => l.Resolve(string.Empty).ReturnsForAnyArgs((FilePath) null)
                };

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("npm could not be found.", ((FileNotFoundException)result).Message);
            }

            [Fact]
            public void Should_Not_Check_For_Npm_If_NpmCmd_Can_Be_Found()
            {
                // Given
                var fixture = new NpmPackageInstallerFixture
                {
                    ToolLocatorSetup = l => l.Resolve("npm.cmd").Returns(new FilePath("npm.cmd"))
                };

                // When
                fixture.Install();

                // Then
                fixture.ToolLocator.Received().Resolve("npm.cmd");
                fixture.ToolLocator.Received(0).Resolve("npm");
            }
        }
    }
}
