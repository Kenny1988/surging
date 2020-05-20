using System;
using System.Collections.Generic;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            List<Action<string>> list = new List<Action<string>>();
            var i = 0;
            for ( i = 0; i < 10; i++)
            {
                list.Add(s =>
                {
                    Console.WriteLine(i);
                });
            }

            for ( i = 0; i < list.Count; i++)
            {
                list[i]("HAHA");
            }
            Console.ReadKey();
        }
    }
}
