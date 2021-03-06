using System;
using System.Collections;
using System.Collections.Generic;
using EasyArchitecture.Core;
using EasyArchitecture.Plugins.Contracts.Persistence;

namespace EasyArchitecture.Plugins.BultIn.Persistence
{
    internal class PersistencePlugin : Plugin,IPersistencePlugin
    {
        private readonly Dictionary<Type, ArrayList> _dataBase = new Dictionary<Type, ArrayList>();

        protected override void ConfigurePlugin(PluginConfiguration pluginConfiguration, PluginInspector pluginInspector)
        {
        }

        public IPersistence GetInstance()
        {
            return new Persistence(_dataBase);
        }
    }
}