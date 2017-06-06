namespace TimeKeeperGadget.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using Framework.Common;
    using Framework.Common.Security;
    using Framework.Model;
    using Moq;
    using NUnit.Framework;
    //Test list
    
    //1. [+] an object of Queue shoud be map to DTO object
    //2. [+] Can map list of objects to list DTOs
    [TestFixture]
    public class MapperTest
    {
        private Mock<IEncryptor> encryptor;
        private MessageMapper messageMapper;

        [TestFixtureSetUp]
        public void SetUp()
        {
            encryptor = new Mock<IEncryptor>();
            messageMapper = new MessageMapper(encryptor.Object);
        }

        [Test]
        public void CanMapActiveMessageToDtoObject()
        {
            //Arange
            var activityMessage = new ActivityMessage();
            //Act
            var dtoObject = messageMapper.Map(activityMessage);

            //Assert
            Assert.That(dtoObject, Is.Not.Null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IfObjectToMapNullShoudThrowException()
        {
            //Arange

            //Act
             messageMapper.Map(null);
            //Assert

        }

        [Test]
        public void CanMapCollectionOfBjects()
        {
            //Arange
            var list = new List<ActivityMessage>();
            //Act
            var dtoArray = messageMapper.MapCollection(list);
            
            //Assert
            Assert.That(dtoArray, Is.Not.Null);

        }
    }
}