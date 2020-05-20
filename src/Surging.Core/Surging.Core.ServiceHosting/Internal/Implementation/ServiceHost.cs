using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.ServiceHosting.Startup;

namespace Surging.Core.ServiceHosting.Internal.Implementation
{
    public class ServiceHost : IServiceHost
    {
        /// <summary>
        ///容器构建器
        /// </summary>
        private readonly ContainerBuilder _builder;
        private IStartup _startup;
        /// <summary>
        /// 容器服务
        /// </summary>
        private IContainer _applicationServices;
        /// <summary>
        /// 主机生命周期
        /// </summary>
        private readonly IHostLifetime _hostLifetime;
        /// <summary>
        /// 服务提供器
        /// </summary>
        private readonly IServiceProvider _hostingServiceProvider;
        /// <summary>
        /// 容器委托
        /// </summary>
        private readonly List<Action<IContainer>> _mapServicesDelegates;
        /// <summary>
        /// 应用程生命周期
        /// </summary>
        private IApplicationLifetime _applicationLifetime;

        public ServiceHost(ContainerBuilder builder,
            IServiceProvider hostingServiceProvider,
            IHostLifetime hostLifetime,
             List<Action<IContainer>> mapServicesDelegate)
        {
            _builder = builder;
            _hostingServiceProvider = hostingServiceProvider;
            _hostLifetime = hostLifetime;
            _mapServicesDelegates = mapServicesDelegate; 
        }

        public void Dispose()
        {
            (_hostingServiceProvider as IDisposable)?.Dispose();
        }

        public IDisposable Run()
        {
            RunAsync().GetAwaiter().GetResult();
            return this;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_applicationServices != null)
                MapperServices(_applicationServices); 

            if (_hostLifetime != null)
            {
                _applicationLifetime = _hostingServiceProvider.GetService<IApplicationLifetime>();
                await _hostLifetime.WaitForStartAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _applicationLifetime?.NotifyStarted();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        { 

            using (var cts = new CancellationTokenSource(2000))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken))
            {
                var token = linkedCts.Token; 
                _applicationLifetime?.StopApplication(); 
                token.ThrowIfCancellationRequested();
                await _hostLifetime.StopAsync(token); 
                _applicationLifetime?.NotifyStopped();
            }
        }

        public IContainer Initialize()
        {
            if (_applicationServices == null)
            {
                _applicationServices = BuildApplication();
            }
            return _applicationServices;
        }

        private void EnsureApplicationServices()
        {
            if (_applicationServices == null)
            {
                EnsureStartup();
                _applicationServices = _startup.ConfigureServices(_builder);
            }
        }

        private void EnsureStartup()
        {
            if (_startup != null)
            {
                return;
            }

            _startup = _hostingServiceProvider.GetRequiredService<IStartup>();
        }

        private IContainer BuildApplication()
        {
            try
            {
                EnsureApplicationServices();
                Action<IContainer> configure = _startup.Configure;
                if (_applicationServices == null)
                    _applicationServices = _builder.Build();
                configure(_applicationServices);
                return _applicationServices;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("应用程序启动异常: " + ex.ToString());
                throw;
            }
        }
        
        private void MapperServices(IContainer mapper)
        {
            foreach (var mapServices in _mapServicesDelegates)
            {
                mapServices(mapper);
            }
        }   
    }
}
