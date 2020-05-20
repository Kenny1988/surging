using System;
using System.Collections.Generic;
using System.Linq;

namespace HashStudy
{
    /// <summary>
    /// 哈希（也叫散列）是一种查找算法（可用于插入）,哈希算法希望能做到不经过任何比较(发生冲突,还是需要少许比较)，
    /// 通过一次存取就能得到查找的数据。
    /// 因此哈希的关键在key和数据元素的存储位置之间建立一个确定的对应关系，每个key在哈希表中都有唯一的地址相对应
    /// (形成有限，连续的地址空间)，查找时根据对应关系经过一步计算 得到Key在散列表的位置
    /// 
    /// ①不同的key值，有哈希函数h(x)作用后可能映射到同一个哈希地址，这就是哈希冲突，冲突发生的概率取决于定义的哈希函数
    /// ②有哈希表作用后的哈希地址需要空间存储，这一些列连续相邻的地址空间叫哈希表、散列表
    /// 
    /// 处理哈希冲突可以分为两大类
    /// 1、开散列法发生冲突的元素存储于数组空间之外。可以把“开”字理解为需要另外“开辟”空间存储发生冲突的元素，又称【链地址法】
    /// 2、闭散列法发生冲突的元素存储于数组空间之内。可以把“闭”字理解为所有元素，不管是否有冲突，都“关闭”于数组之中，
    /// 闭散列法又称【开放定址法】,意指数组空间对所有元素，不管是否冲突都是开放的
    /// 
    /// 哈希表示用数组实现的一片连续的地址空间，两种冲突解决方案的区别在于发生冲突的元素是存储在这片数组空间之外还是空间之内
    /// 
    /// ①哈希函数  收敛函数，不可避免会冲突，需要思考设定一个均衡的哈希函数，使哈希地址尽可能均匀分布在哈希地址空间
    /// ②构造哈希表+冲突链表：装填因子loadfactor：  哈希表中已存入的记录数n与哈希地址空间大小m的比值，即a=n/m，a越小，冲突发生的
    /// 可能性就越小；a越大(最大可取1)，冲突发生的可能性就越大；另一方面，a越小，存储窨的利用率就越低，反之越高。
    /// 通常会把a控制在0.6-0.9范围内
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Object基类中有GetHashCode方法，HashCode是一个数字值，用于在【基于哈希特性的集合】中插入和查找某对象；
        /// GetHashCode方法为需要快速检查对象相等性的算法提供此哈希代码
        /// 
        /// 单纯判断【逻辑相等】时，本无所谓重写GetHashCode方法；但若在基于hash的集合中快速查找、插入某元素，则一定要重写GetHashCode方法
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            var footabllTeam = new List<TeamMember>
            {
                new TeamMember { Member = new Person { Name = "nash", Age=21 } , Sport="footabll"},
                new TeamMember { Member = new Person { Name = "Beckham", Age = 21 },Sport="footabll" }
            };
            var basketballTeam = new List<TeamMember>
            {
                new TeamMember { Member = new Person {  Name = "nash", Age=21 },Sport="basketball" },
                new TeamMember { Member = new Person { Name = "Kobe", Age = 21 },Sport = "basketball" }
            };


            //可以重写 ==
            // “==”操作符：判断引用相等
            Console.WriteLine($"足球队[0]和篮球队[0]是不同引用对象，footabllTeam[0].Member == basketballTeam[0].Member输出：{footabllTeam[0].Member == basketballTeam[0].Member}");

            // 对两个内存对象，判断逻辑相等
            Console.WriteLine($"足球队[0]和篮球队[0]逻辑上是一个人,footabllTeam[0].Member.Equals(basketballTeam[0].Member输出：{footabllTeam[0].Member.Equals(basketballTeam[0].Member)}");

            // 统计两个球队中所有队员, hash-base查找/插入，务必重写GetHashCode方法
            var members1 = footabllTeam.Select(x => x.Member).Union(basketballTeam.Select(x => x.Member));
            Console.WriteLine($"总成员人数：{members1.Count()} ");

            var members2 = footabllTeam.Select(x => x.Member).Union(basketballTeam.Select(x => x.Member), new PersonComparer());
            Console.WriteLine($"总成员人数：{members2.Count()} ");
            Console.Read();

            /*
             足球队[0]和篮球队[0]是不同引用对象，footabllTeam[0].Member == basketballTeam[0].Member输出：False
             足球队[0]和篮球队[0]逻辑上是一个人,footabllTeam[0].Member.Equals(basketballTeam[0].Member输出：True
             总成员人数：4
             总成员人数：3
             */
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        #region 另外一种写法
        //public override bool Equals(object obj)
        //{
        //    var person = obj as Person;
        //    return Name == person.Name && Age == person.Age;
        //}
        //public override int GetHashCode()
        //{
        //    return (Name + Age).GetHashCode();
        //} 
        #endregion
    }

    /// <summary>
    /// 体育小组成员
    /// </summary>
    public class TeamMember
    {
        public Person Member { get; set; }
        public string Sport { get; set; }
    }

    internal class PersonComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            return x.Age == y.Age
          && x.Name == y.Name
          && x.Age == y.Age;
        }

        public int GetHashCode(Person obj)
        {
            var a = obj.Age + obj.Name;
            return a.GetHashCode();
        }
    }
}
