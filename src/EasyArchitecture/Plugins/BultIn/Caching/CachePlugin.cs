﻿using EasyArchitecture.Core;
using EasyArchitecture.Plugins.Contracts.Caching;

namespace EasyArchitecture.Plugins.BultIn.Caching
{
    internal class CachePlugin :AbstractPlugin,ICachePlugin
    {
        protected override void ConfigurePlugin(ModuleAssemblies moduleAssemblies, PluginInspector pluginInspector)
        {
        }

        public ICache GetInstance()
        {
            return new Cache();
        }
    }
}