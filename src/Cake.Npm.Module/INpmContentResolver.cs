using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Npm.Module
{
    /// <summary>
    /// Represents a content resolver for packages installed with the npm package manager.
    /// </summary>
    public interface INpmContentResolver
    {
        /// <summary>
        /// Returns the files installed by the given package
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="type">The package type.</param>
        /// <param name="isGlobal">Whether the package is globally installed.</param>
        /// <returns></returns>
         IReadOnlyCollection<IFile> GetFiles(PackageReference package, PackageType type, bool isGlobal);
    }
}
