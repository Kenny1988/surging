using System;
using System.Collections.Generic;
using System.Linq;

namespace EqualsAndHashCode
{
    /// <summary>
    /// 默认的实现其实比较的是两个对象的内存地址(==操作符默认比较内存地址),值类型和string类型除外
    /// 因为所有值类型继承于System.Value(),而System.Value()同样继承与Object，但是System.ValueType()本身确实引用类型；
    /// 而System.Value()对Equals()和==操作符进行了重写，是逐字节进行比较的。而string类型是比较特殊的引用类型，
    /// 所以string在很多地方都是特殊处理的
    /// 
    /// 在值类型使用Equals是，使用了反射，在比较时会影响效率
    /// 
    /// GetHashCode() 在操作值类型的时候，也是被System.Value()重写
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            ////1、"==" 是比较引用地址
            //Test t1 = new Test() { Name = "张三", Age = 21 };
            //Test t2 = new Test() { Name = "张三", Age = 21 };
            //bool test1 = t1 == t2;
            //Console.WriteLine(test1);

            ////2、Equals 默认也是比较 引用地址，但是在使用Equals之前会调用GetHashCode方法，HashCode相同才会调用Equals方法
            //bool test2 = t1.Equals(t2);
            //Console.WriteLine(test1);

            // 应用  Linq  Uion
            //List<Test> list = new List<Test>() {
            //    new Test() { Name = "李四", Age = 21 },
            //    new Test() { Name = "王五", Age = 21 },
            //    new Test() { Name = "张三", Age = 21 },
            //    new Test() { Name = "张三", Age = 21 }
            //    };

            //var a = list.Distinct();
            //var aa = list.Distinct(new TestComparer());

            ////
            //List<Test> list1 = new List<Test>() {
            //    new Test() { Name = "李四", Age = 21 },
            //    };

            ////Union
            //var b =list.Union(list1);
            //var c = list.Union(list1, new TestComparer());

            //Console.WriteLine("Hello World!");
        }
    }

    public class Test
    {
        public string Name { get; set; }
        public int Age { get; set; }
        //public override bool Equals(object obj)
        //{
        //    var model = obj as Test;
        //    if (model == null)
        //        return false;
        //    if (obj.GetType() != GetType())
        //        return false;
        //    return model.Name == Name && model.Age == Age;
        //}

        //public override int GetHashCode()
        //{
        //    return ToString().GetHashCode();
        //}
        public static bool operator ==(Test model1, Test model2)
        {
            return Equals(model1, model2);
        }

        public static bool operator !=(Test model1, Test model2)
        {
            return !Equals(model1, model2);
        }

    }

    internal class TestComparer : IEqualityComparer<Test>
    {
        public bool Equals(Test x, Test y)
        {
            return x.Age == y.Age
          && x.Name == y.Name;
        }

        public int GetHashCode(Test obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
