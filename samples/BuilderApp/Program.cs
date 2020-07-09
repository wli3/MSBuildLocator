// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Locator;

namespace BuilderApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MSBuildLocator.RegisterMSBuildPath(@"C:\Program Files\dotnet\sdk\5.0.100-preview.4.20258.7\");
            var result = new Builder().Build(@"C:\work\temp\cannnot_add_package2\TestAppSimple.csproj");
        }
    }

    public class Builder
    {
        public bool Build(string projectFile)
        {
            var assembly = typeof(Project).Assembly;
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.WriteLine();
            Console.WriteLine($"BuildApp running using MSBuild version {fvi.FileVersion}");
            Console.WriteLine(Path.GetDirectoryName(assembly.Location));
            Console.WriteLine();

            var pre = ProjectRootElement.Open(projectFile);
            var project = new Project(pre);
            return project.Build(new Logger());
        }

        private class Logger : ILogger
        {
            public void Initialize(IEventSource eventSource)
            {
                eventSource.AnyEventRaised += (_, args) => { Console.WriteLine(args.Message); };
            }

            public void Shutdown()
            {
            }

            public LoggerVerbosity Verbosity { get; set; }
            public string Parameters { get; set; }
        }
    }
}
