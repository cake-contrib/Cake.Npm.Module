#module nuget://?package=Cake.Npm.Module&prerelease
#tool npm://?package=yo&caketools

Task("Default")
.Does(() => {
   var yo = Context.Tools.Resolve(IsRunningOnWindows() ? "yo.cmd" : "yo");
   if(yo == null) {
      Error("Something is very wrong.");
      throw new Exception("yo not found!");
   }
   Information("yo installed at: " + yo.FullPath);
   IEnumerable<string> version;
   StartProcess(
      yo,
      new ProcessSettings {
            Arguments = "--version",
            RedirectStandardOutput = true
      },
      out version);
   Information("yo version: " + version.First());
});

RunTarget("Default");