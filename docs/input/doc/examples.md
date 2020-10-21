---
Order: 4
Title: Examples
---

# Examples

Installing a tool using the Npm Cake Module is as simple as:

```
#tool npm:?package=yo
```

If the tool in question comes from a different source, you can change that as follows:

```
#tool npm:http://registry.npmjs.org?package=yo
```

To install a specific version of a package:

```
#tool npm:?package=yo&version=1.8.5
```

or to tell npm to use a SemVer-compliant range

```
#tool npm:?package=sax&version=">=0.1.0 <0.2.0"
```

Full parameter information is covered in the [parameters documentation](parameters.md).