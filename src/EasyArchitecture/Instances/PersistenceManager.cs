﻿using EasyArchitecture.Internal;
using EasyArchitecture.Plugins;

namespace EasyArchitecture.Instances
{
    public class PersistenceManager
    {
        private readonly string _moduleName;
        private readonly IPersistencePlugin _plugin;

        internal PersistenceManager(EasyConfig easyConfig)
        {
            _moduleName = easyConfig.ModuleName;

            _plugin = (IPersistencePlugin)easyConfig.Plugins[typeof(IPersistencePlugin)];

            _plugin.Configure( _moduleName,"",easyConfig.InfrastructureAssembly);
        }

        internal object GetSession()
        {
            return LocalThreadStorage.RecoverSession(_moduleName) ?? _plugin.GetSession(_moduleName);
        }

        internal void BeginTransaction()
        {
            var session = GetSession();

            _plugin.BeginTransaction(session);

            LocalThreadStorage.StoreSession(_moduleName, session);
        }

        internal void CommitTransaction()
        {
            var session = LocalThreadStorage.RecoverSession(_moduleName);

            _plugin.CommitTransaction(session);

            LocalThreadStorage.ClearSession(_moduleName);
        }

        internal void RollbackTransaction()
        {
            var session = LocalThreadStorage.RecoverSession(_moduleName);

            _plugin.RollbackTransaction(session);

            LocalThreadStorage.ClearSession(_moduleName);
        }
    }
}
