# Installation

Due to the fact that Cake Modules are extending and altering the internals of Cake, the module assembly needs to be loaded prior to the main Cake execution. As a result, the only place that this can really happen is during the bootstrapping phase. If you use the latest version of the default bootstrapper this process is made very easy. All you need to do is the following.

1. Add a `Modules` folder into your `tools` folder
1. Add a `packages.config` file into the newly created `Modules` folder
1. Add the name and version of the module that you want to use. This should look something like:
	```xml
	<?xml version="1.0" encoding="utf-8"?>
	<packages>
		<package id="Cake.Npm.Module" version="0.1.0" />
	</packages>
	```
1. Run the build as normal. During Cake's execution, it will recognise the module assembly which has been restored into the `tools/Modules` folder, and load it.

> [!NOTE]
> Similar to the recommendation regarding only checking in your `packages.config` and not the entire contents of the Cake `tools/` folder, the same recommendation is applied here. Only check in the `packages.config` file in the `Modules` folder, and not the entire contents.

> [!NOTE]
> These steps assume you are using the `nuget.org` feed. Custom feeds require additional steps.