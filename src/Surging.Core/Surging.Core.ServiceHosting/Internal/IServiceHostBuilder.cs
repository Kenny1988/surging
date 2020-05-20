using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.ServiceHosting.Internal
{
    public  interface IServiceHostBuilder
    {
        IServiceHost Build();
        /// <summary>
        /// 启用配置实例化依赖项容器，这可以被多次调用
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IServiceHostBuilder RegisterServices(Action<ContainerBuilder> builder);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        IServiceHostBuilder ConfigureLogging(Action<ILoggingBuilder> configure);
        /// <summary>
        /// 向容器添加服务，这可以调用多次，并且姐夫哦是可以叠加的
        /// </summary>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        IServiceHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);
        /// <summary>
        /// 为构建器本身设计进行配置。用于初始化IHostingEnviroment 以共稍后再构建过程中使用
        /// 这可以调用多次，并且结果是可以叠加的
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IServiceHostBuilder Configure(Action<IConfigurationBuilder> builder);

        IServiceHostBuilder MapServices(Action<IContainer> mapper);
         
    }
}
