namespace TimeKeeperGadget.Tests.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Framework.Common;
    using Framework.Common.Security;
    using Framework.Model;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class MapToObjectsTest
    {
        private Mock<IEncryptor> encryptor;
        Dictionary<string, string> dictionary;
        private ObjectMapper<Project> objectMapper;

        [SetUp]
        public void SetUp()
        {
            encryptor = new Mock<IEncryptor>();
            encryptor.Setup(x => x.Decode("test")).Returns("1");
            objectMapper = new ObjectMapper<Project>(encryptor.Object);

            dictionary = new Dictionary<string, string> { { "id", "test" }, { "name", "test" } };
        }
        [Test]
        public void CanMapOneEntity()
        {

            //Act
            Entity entity = objectMapper.Map(dictionary);

            //Assert
            Assert.IsNotNull(entity);
            Assert.That(entity.Id, Is.EqualTo(1));
            Assert.That(entity.Name, Is.EqualTo("1"));
        }

        [Test]
        public void CanMapCollectionOfObjects()
        {
            //Arange
            IList<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            list.Add(dictionary);
            //Act
            var entities = objectMapper.MapCollection(list).ToList();

            //Assert
            Assert.That(entities.Count, Is.EqualTo(1));
        }
    }
}