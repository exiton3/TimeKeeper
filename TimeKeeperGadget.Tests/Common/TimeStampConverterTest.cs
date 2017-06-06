namespace TimeKeeperGadget.Tests.Common
{
    using System;
    using Framework.Common;
    using NUnit.Framework;

    [TestFixture]
    public class TimeStampConverterTest
    {
        [Test]
        public void OrginalUTimeStampShoudBeZero()
        {
            //Arange
            DateTime org = new DateTime(1970,1,1,0,0,0);
            //Act
            double timeStamp = TimeStampConverter.ConvertToUnixTimestamp(org);
            //Assert
            Assert.That(timeStamp, Is.EqualTo(0));
        }
    }
}