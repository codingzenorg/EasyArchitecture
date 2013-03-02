﻿using System;
using System.Reflection;
using EasyArchitecture.Core;
using EasyArchitecture.Plugins.Contracts.Persistence;
using FluentNHibernate.Cfg;
using NHibernate;

namespace EasyArchitecture.Plugins.NHibernate.Persistence
{
    public class NHibernatePlugin : AbstractPlugin,IPersistencePlugin
    {
        private ISessionFactory _sessionFactory;
 
        public IPersistence GetInstance()
        {
            var session = _sessionFactory.OpenSession();
            return new NHibernatePersistence(session);
        }

        protected override void ConfigurePlugin(ModuleAssemblies moduleAssemblies, PluginInspector pluginInspector)
        {
            var assembly = moduleAssemblies.InfrastructureAssembly;
            var nhibernateConfigurationType = Array.Find(assembly.GetExportedTypes(), t => typeof(INHibernateConfiguration).IsAssignableFrom(t));

            INHibernateConfiguration nhibernateConfiguration = null;
            if (nhibernateConfigurationType != null)
            {
                nhibernateConfiguration = (INHibernateConfiguration)nhibernateConfigurationType.Assembly.CreateInstance(nhibernateConfigurationType.FullName);
            }

            _sessionFactory = GetConfiguredSessionFactory(assembly,nhibernateConfiguration);
        }

        private static ISessionFactory GetConfiguredSessionFactory(Assembly mappingAssembly, INHibernateConfiguration configuration )
        {
            return Fluently.Configure()
                .Database(configuration.ConfigureDatabase())
                .ProxyFactoryFactory("NHibernate.Bytecode.DefaultProxyFactoryFactory, NHibernate")
                .Mappings(m => m.FluentMappings.AddFromAssembly(mappingAssembly))
                .BuildConfiguration()
                .BuildSessionFactory();
        }
    }
}
