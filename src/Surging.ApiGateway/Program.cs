using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using Surging.Core.ApiGateWay;
using Surging.Core.Codec.MessagePack;
using Surging.Core.Consul;
using Surging.Core.Consul.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.DotNetty;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using System;
using System.IO;


namespace Surging.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseUrls("http://*:8001")//网关的监听端口，也就是部署的时候容器运行的端口，自己根据情况进行配置
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
            host.Run();
          
        }
    }
}
