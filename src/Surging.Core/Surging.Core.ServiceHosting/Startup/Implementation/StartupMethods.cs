using Autofac;
using System;
using System.Diagnostics;

namespace Surging.Core.ServiceHosting.Startup.Implementation
{
    public class StartupMethods
    {
        public StartupMethods(object instance, Action<IContainer> configure, Func<ContainerBuilder, IContainer> configureServices)
        {
            Debug.Assert(configure != null);
            Debug.Assert(configureServices != null);

            StartupInstance = instance;
            ConfigureDelegate = configure;
            ConfigureServicesDelegate = configureServices;
        }

        public object StartupInstance { get; }

        //下面两个委托的实现定义在ConventionBasedStartup
        public Func<ContainerBuilder, IContainer> ConfigureServicesDelegate { get; }
        public Action<IContainer> ConfigureDelegate { get; }

    }
}