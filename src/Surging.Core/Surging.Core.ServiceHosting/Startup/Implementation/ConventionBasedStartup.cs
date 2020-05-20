using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Surging.Core.ServiceHosting.Startup.Implementation
{
    /// <summary>
    /// 如果我们再应用中将服务和中间件注册的实现定义在启动类型中，当WebHost被启动的时候，会创建一个
    /// 类型为ConventionBasedStartup的Startup对象
    /// </summary>
    public class ConventionBasedStartup : IStartup
    {
        private readonly StartupMethods _methods;

        /// <summary>
        /// ConventionBasedStartup 是根据一个类型为StartupMethods对象创建的。
        /// </summary>
        /// <param name="methods"></param>
        public ConventionBasedStartup(StartupMethods methods)
        {
            _methods = methods;
        }

        public void Configure(IContainer app)
        {
            try
            {
                _methods.ConfigureDelegate(app);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }

                throw;
            }
        }

        public IContainer ConfigureServices(ContainerBuilder services)
        {
            try
            {
                return _methods.ConfigureServicesDelegate(services);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }

                throw;
            }
        }
    }
}