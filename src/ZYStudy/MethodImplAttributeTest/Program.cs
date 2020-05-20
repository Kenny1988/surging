using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MethodImplAttributeTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SyncHelp helper = new SyncHelp();

            #region 每隔1秒执行一次
            //Timer 对象采用的异步方式进行调用，所以虽然Execute方法执行的时间是5s
            //但是该方法依然是每个1s被执行一次
            //Timer timer = new Timer(delegate
            //   {
            //       helper.Execute();
            //   }, null, 0, 1000);
            #endregion 

            #region 每隔5秒执行一次 
            //[MethodImplAttribute(MethodImplOptions.Synchronized)] = lock(this)  相当于给当前实例加锁
            Timer timer1 = new Timer(delegate
            {
                helper.Execute1();
            }, null, 0, 1000);
            #endregion


            #region 每隔5秒执行一次
            //由于LockMyself()  是在另外一个线程中执行的，我们可以简单讲该方法的执行和Execute的第一次执行看做是同时进行的。
            //但是如果  MethodImplAttribute(MethodImplOptions.Synchronized)]是通过lock(this)的方式实现
            //那么 Execute必须在等待LockMyself方法执行结束将对自身的锁释放后才能的一执行
            //也就是说LockMyself和第一次Execute方法的执行应该相差5s
            //Thread thread = new Thread(delegate ()
            //{
            //    helper.LockMyself();
            //});
            //thread.Start();
            //Timer timer2 = new Timer(delegate
            //{
            //    helper.Execute();
            //}, null, 0, 1000);
            #endregion
            Console.Read();
        }
    }

    internal class SyncHelp
    {
        public void Execute()
        {
            Console.WriteLine($"Execute时间:{DateTime.Now}");
            Thread.Sleep(5000);
        }
        /// <summary>
        /// [MethodImplAttribute(MethodImplOptions.Synchronized)] = lock(this)
        /// 相当于对当前实例加锁
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Execute1()
        {
            Console.WriteLine($"Execute1时间:{DateTime.Now}");
            Thread.Sleep(5000);
        }
        /// <summary>
        /// 标记了[MethodImpl(MethodImplOptions.AggressiveInlining)]以便编译器尽可能的内联。
        /// 所谓内联，即将被调用函数的函数体代码直接的整个插入到该函数被调用处，而不是通过call语句进行。
        /// 当然，编译器在真正进行内联时，因为考虑到被内联函数的传入参数，自己的局部变量，以及返回值的因素，不仅仅只是进行简单的代码
        /// 拷贝，还要做很多细致的工作
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute2()
        {
            Console.WriteLine($"Execute2时间:{DateTime.Now}");
            Thread.Sleep(5000);
        }

        public void LockMyself()
        {
            lock (this)
            {
                Console.WriteLine("Lock myself at {0}", DateTime.Now);
                Thread.Sleep(5000);
                Console.WriteLine("Unlock myself at {0}", DateTime.Now);
            }
        }
    }
}
