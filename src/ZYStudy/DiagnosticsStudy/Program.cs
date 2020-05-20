using System;

namespace DiagnosticsStudy
{
    /// <summary>
    /// Microsoft.Diagnostics.Tracing.TraceEvent 这个包  用来为Windows时间追踪(ETW) 剔红一个强大的支持，
    /// 使用这个包可以很容易的为我在云环境和生产环境来提供
    /// 端到端的监控日志时间，它轻量级、高效，并且可以和系统日志进行交互
    /// 
    /// Diagnostics的定义:在应用程序出现问题的时候，特别是出现可用性 或者 性能问题的时候，开发人员或则IT人员经常会对这些问题
    /// 花大量的时间来进行诊断，很多时候生产环境的问题都无法复现，这可能对业务造成很大的影响，
    /// Diagnostics就是提供一组功能,让我们能够很方便的记录(在应用程序运行期间发生的关键性操作，以及执行时间等)，使管理员排查
    /// 问题出现的根本原因
    /// 
    /// 从宏观的角度来说这属于 APM(Application Performance Management) 的一部分，但AMP不仅仅只有这些
    /// 
    /// .NET core 之全新DiagnosticSource
    /// 新的DiagnosticSource非常简单，它允许你在生产环境记录丰富的payload数据，然后你可以用另外一个消费者消费感兴趣的记录
    /// 
    /// 我们先说说DiagnosticSource和上面的EventSource的区别，他们的架构设计有点类似，主要区别是EventSource 它记录的数据是
    /// 可序列化的数据，会被消费(进程外)，所以要求记录的对象必须是可被序列化。而DiagnosticSource被设计在进程内处理数据，
    /// 所以通过它可以拿到更多详细数据信息，它支持非序列化的对象，比如Httpcontext，HttpResponseMessage等。如果你想在
    /// EventSource 中获取DiagnosticSource中的事件数据，可以通过DiagnosticSourceEventSource 这个对象来进行数据桥接
    /// 
    /// DiagnosticSource从命名上来看它是一个监听诊断信息的对象，他确实是一个用来接收事件的类，在.Net Core 中DiagnosticSource
    /// 它其实是一个抽象类，定义了记录事件日志需要的方法，那么我们在使用的时候，就需要使用具体的对象
    /// DiagnosticListener 就是 DiagnosticSource 的默认实现
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
