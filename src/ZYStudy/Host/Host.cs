using System;

namespace Host
{
    /// <summary>
    /// Web Host:创建一个Asp.Net Core 的Web项目(如MVC 或 WebApi)，然后使用IHostService或者BackgroundService处理后台任务
    /// 这个方案是Web项目和后台任务混杂在一起运行
    /// 
    /// Generic Host:通过主机将HTTP管道从Web Host的API中分类出来，从而提供更多的主机选择方案，比如后台服务，费Http工作负载
    /// 同时可以方便使用基础功能如:配置  依赖注入和日志等
    /// 
    /// 通过主机(HostBuilder):该Host不处理HTTP请求的应用程序
    /// Host的目的是将HTTP请求管道从WebHost 中分离出来，已实现更多的主机方案。
    /// 
    /// Asp.net Core 应用程序配置并启动主机。主机负责应用程序启动和生命周期的管理
    /// 
    /// 主机只要需要配置服务器和请求处理管道。主机还可以配置日志、依赖注入、配置文件
    /// 
    /// Web应用程序的使用IWebHostBuilder实例创建主机
    /// </summary>
    public class Host

    {
    }
}
