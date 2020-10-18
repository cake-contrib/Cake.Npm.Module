# Installation

Due to the fact that Cake Modules are extending and altering the internals of Cake, the module assembly needs to be loaded prior to the main Cake execution. As a result, the only place that this can really happen is during the bootstrapping phase. If you use the latest version of the default bootstrapper this process is made very easy. All you need to do is the following.

1. Add the following line in you're cake.recipe:

   ```
   #module nuget:?package=Cake.Npm.Module&version=<version>
   ```
1. Run the build with argument `--bootstrap` (i.e. `./build.ps1 --bootrap`).

   This will restorethe module assembly into the `tools/Modules` folder
1. Run the build as normal. During Cake's execution, it will recognise the module assembly which has been restored into the `tools/Modules` folder, and load it.


> [!NOTE]
> These steps assume you are using the `nuget.org` feed. Custom feeds may require additional steps.