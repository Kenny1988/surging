// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.ServiceHosting.Startup.Implementation;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Surging.Core.ServiceHosting.Internal.Implementation
{
    public class StartupLoader
    {
        /// <summary>
        /// ����һ��StartupMethods��ʵ��������������������Ӧ�ó������Ͳ���Ӧ�ó��������ܵ���
        /// ʹ�û���Լ��������ʱ����ʼ������Ĺ���������ʾ��
        /// ���������Ҿ���ǩ��Ϊ  IServiceProvider ConfigureServices(IServiceCollection ***)�ķ���
        /// ����Void ConfigureServices(IServiceCollertion ***)
        /// 
        /// ConfigureServices����ֵ��void
        /// </summary>
        /// <param name="hostingServiceProvider"></param>
        /// <param name="config"></param>
        /// <param name="startupType"></param>
        /// <param name="environmentName"></param>
        /// <returns></returns>
        public static StartupMethods LoadMethods(IServiceProvider hostingServiceProvider, IConfigurationBuilder config, Type startupType, string environmentName)
        {
            var configureMethod = FindConfigureDelegate(startupType, environmentName);
            var servicesMethod = FindConfigureServicesDelegate(startupType, environmentName);
            var configureContainerMethod = FindConfigureContainerDelegate(startupType, environmentName);

            object instance = null;
            if (!configureMethod.MethodInfo.IsStatic || (servicesMethod != null && !servicesMethod.MethodInfo.IsStatic))
            {
                instance = ActivatorUtilities.CreateInstance(hostingServiceProvider, startupType,config);
            }

            var configureServicesCallback = servicesMethod.Build(instance);
            var configureContainerCallback = configureContainerMethod.Build(instance);

            Func<ContainerBuilder, IContainer> configureServices = services =>
            {
                IContainer applicationServiceProvider = configureServicesCallback.Invoke(services);
                if (applicationServiceProvider != null)
                {
                    return applicationServiceProvider;
                }
                if (configureContainerMethod.MethodInfo != null)
                {
                    var serviceProviderFactoryType = typeof(IServiceProviderFactory<>).MakeGenericType(configureContainerMethod.GetContainerType());
                    var serviceProviderFactory = hostingServiceProvider.GetRequiredService(serviceProviderFactoryType);
                    var builder = serviceProviderFactoryType.GetMethod(nameof(DefaultServiceProviderFactory.CreateBuilder)).Invoke(serviceProviderFactory, new object[] { services });
                    configureContainerCallback.Invoke(builder);
                    applicationServiceProvider = (IContainer)serviceProviderFactoryType.GetMethod(nameof(DefaultServiceProviderFactory.CreateServiceProvider)).Invoke(serviceProviderFactory, new object[] { builder });
                }

                return applicationServiceProvider;
            };

            return new StartupMethods(instance, configureMethod.Build(instance), configureServices);
        }

        /// <summary>
        /// ������(Startup)�Ľ���
        /// 
        /// ���û��ͨ������WebHostBuilder����չ����(UseStartup)��ʾע�ᣬ��ôFindStartupType�����ᱻ��������������ȷ��
        /// �������͡���FindStartupType������ִ�в��ɹ��������ṩ�ĳ���֮�����ᰴ��Լ������������ȫ���Ӹó��򼯼���
        /// �������ͣ���ѡ����������ȫ�������������ȼ��������£������������ȣ��������ռ����ȣ���
        /// Startup{EnvironmentName} ���������ռ䣩
        /// {StartupAssemblyName}.Startup{EnvironmentName}
        /// Startup���������ռ䣩
        /// {StartupAssemblyName}.Startup{EnvironmentName}
        /// **. Startup{EnvironmentName}�����������ռ䣩
        /// **. Startup�����������ռ䣩
        /// </summary>
        /// <param name="startupAssemblyName">��������</param>
        /// <param name="environmentName">��ǰ���л���</param>
        /// <returns></returns>
        public static Type FindStartupType(string startupAssemblyName, string environmentName)
        {
            if (string.IsNullOrEmpty(startupAssemblyName))
            {
                throw new ArgumentException(
                    string.Format("'{0}' ����Ϊ��.",
                    nameof(startupAssemblyName)),
                    nameof(startupAssemblyName));
            }

            var assembly = Assembly.Load(new AssemblyName(startupAssemblyName));
            if (assembly == null)
            {
                throw new InvalidOperationException(String.Format("���� '{0}' �����ܼ���", startupAssemblyName));
            }

            var startupNameWithEnv = "Startup" + environmentName;
            var startupNameWithoutEnv = "Startup";
            var type =
                assembly.GetType(startupNameWithEnv) ??
                assembly.GetType(startupAssemblyName + "." + startupNameWithEnv) ??
                assembly.GetType(startupNameWithoutEnv) ??
                assembly.GetType(startupAssemblyName + "." + startupNameWithoutEnv);

            if (type == null)
            {
                var definedTypes = assembly.DefinedTypes.ToList();
                var startupType1 = definedTypes.Where(info => info.Name.Equals(startupNameWithEnv, StringComparison.OrdinalIgnoreCase));
                var startupType2 = definedTypes.Where(info => info.Name.Equals(startupNameWithoutEnv, StringComparison.OrdinalIgnoreCase));
                var typeInfo = startupType1.Concat(startupType2).FirstOrDefault();
                if (typeInfo != null)
                {
                    type = typeInfo.AsType();
                }
            }

            if (type == null)
            {
                throw new InvalidOperationException(String.Format("���� '{0}' ���� '{1}' ���ܴӳ��� '{2}'�ҵ�.",
                    startupNameWithEnv,
                    startupNameWithoutEnv,
                    startupAssemblyName));
            }

            return type;
        }

        private static ConfigureBuilder FindConfigureDelegate(Type startupType, string environmentName)
        {
            var configureMethod = FindMethod(startupType, "Configure{0}", environmentName, typeof(void), required: true);
            return new ConfigureBuilder(configureMethod);
        }

        private static ConfigureContainerBuilder FindConfigureContainerDelegate(Type startupType, string environmentName)
        {
            var configureMethod = FindMethod(startupType, "Configure{0}Container", environmentName, typeof(void), required: false);
            return new ConfigureContainerBuilder(configureMethod);
        }

        private static ConfigureServicesBuilder FindConfigureServicesDelegate(Type startupType, string environmentName)
        {
            var servicesMethod = FindMethod(startupType, "Configure{0}Services", environmentName, typeof(IContainer), required: false)
                ?? FindMethod(startupType, "Configure{0}Services", environmentName, typeof(void), required: false);
            return new ConfigureServicesBuilder(servicesMethod);
        }

        private static MethodInfo FindMethod(Type startupType, string methodName, string environmentName, Type returnType = null, bool required = true)
        {
            var methodNameWithEnv = string.Format(CultureInfo.InvariantCulture, methodName, environmentName);
            var methodNameWithNoEnv = string.Format(CultureInfo.InvariantCulture, methodName, "");

            var methods = startupType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var selectedMethods = methods.Where(method => method.Name.Equals(methodNameWithEnv, StringComparison.OrdinalIgnoreCase)).ToList();
            if (selectedMethods.Count > 1)
            {
                throw new InvalidOperationException(string.Format("������ط���  '{0}' ��֧��.", methodNameWithEnv));
            }
            if (selectedMethods.Count == 0)
            {
                selectedMethods = methods.Where(method => method.Name.Equals(methodNameWithNoEnv, StringComparison.OrdinalIgnoreCase)).ToList();
                if (selectedMethods.Count > 1)
                {
                    throw new InvalidOperationException(string.Format("������ط���  '{0}' ��֧��.", methodNameWithNoEnv));
                }
            }

            var methodInfo = selectedMethods.FirstOrDefault();
            if (methodInfo == null)
            {
                if (required)
                {
                    throw new InvalidOperationException(string.Format("�����������Ʊ���Ϊ'{0}' ���� '{1}' �Ҳ��� '{2}' ����.",
                        methodNameWithEnv,
                        methodNameWithNoEnv,
                        startupType.FullName));

                }
                return null;
            }
            if (returnType != null && methodInfo.ReturnType != returnType)
            {
                if (required)
                {
                    throw new InvalidOperationException(string.Format(" '{0}'�ķ��������� '{1}' �����з������� '{2}'.",
                        methodInfo.Name,
                        startupType.FullName,
                        returnType.Name));
                }
                return null;
            }
            return methodInfo;
        }
    }
}