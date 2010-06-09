﻿// Copyright 2007-2010 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Topshelf.Host
{
    using System;
    using System.IO;
    using Configuration;
    using Configuration.Dsl;
    using log4net.Config;

    public class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(".\\log4net.config"));
            RunConfiguration cfg = RunnerConfigurator.New(x =>
            {
                x.AfterStoppingTheHost(h => { Console.WriteLine("AfterStop called invoked, services are stopping"); });

                x.ConfigureService<TopshelfHostService>(s =>
                {
                    s.Named("Topshelf.Host");
                    s.HowToBuildService(name => new TopshelfHostService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("Topshelf Hosting Service");
                x.SetDisplayName("Topshelf.Host");
                x.SetServiceName("Topshelf.Host");
            });

            Runner.Host(cfg, args);
        }
    }
}
