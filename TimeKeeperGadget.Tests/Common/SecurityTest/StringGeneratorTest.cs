namespace TimeKeeperGadget.Tests.Common.SecurityTest
{
    using System;
    using System.Diagnostics;
    using Framework.Common.Security;
    using NUnit.Framework;

    //Test list
    //1. Generate random string with predefined length
    //2. Two generated string shoud be not equal
    [TestFixture]
    public class StringGeneratorTest
    {
        RandomStringGenerator randomStringGenerator = new RandomStringGenerator();

        [Test]
        public void CanGenerateRandomStringWithPredefinedLength()
        {
            //Arange
           
            //Act
            int length = 10;
            string randomString = randomStringGenerator.Generate(length);

            //Assert
            Assert.That(randomString, Is.Not.Null);
            Assert.That(randomString.Length, Is.EqualTo(length));
            Console.WriteLine(randomString);
        }

        [Test]
        public void TwoGeneretedStringShoudBeNotEqual()
        {
            //Arange
         
            int length = 10;
            //Act
            string randomString1 = randomStringGenerator.Generate(length);
            string randomString2 = randomStringGenerator.Generate(length);

            //Assert
            Console.WriteLine(randomString1);
            Console.WriteLine(randomString2);

            Assert.That(randomString1, Is.Not.EqualTo(randomString2));

        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IfLengthParameterThrowException()
        {
            //Arange

            //Act
            randomStringGenerator.Generate(-10);
            //Assert

        }

        [Test]
        public void PerformanceTest()
        {
            //Arange
            Stopwatch stopwatch = new Stopwatch();
            //Act
            stopwatch.Start();
            for (int i = 0; i < 10; i++)
            {
                randomStringGenerator.Generate(10);
            }
            stopwatch.Stop();
            //Assert
            Console.WriteLine(@"Time elapsed {0}",stopwatch.ElapsedMilliseconds);
        }


    }
}