# Configuration

As mentioned in the parameters section, it is possible to specify the specific source that is used when installing a tool. If you always use the same alternative source, doing this for each source can become unwieldy. Instead, it is possible to configure a default source which is used for all tool installations made by the Cake.Npm.Module.

This makes use of the configuration options within Cake which are documented [here](http://cakebuild.net/docs/fundamentals/configuration).

## Source repository URL

You can provide a non-default repository URL that all installations will use as the registry. Note that this uses npm's `--registry` switch and `npm` will *only* use the specified registry, replacing `npmjs.com`

To provide a repository URL, either set the environment variable `NPM_SOURCE`, provide the `--npm_source` argument to `cake.exe` or add a section to your `cake.config` as per below:

```ini
[NPM]
Source=http://mynodepackages.com/registry/
```

With this configuration in place, all calls to the `npm` installer will only use the specified repo.