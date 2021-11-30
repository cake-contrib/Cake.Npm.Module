using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseModule<Cake.Npm.Module.NpmModule>()
            .InstallTool(new Uri("npm://?package=yo&caketools"))
            .UseWorkingDirectory("..")
            .Run(args);
    }
}

[TaskName("Default")]
public class DefaultTask : FrostingTask
{
    public override void Run(ICakeContext context)
    {
        var yo = context.Tools.Resolve(context.IsRunningOnWindows() ? "yo.cmd" : "yo");
        if(yo == null) {
            context.Error("Something is very wrong.");
            throw new Exception("yo not found!");
        }
        context.Information("yo installed at: " + yo.FullPath);
        IEnumerable<string> version;
        context.StartProcess(
            yo,
            new ProcessSettings {
                Arguments = "--version",
                RedirectStandardOutput = true
            },
            out version);
        context.Information("yo version: " + version.First());
    }
}
