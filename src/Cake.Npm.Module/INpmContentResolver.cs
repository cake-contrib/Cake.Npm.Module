using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Npm.Module
{
    public interface INpmContentResolver
    {
         IReadOnlyCollection<IFile> GetFiles(PackageReference package, PackageType type, bool isGlobal);
    }
}