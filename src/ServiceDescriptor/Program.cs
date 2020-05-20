using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceDescriptor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceDescriptor s1 = new ServiceDescriptor() { Id = "111", RoutePath = "AA", token = "ccc" };
            ServiceDescriptor s2 = new ServiceDescriptor() { Id = "111", RoutePath = "aa", token = "vvv" };

            var a = s1.Equals(s2);

            Console.WriteLine("Hello World!");
        }
    }
    /// <summary>
    /// 服务描述符
    /// </summary>
    [Serializable]
    public class ServiceDescriptor
    {
        public ServiceDescriptor()
        {
            //使用序号排序规则并忽略被比较字符串的大小写，对字符串进行比较
            Metadatas = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 访问的令牌
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string RoutePath { get; set; }
        /// <summary>
        /// 元数据
        /// </summary>
        public IDictionary<string, object> Metadatas { get; set; }
        /// <summary>
        /// 获取一个元数据
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="name">元数据名称</param>
        /// <param name="def">如果指定名称的元数据不存在则返回这个参数</param>
        /// <returns></returns>
        public T GetMetadata<T>(string name, T def = default(T))
        {
            if (!Metadatas.ContainsKey(name))
                return def;
            return (T)Metadatas[name];
        }

        #region Equality members

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            var model = obj as ServiceDescriptor;
            if (model == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            if (model.Id != Id)
                return false;

            return model.Metadatas.Count == Metadatas.Count && model.Metadatas.All(metadata =>
            {
                object value;
                if (!Metadatas.TryGetValue(metadata.Key, out value))
                    return false;
                if (metadata.Value == null && value == null)
                    return true;
                if (metadata.Value == null || value == null)
                    return false;
                return metadata.Value.Equals(value);
            });
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// 通过operator 来重载 ==运算符
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        /// <returns></returns>
        public static bool operator ==(ServiceDescriptor model1, ServiceDescriptor model2)
        {
            return Equals(model1, model2);
        }

        public static bool operator !=(ServiceDescriptor model1, ServiceDescriptor model2)
        {
            return !Equals(model1, model2);
        }

        #endregion Equality members
    }
}
