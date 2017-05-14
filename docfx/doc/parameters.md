The folllowing URI parameters are supported by the Cake.Npm.Module.

# Registry

By default, npm will attempt to install packages using the `registry.npmjs.org` registry, unless you have configured it otherwise. To use a specific repository, provide a source in the URI.

> [!WARNING]
> Using this parameter invokes the underlying `--registry` option, meaning that *only* this registry will be used for the installation.

### Example

```
#tool npm:http://registry.npmjs.org?package=yo
```

# Package

This is the name of the npm package that you would like to install.  This should match the package name exactly.

### Example

```
#tool npm:?package=yo
```

> [!NOTE]
> While you can specify the scope and tag here, it's recommended to use the `scope` and `version` parameters (see below)

# Version

The specific version of the package that is being installed.  If not provided, npm will install the latest package version that is available.

### Example

```
#tool npm:?package=yo&version=1.8.5
```

# Force

This is equivalent to the `--force` switch and will force npm to fetch remote resources even if a local copy exists on disk.

### Example

```
#tool npm:?package=yo&version=1.8.5&force
```

# Global

This corresponds to the `--global` option, and tells npm to install this package globally, rather than to the project-local modules.

### Example

```
#tool npm:?package=yo&global
```

# Save

This corresponds to the `--save*` options and controls npm's behaviour for saving dependencies. Specifying `save` with no arguments is equivalent to `--save`, while providing a value (or multiple parameters) provides fine-grained control.

### Examples

```csharp
#tool npm:?package=yo&save //equivalent to --save
#tool npm:?package=yo&save=dev //equivalent to --save-dev
#tool npm:?package=yo&save=optional //equivalent to --save-optional
#tool npm:?package=yo&save=dev&save=exact //equivalent to --save-dev --save-exact
#tool npm:?package=yo&save=optional&save=bundle //equivalent to --save-optional --save-bundle
```

# Source

This allows using npm's support for installing directly from some repositories. Supported sources include `github`, `gist`, `bitbucket`, and `gitlab`

### Example

```csharp
#tool npm:?package=yeoman/yo&source=github
// equivalent to npm install github:yeoman/yo
```

# Scope

This allows installing scoped packages.

> [!NOTE]
> While providing the scope with the package name *should* work, it's recommended to use this option.

### Example

```csharp
#tool npm:?package=app-scripts&scope=ionic
// equivalent to npm install @ionic/app-scripts
```

# Others

The module also allows a couple of other options, corresponding to their CLI equivalents:

```csharp
#tool npm:?package=yo&ignore-scripts // npm install yo --ignore-scripts
#tool npm:?package=yo&no-optional // npm install yo --no-optional
```

If there are other CLI options you need the module to support, raise an issue [on GitHub](https://github.com/cake-contrib/Cake.Npm.Module/).