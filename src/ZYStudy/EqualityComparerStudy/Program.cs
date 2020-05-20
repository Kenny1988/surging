using System;
using System.Collections.Generic;
using System.Linq;

namespace EqualityComparerStudy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Man> list = new List<Man>(){
              new Man(){Age=21,Name="Adam",Adress="Shenzhen",Weight=60,Height=170},
              new Man(){Age=21,Name="Adam",Adress="Shenzhen",Weight=60,Height=170},
                new Man(){Age=22,Name="Ad1am",Adress="Sh1enzhen",Weight=110,Height=111}
              };
            //直接去重之后 集合中依然是两个Adam
            //实际上Distinct方法内进行比较的是声明的引用，而不是对象的属性，就算是  两个属性一模一样的对象用Equals()方法的到的是false
            //因此我们对对象集合使用Distinct方法时使用重载
            var count1 = list.Distinct();//的数量为3

            var count2 = list.Distinct(new ManComparer());//count2  数量为2
            Console.WriteLine("Hello World!");
        }
    }
    /// <summary>
    /// IEqualityComparer内部会在使用Equals前，先试用GetHashCode方法，在两个对象的HashCode都相同时即刻判断对象相等
    /// 而对两个对象HashCode不相同时，Equals方法就不会被调用
    /// </summary>
    internal class ManComparer : IEqualityComparer<Man>
    {
        public bool Equals(Man x, Man y)
        {
            return x.Age == y.Age
          && x.Name == y.Name
          && x.Adress == y.Adress
          && x.Weight == y.Weight
          && x.Height == y.Height;
        }

        public int GetHashCode(Man obj)
        {
          return ToString().GetHashCode();
        }
    }

    internal class Man
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
    }
}
