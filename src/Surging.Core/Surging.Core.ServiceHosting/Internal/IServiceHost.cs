using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Surging.Core.ServiceHosting.Internal
{
   public interface IServiceHost : IDisposable
    {
        /// <summary>
        /// 运行主机
        /// </summary>
        /// <returns></returns>
        IDisposable Run();
        /// <summary>
        /// 初始化容器
        /// </summary>
        /// <returns></returns>
        IContainer Initialize();
    }
}
