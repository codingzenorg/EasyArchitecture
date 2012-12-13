﻿using Application4Test.Domain;
using EasyArchitecture.Configuration;
using EasyArchitecture.Runtime;
using EasyArchitecture.Validation;
using EasyArchitecture.Validation.Plugin.Contracts;
using NUnit.Framework;

namespace EasyArchitecture.Tests.Mechanisms
{
    [TestFixture]
    public class ValidatorTest
    {
        private Dog _oldDog;
        private Dog _youngDog;

        [SetUp]
        public void SetUp()
        {
            Configure
                .For("Application4Test")
                    .Done();

            LocalThreadStorage.SetCurrentModuleName("Application4Test");


            _oldDog = new Dog { Age = 15, Name = "Old Dog" };
            _youngDog = new Dog { Age = 5, Name = "Young Dog" };
        }

        [Test]
        public void Should_throws_invalid_entity_exception_to_invalid_entity()
        {
            Assert.That(() => Validator.This(_oldDog).IsValid(), Throws.TypeOf<InvalidEntityException>());
        }

        [Test]
        public void Can_pass_valid_entity()
        {
            Assert.That(() => Validator.This(_youngDog).IsValid(), Throws.Nothing);
        }

        [Test]
        public void Should_return_validation_messages_to_invalid_entity()
        {
            var expected = new System.Collections.Generic.List<string>() { "There's no dog so old" };
            var actual = Validator.This(_oldDog).HasMessages();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Should_not_return_messages_valid_entity()
        {
            var expected = new System.Collections.Generic.List<string>();
            var actual = Validator.This(_youngDog).HasMessages();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
