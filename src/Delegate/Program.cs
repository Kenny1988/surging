using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DelegateStudy
{
    /// <summary>
    /// 模拟.net core 管道实现原理
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Action<string> a = s => Console.WriteLine(s);
            Host test = new Host().RegisterServices(a).Run();
            Console.WriteLine("End");
            Console.ReadKey();

        }
    }

    public class Host
    {
        private readonly List<Action<string>> _stringDelegates;


        public Host()
        {
            _stringDelegates = new List<Action<string>>();
        }

        public Host RegisterServices(Action<string> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _stringDelegates.Add(builder);
            return this;
        }
        public Host Run()
        {
            if (_stringDelegates != null)
            {
                foreach (var item in _stringDelegates)
                {
                    item("AAAAA");
                }
            }
            return this;
        }
        public Host Run1()
        {
            if (_stringDelegates != null)
            {
                foreach (var item in _stringDelegates)
                {
                    Task task1 = Task.Run(() => item("AAAAA"));
                    Task task2 = task1.ContinueWith(t =>
                    {
                        Console.WriteLine("SSS");
                    });
                }
            }
            return this;
        }
    }
}
