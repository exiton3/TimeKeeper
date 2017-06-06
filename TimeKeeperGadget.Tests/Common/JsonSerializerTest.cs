namespace TimeKeeperGadget.Tests.Common
{
    using System;
    using Framework.Common;
    using Framework.Dtos;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class JsonSerializerTest
    {
        //string json =// @"[{"id":"jidMQ==","name":"yvpUHJvamVjdCAx"},{"id":"d49Mg==","name":"ss8UHJvamVjdCAy"},{"id":"8a0Mw==","name":"hyqUHJvamVjdCAz"},{"id":"mt0NA==","name":"11fUHJvamVjdCA0"}]";
        [Test]
        public void CanSerializeListOfDtosToJsonString()
        {
            //Arange
            var jsonSerializer = new JsonSerializer();
            var list = new List<MessageDto>
                                        {
                                            new MessageDto{AcId = "123",PrId = "1234",End = 12345,Start = 6345},
                                            new MessageDto{AcId = "123",PrId = "1234",End = 12345,Start = 6345}
                                        };
            //Act
            string jsonString = jsonSerializer.Serialize(list);
            //Assert
            Assert.That(jsonString, Is.Not.Null);
            Console.WriteLine(jsonString);

        }

        [Test]
        public void CanDeserializeJsonToProjects()
        {
            //Arange
            var jsonSerializer = new JsonSerializer();
            //Act
            string inputData ="";
           object obj = jsonSerializer.Deserialize(inputData);
            //Assert

        }
    }
}