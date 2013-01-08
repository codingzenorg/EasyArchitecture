﻿using EasyArchitecture.IoC.Plugin.BultIn;
using EasyArchitecture.IoC.Plugin.Contracts;
using EasyArchitecture.Tests.Stuff.DI;
using NUnit.Framework;
using System.Collections.Generic;

namespace EasyArchitecture.Tests.Plugins
{
    [TestFixture]
    public class ContainerTest
    {
        private IContainer _container;


        [SetUp]
        public void SetUp()
        {

            _container = new ContainerPlugin().GetInstance();
        
        }

        [Test]
        public void Should_register_interfaces()
        {
            _container.Register<IDummyInterface, DummyImplementation>();

            var actual = _container.Resolve<IDummyInterface>();

            Assert.That(actual, Is.TypeOf<DummyImplementation>());
        }

        [Test]
        public void Should_override_register()
        {
            _container.Register<IDummyInterface, DummyImplementation>();
            _container.Register<IDummyInterface, NewDummyImplementation>();

            var actual = _container.Resolve<IDummyInterface>();

            Assert.That(actual, Is.TypeOf<NewDummyImplementation>());
        }

        [Test]
        public void Should_resolve_interfaces()
        {
            _container.Register<IDummyInterface, DummyImplementation>();

            var implementation = _container.Resolve<IDummyInterface>();

            Assert.That(implementation, Is.AssignableTo<IDummyInterface>());

            var methodReturn = implementation.DummyMethod();
            const string expectedMethodReturn = "DummyMethod";

            Assert.That(methodReturn, Is.EqualTo(expectedMethodReturn));
        }

        [Test]
        public void Should_not_register_class_that_dont_implement_the_provided_interface()
        {
            ////Garantido pela constraint de U:T
            //Assert.That(
            //    () => _plugin.Register<IDummyInterface, DummyStrangeImplementation>(),
            //    Throws.InstanceOf<Exception>()
            //);
            Assert.Pass();
        }

        [Test]
        public void Should_resolve_dependencies()
        {
            _container.Register<IDummyInterface, DependencyImplementation>();
            _container.Register<IDummyRequiredObjectInterface, DummyRequiredObjectImplementation>();

            var implementation = _container.Resolve<IDummyInterface>();

            Assert.That(implementation, Is.AssignableTo<IDummyInterface>());

            var methodReturn = implementation.DummyMethod();
            const string expectedMethodReturn = "RequiredMethod";

            Assert.That(methodReturn, Is.EqualTo(expectedMethodReturn));
        }

        [Test]
        public void Should_resolve_interfaces_with_proxy()
        {
            _container.Register<IDummyInterface, DummyImplementation>();

            var implementation = _container.Resolve<IDummyInterface>();

            Assert.That(implementation, Is.AssignableTo<IDummyInterface>());

            var methodReturn = implementation.DummyMethod();
            const string expectedMethodReturn = "DummyMethod";

            Assert.That(methodReturn, Is.EqualTo(expectedMethodReturn));
        }

        [Test]
        public void Should_be_possible_call_void_facade_methods()
        {
            _container.Register<IDummyInterface, DummyImplementation>();

            var implementation = _container.Resolve<IDummyInterface>();
            const string voidmethodmessage = "VoidDummyMethodExecuted";


            implementation.VoidDummyMethod();
            Assert.That(implementation.Message(), Is.EqualTo(voidmethodmessage));

        }

        [Test]
        public void Should_be_possible_call_facade_methods_containing_primitive_type_arg_and_return()
        {
            _container.Register<IDummyInterface, DummyImplementation>();
            var implementation = _container.Resolve<IDummyInterface>();

            const int expectedInt = 1;
            Assert.That(implementation.PrimitiveWithOneArg(1), Is.EqualTo(expectedInt));
        

        }

        [Test]
        public void Should_be_possible_call_facade_methods_containing_two_primitive_type_args()
        {
            _container.Register<IDummyInterface, DummyImplementation>();
            var implementation = _container.Resolve<IDummyInterface>();

            string expectedStr = "message1+message2";
            Assert.That(implementation.withArgs("message1+", "message2"), Is.EqualTo(expectedStr));
            

        }

        [Test]
        public void Should_be_possible_call_facade_methods_containing_generic_type_return_and_user_defined_class_arg()
        {
            _container.Register<IDummyInterface, DummyImplementation>();
            var implementation = _container.Resolve<IDummyInterface>();

            var kvp = new KeyValuePair<int, DummyClass>(1, new DummyClass());
            Assert.That(implementation.WithTypedInterfaceArg(kvp), Is.EqualTo(kvp.Value.GetType()));

        }

        [Test]
        public void Should_be_possible_call_facade_methods_containing_enum_arg_and_return()
        {
            _container.Register<IDummyInterface, DummyImplementation>();
            var implementation = _container.Resolve<IDummyInterface>();

            Assert.That(implementation.EnumWithEnumArg(DummyEnum.Second), Is.EqualTo(DummyEnum.Second));
          

        }


        [Test]
        public void Should_be_possible_call_facade_methods_containing_array_arg_and_return()
        {
            _container.Register<IDummyInterface, DummyImplementation>();
            var implementation = _container.Resolve<IDummyInterface>();

            string[] expectedArr = { "message1", "message2" };
            string[] strArr = { "message1", "message2" };

            var returnedStrArr = implementation.ArrayWithArrArg(strArr);
            Assert.That(returnedStrArr, Is.EqualTo(expectedArr));
        }


        [Test]
        public void Should_be_possible_call_facade_methods_containing_generic_return_with_user_defined_type()
        {

            _container.Register<IDummyInterface, DummyImplementation>();
            var implementation = _container.Resolve<IDummyInterface>();

            IList<DummyClass> expectedLst = new List<DummyClass>();
            expectedLst.Add(new DummyClass());

            var actualLst = implementation.TypedInterfaceWithoutArgs();

            Assert.That(actualLst, Is.EqualTo(expectedLst));

        }
        
   
    }

}
