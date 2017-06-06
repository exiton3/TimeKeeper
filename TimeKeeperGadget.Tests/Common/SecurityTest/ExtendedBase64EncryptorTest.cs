namespace TimeKeeperGadget.Tests.Common.SecurityTest
{
    using System;
    using Framework.Common.Security;
    using NUnit.Framework;

    [TestFixture]
    public class ExtendedBase64EncryptorTest
    {
        const string StringToEncrypt = "amdar";
        const string TestResult = "YW1kYXI=";
        private Base64EncryptorEx base64Encryptor;

        [SetUp]
        public void SetUp()
        {
            base64Encryptor = new Base64EncryptorEx();
        }
        [Test]
        public void EncryptString()
        {
            //Arange
            //Act
            var encodedString = base64Encryptor.Encode(StringToEncrypt);
            //Assert
            Assert.IsNotNull(encodedString);
            Assert.That(encodedString.Substring(3), Is.EqualTo(TestResult));
            Console.WriteLine(encodedString);
        }

        [Test]
        public void DecryptString()
        {
            //Arange
            //Act
            string decodedstring = base64Encryptor.Decode("asd" +TestResult);

            //Assert
            Assert.IsNotNull(decodedstring);
            Console.WriteLine(decodedstring);
            Assert.That(decodedstring, Is.EqualTo(StringToEncrypt));
        }
    }
}