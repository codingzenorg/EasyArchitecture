﻿using EasyArchitecture.Configuration;
using EasyArchitecture.Mechanisms.IoC;
using Exemplo.Produto.Application.Contracts;
using Exemplo.Produto.Domain.Repositories;
using Exemplo.Produto.Tests.Repositories;
using NUnit.Framework;

namespace Exemplo.Produto.Tests
{
    [TestFixture]
    public class NHibernateTest : BaseTest
    {
        [SetUp]
        public override void SetUp()
        {
            Configure
                .For<IProdutoFacade>()
                .Persistence<EasyArchitecture.Plugins.NHibernate.Persistence.NHibernatePlugin>()
                .Done();

            //Container.Register<IProdutoRepository, ProdutoNHibernateRepository>();
        }
    }
}