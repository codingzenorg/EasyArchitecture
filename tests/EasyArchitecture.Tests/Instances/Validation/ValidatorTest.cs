﻿using System.Collections.Generic;
using EasyArchitecture.Core;
using EasyArchitecture.Instances.Validation;
using EasyArchitecture.Plugins.Contracts.Log;
using EasyArchitecture.Plugins.Contracts.Validation;
using EasyArchitecture.Tests.Instances.Validation.Stuff;
using NUnit.Framework;
using Rhino.Mocks;
using Validator = EasyArchitecture.Mechanisms.Validation.Validator;

namespace EasyArchitecture.Tests.Instances.Validation
{
    [TestFixture]
    public class ValidatorTest
    {
        private Dog _oldDog;
        private Dog _youngDog;
        private MockRepository _mockery;
        private IValidator _instancePlugin;

        [SetUp]
        public void SetUp()
        {
            _mockery = new MockRepository();
            _instancePlugin = _mockery.DynamicMock<IValidator>();

            ThreadContext.Create("EasyArchitecture.Tests");
            ThreadContext.GetCurrent().SetInstance(new EasyArchitecture.Instances.Validation.Validator(_instancePlugin));
            ThreadContext.GetCurrent().SetInstance(new EasyArchitecture.Instances.Log.Logger(MockRepository.GenerateStub<ILogger>()));

            _oldDog = new Dog { Age = 15, Name = "Old Dog" };
            _youngDog = new Dog { Age = 5, Name = "Young Dog" };
        }

        [Test]
        public void Should_throws_invalid_entity_exception_to_invalid_entity()
        {
            Expect.Call(_instancePlugin.Validate(_oldDog)).Return(new List<string>(){"There's no dog so old" });
            _mockery.ReplayAll();

            Assert.That(() => Validator.This(_oldDog).IsValid(), Throws.TypeOf<InvalidEntityException>());

            _mockery.VerifyAll();

        }

        [Test]
        public void Can_pass_valid_entity()
        {
            Expect.Call(_instancePlugin.Validate(_youngDog)).Return(new List<string>());
            _mockery.ReplayAll();

            Assert.That(() => Validator.This(_youngDog).IsValid(), Throws.Nothing);

            _mockery.VerifyAll();
        }

        [Test]
        public void Should_return_validation_messages_to_invalid_entity()
        {
            var expected = new System.Collections.Generic.List<string>() { "There's no dog so old" };

            Expect.Call(_instancePlugin.Validate(_oldDog)).Return(new List<string>() { "There's no dog so old" });
            _mockery.ReplayAll();

            var actual = Validator.This(_oldDog).HasMessages();
            Assert.That(actual, Is.EqualTo(expected));

            _mockery.VerifyAll();
        }

        [Test]
        public void Should_not_return_messages_valid_entity()
        {
            var expected = new List<string>();

            Expect.Call(_instancePlugin.Validate(_youngDog)).Return(new List<string>());
            _mockery.ReplayAll();

            var actual = Validator.This(_youngDog).HasMessages();

            Assert.That(actual, Is.EqualTo(expected));

            _mockery.VerifyAll();
        }
    }
}
