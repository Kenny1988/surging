using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Surging.Core.ServiceHosting.Internal
{
    /// <summary>
    /// Host的生命周期
    /// </summary>
   public interface IHostLifetime
    {
         
        Task WaitForStartAsync(CancellationToken cancellationToken);
 
        Task StopAsync(CancellationToken cancellationToken);
    }
}
