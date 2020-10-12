using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tooling;

namespace Cake.Npm.Module
{
    /// <summary>
    /// <see cref="IPackageInstaller"/> implementation for the npm package manager.
    /// </summary>
    public class NpmPackageInstaller : IPackageInstaller
    {
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly ICakeLog _log;
        private readonly INpmContentResolver _contentResolver;
        private readonly ICakeConfiguration _config;
        private readonly IToolLocator _toolLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpmPackageInstaller"/> type.
        /// </summary>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="log">The log.</param>
        /// <param name="contentResolver">The content resolver.</param>
        /// <param name="config">The configuration.</param>
        public NpmPackageInstaller(ICakeEnvironment environment, IProcessRunner processRunner, ICakeLog log, INpmContentResolver contentResolver, ICakeConfiguration config, IToolLocator toolLocator)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (processRunner == null)
            {
                throw new ArgumentNullException(nameof(processRunner));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            if (contentResolver == null)
            {
                throw new ArgumentNullException(nameof(contentResolver));
            }

            _environment = environment;
            _processRunner = processRunner;
            _log = log;
            _contentResolver = contentResolver;
            _config = config;
            _toolLocator = toolLocator;
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <param name="package">The package resource.</param>
        /// <param name="type">The package type.</param>
        /// <returns>
        ///   <c>true</c> if this installer can install the
        ///   specified resource; otherwise <c>false</c>.
        /// </returns>
        public bool CanInstall(PackageReference package, PackageType type)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            return package.Scheme.Equals(Constants.PackageScheme, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Installs the specified resource.
        /// </summary>
        /// <param name="package">The package resource.</param>
        /// <param name="type">The package type.</param>
        /// <param name="path">The location where to install the resource.</param>
        /// <returns>The installed files.</returns>
        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            // find npm
            _log.Debug("looking for npm.cmd"); 
            var npmTool = _toolLocator.Resolve("npm.cmd");

            if (npmTool == null)
            {
                _log.Debug("looking for npm");
                npmTool = _toolLocator.Resolve("npm");
            }

            if (npmTool == null)
            {
                throw new FileNotFoundException("npm could not be found.");
            }

            _log.Debug("Found npm at {0}", npmTool);

            // Install the package.
            _log.Debug("Installing package {0} with npm...", package.Package);
            var process = _processRunner.Start(
                npmTool.FullPath,
                new ProcessSettings { Arguments = GetArguments(package, _config), RedirectStandardOutput = true, Silent = _log.Verbosity < Verbosity.Diagnostic });

            process.WaitForExit();

            var exitCode = process.GetExitCode();
            if (exitCode != 0)
            {
                _log.Warning("npm exited with {0}", exitCode);
                var output = string.Join(Environment.NewLine, process.GetStandardOutput());
                _log.Verbose(Verbosity.Diagnostic, "Output:\r\n{0}", output);
            }

            var result = _contentResolver.GetFiles(package, type, package.GetSwitch("global"));
            if (result.Count != 0)
            {
                return result;
            }

            _log.Warning("Could not determine installed package files! Installation may not be complete.");
            // TODO: maybe some warnings here
            return result;
        }

        private ProcessArgumentBuilder GetArguments(
            PackageReference definition,
            ICakeConfiguration config)
        {
            var arguments = new ProcessArgumentBuilder();

            arguments.Append("install");

            if (_log.Verbosity == Verbosity.Verbose || _log.Verbosity == Verbosity.Diagnostic)
            {
                arguments.Append("--verbose");
            }

            if (definition.Address != null) {
                arguments.Append($"--registry \"{definition.Address}\"");
            } else {
                var npmSource = config.GetValue(Constants.ConfigKey);
                if (!string.IsNullOrWhiteSpace(npmSource))
                {
                    arguments.Append($"--registry \"{npmSource}\"");
                }
            }

            if (definition.GetSwitch("force"))
            {
                arguments.Append("--force");
            }

            if (definition.GetSwitch("global"))
            {
                arguments.Append("--global");
            }

            if (definition.GetSwitch("ignore-scripts"))
            {
                arguments.Append("--ignore-scripts");
            }

            if (definition.GetSwitch("no-optional"))
            {
                arguments.Append("--no-optional");
            }

            if (definition.GetSwitch("save"))
            {
                GetSaveArguments(arguments, definition);
            }

            var packageString = new StringBuilder();

            if (definition.HasValue("source", out string source))
            {
                packageString.Append(source.Trim(':') + ":");
            }
            if (definition.HasValue("scope", out string scope))
            {
                packageString.Append("@" + scope.Trim('@') + "/");
            }

            packageString.Append(definition.Package);

            if (definition.HasValue("version", out string version))
            {
                packageString.Append(("@" + version.Trim(':', '=', '@')).Quote());
            }

            arguments.Append(packageString.ToString());
            return arguments;
        }

        private void GetSaveArguments(ProcessArgumentBuilder arguments, PackageReference definition)
        {
                var values = definition.Parameters["save"].ToList();
                foreach (var value in values)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        arguments.Append("--save");
                    }
                    else
                    {
                        switch (value)
                        {
                            case "dev":
                                arguments.Append("--save-dev");
                                break;
                            case "optional":
                                arguments.Append("--save-optional");
                                break;
                            case "exact":
                                arguments.Append("--save-exact");
                                break;
                            case "bundle":
                                arguments.Append("--save-bundle");
                                break;
                            default:
                                break;
                        }
                    }

                }
        }
    }
}
