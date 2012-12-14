﻿using EasyArchitecture.Caching.Plugin.BultIn;
using EasyArchitecture.Caching.Plugin.Contracts;
using EasyArchitecture.Configuration;
using EasyArchitecture.Configuration.Exceptions;
using EasyArchitecture.Configuration.Instance;
using EasyArchitecture.IoC.Plugin.BultIn;
using EasyArchitecture.IoC.Plugin.Contracts;
using EasyArchitecture.Log.Plugin.BultIn;
using EasyArchitecture.Log.Plugin.Contracts;
using EasyArchitecture.Persistence.Plugin.BultIn;
using EasyArchitecture.Persistence.Plugin.Contracts;
using EasyArchitecture.Runtime;
using EasyArchitecture.Storage.Plugin.BultIn;
using EasyArchitecture.Storage.Plugin.Contracts;
using EasyArchitecture.Translation.Plugin.BultIn;
using EasyArchitecture.Translation.Plugin.Contracts;
using EasyArchitecture.Validation.Plugin.BultIn;
using EasyArchitecture.Validation.Plugin.Contracts;
using NUnit.Framework;

namespace EasyArchitecture.Tests.Mechanisms
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void Should_use_default_plugins_if_no_configuration_are_specified()
        {
            Configure
                .For("Application4Test")
                .Done();

            Verify();
        }


        [Test]
        public void Should_not_find_configuration_instance()
        {
            LocalThreadStorage.SetCurrentModuleName("None");
            Assert.That(() => ConfigurationSelector.SelectorByThread(), Throws.TypeOf<NotConfiguredModuleException>());
        }

        [Test]
        public void Should_use_a_type_of_module_instead_module_name_to_configure()
        {
            Configure
                .For<Application4Test.Application.Contracts.IDogFacade>()
                .Done();

            Verify();
        }

        [Test]
        public void Should_use_default_plugins_if_they_are_not_specified()
        {
            Configure
                .For("Application4Test")
                    .Log()
                    .Translation()
                    .Persistence()
                    .DependencyInjection()
                    .Cache()
                    .Storage()
                    .Validator()
                    .Done();

            Verify();
        }

        [Test]
        public void Should_use_specified_plugins_implementations()
        {
            Configure
                .For("Application4Test")
                    .Log(new LoggerPlugin())
                    .Translation(new TranslatorPlugin())
                    .Persistence(new PersistencePlugin())
                    .DependencyInjection(new IocPlugin())
                    .Cache(new CachePlugin())
                    .Storage(new StoragePlugin())
                    .Validator(new ValidatorPlugin())
                    .Done();

            Verify();
        }

        [Test]
        public void Should_use_specified_plugin_types()
        {
            Configure
                .For("Application4Test")
                    .Log<LoggerPlugin>()
                    .Translation<TranslatorPlugin>()
                    .Persistence<PersistencePlugin>()
                    .DependencyInjection<IocPlugin>()
                    .Cache<CachePlugin>()
                    .Storage<StoragePlugin>()
                    .Validator<ValidatorPlugin>()
                    .Done();

            Verify();
        }

        private static void Verify()
        {
            var loggerPlugin = ConfigurationSelector.Configurations["Application4Test"].Plugins[typeof(ILoggerPlugin)];
            var translatorPlugin = ConfigurationSelector.Configurations["Application4Test"].Plugins[typeof(ITranslatorPlugin)];
            var persistencePlugin = ConfigurationSelector.Configurations["Application4Test"].Plugins[typeof(IPersistencePlugin)];
            var iocPlugin = ConfigurationSelector.Configurations["Application4Test"].Plugins[typeof(IIoCPlugin)];
            var validatorPlugin = ConfigurationSelector.Configurations["Application4Test"].Plugins[typeof(IValidatorPlugin)];
            var cachePlugin = ConfigurationSelector.Configurations["Application4Test"].Plugins[typeof(ICachePlugin)];
            var storagePlugin = ConfigurationSelector.Configurations["Application4Test"].Plugins[typeof(IStoragePlugin)];

            Assert.That(loggerPlugin, Is.TypeOf<LoggerPlugin>());
            Assert.That(translatorPlugin, Is.TypeOf<TranslatorPlugin>());
            Assert.That(persistencePlugin, Is.TypeOf<PersistencePlugin>());
            Assert.That(iocPlugin, Is.TypeOf<IocPlugin>());
            Assert.That(validatorPlugin, Is.TypeOf<ValidatorPlugin>());
            Assert.That(cachePlugin, Is.TypeOf<CachePlugin>());
            Assert.That(storagePlugin, Is.TypeOf<StoragePlugin>());
        }
    }
}