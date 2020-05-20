using Autofac;


namespace Surging.Core.ServiceHosting.Startup
{
    /// <summary>
    /// 派生于StartupBase的Startup类型如果没有重写ConfigureServices方法，他们实际只
    /// 关心中间件的注册，而不需要注册额外的服务
    /// 
    /// 
    /// 中间件的注册可以采用两种方式，最简单的方式就是直接调用IWebHostBuilder的Configure方法
    /// 借助一个类型为Action<IApplicationBuilder>的委托对象将中间注册到提供Application对象上
    /// 如果应用通过直接直接调用Configure方法来注册所需的中间件，WebHost在启动的时候会创建
    /// 一个类型为DelegateStartup的Startup对象来完成真正的中间件注册工作。
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public interface IStartup
    {
        IContainer ConfigureServices(ContainerBuilder services);

        void Configure(IContainer app);
    }
}
