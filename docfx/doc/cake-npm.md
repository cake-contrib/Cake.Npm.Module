# `Cake.Npm` vs `Cake.Npm.Module`

Cake already has support for `npm` using the [`Cake.Npm`](https://nuget.org/packages/Cake.Npm) addin from community member *[philo](https://github.com/philo)*. The addin also supports installing packages using the `Npm.Install()` alias, in a similar fashion to this module.

Using this module doesn't replace the addin, and you can even use both in the same script. So what's the difference?

This module integrates itself into the internals of Cake meaning that tools installed using the `#tool npm:` directive are installed before the script itself is run. This is especially useful when your script uses tools installed from npm, like `Cake.Yeoman`, `Cake.Tfx`, or `Cake.AutoRest`.

Conversely, the addin is more flexible, so you can install tools only when required and have more control over when in the script you install your required npm packages. When using the module, every `#tool` is installed every time you run, regardless of whether it's needed. The addin also supports other npm commands like `pack` and `run`, while the module only supports `install`.

Finally, remember that while Cake will install addins for you (when using the `#addin` directive), modules need to be installed into the modules directory **before** the script runs. The [latest PowerShell bootstrapper](https://raw.githubusercontent.com/cake-build/resources/develop/build.ps1) does this for you, but when using your own script, or running in another environment you may need to install the module yourself before `cake.exe` starts.


In summary:

**Addin:**

- Automatically installed by Cake
- Can be conditionally run
- Supports installing multiple packages
- Supports `npm pack` and `npm run`

**Module:**

- Packages are installed before the script is run
- Needs to be installed by bootstrapper (or equivalent)
- Integrated into Cake directly (including tool resolution)
- Allows declaring `#tool` directives alongside other packages (i.e. NuGet, DNF, Chocolatey)

Deciding whether to install your desired packages using the module or the addin will depend on your specific build requirements, your environment and how you intend to use your packages. Remember you can always get help in [the Gitter room](https://gitter.im/cake-build/cake) if you have any questions about the addin or module.