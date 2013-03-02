using System;
using System.Collections.Generic;
using System.Reflection;
using EasyArchitecture.Configuration;
using EasyArchitecture.Configuration.Exceptions;
using EasyArchitecture.Core.Plugin;
using EasyArchitecture.Instances.Caching;
using EasyArchitecture.Instances.IoC;
using EasyArchitecture.Instances.Log;
using EasyArchitecture.Instances.Persistence;
using EasyArchitecture.Instances.Storage;
using EasyArchitecture.Instances.Translation;
using EasyArchitecture.Instances.Validation;
using EasyArchitecture.Plugin.Contracts.Caching;
using EasyArchitecture.Plugin.Contracts.IoC;
using EasyArchitecture.Plugin.Contracts.Log;
using EasyArchitecture.Plugin.Contracts.Persistence;
using EasyArchitecture.Plugin.Contracts.Storage;
using EasyArchitecture.Plugin.Contracts.Translation;
using EasyArchitecture.Plugin.Contracts.Validation;

namespace EasyArchitecture.Core
{
    public static class InstanceProvider
    {
        private static readonly Dictionary<string, List<AbstractPlugin>> Factories = new Dictionary<string, List<AbstractPlugin>>();
        private static readonly Dictionary<Type, Type> Map = new Dictionary<Type, Type>();

        static InstanceProvider()
        {
            Map.Add(typeof(Persistence), typeof(IInstanceProvider<IPersistence>));
            Map.Add(typeof(Container), typeof(IInstanceProvider<IContainer>));
            Map.Add(typeof(Translator), typeof(IInstanceProvider<ITranslator>));
            Map.Add(typeof(Validator), typeof(IInstanceProvider<IValidator>));
            Map.Add(typeof(Storer), typeof(IInstanceProvider<IStorage>));
            Map.Add(typeof(Cache), typeof(IInstanceProvider<ICache>));
            Map.Add(typeof(Logger), typeof(IInstanceProvider<ILogger>));
        }

        public static T GetInstance<T>() where T : class
        {
            var context = LocalThreadStorage.GetCurrentContext();
            if(context==null)
                throw new NotConfiguredException();

            var instance = LocalThreadStorage.GetCurrentContext().GetInstance<T>();
            if (instance == null)
            {
                var moduleName = LocalThreadStorage.GetCurrentContext().Name;
                if (!Factories.ContainsKey(moduleName))
                    throw new NotConfiguredException(moduleName);

                var plugins = Factories[moduleName];
                var providerType = typeof(T);
                var pluginType = Map[providerType];
                var plugin = plugins.Find(pluginType.IsInstanceOfType);
                var pluginInstance = pluginType.InvokeMember("GetInstance", BindingFlags.InvokeMethod, null, plugin, null);
                instance = (T)providerType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0].Invoke(new[]{pluginInstance});
                LocalThreadStorage.GetCurrentContext().SetInstance(instance);
            }
            
            return (T)instance;
        }

        internal static void Configure(string moduleName, PluginConfiguration pluginConfiguration)
        {
            var moduleAssemblies = AssemblyManager.GetModuleAssemblies(moduleName);
            var inspectors = new List<PluginInspector>();

            var plugins = pluginConfiguration.GetPluginList();
            foreach (var plugin in plugins)
            {
                PluginInspector pluginInspector;
                plugin.Configure(moduleAssemblies, out pluginInspector);
                inspectors.Add(pluginInspector);
            }

            Factories[moduleAssemblies.ModuleName] = plugins;

            var pluginInfo = new PluginInspectorExtrator(inspectors);

            LocalThreadStorage.CreateContext(moduleName);

            GetInstance<Logger>().LogInfo(pluginInfo, null);
        }
    }

}