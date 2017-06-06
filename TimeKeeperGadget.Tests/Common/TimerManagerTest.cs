namespace TimeKeeperGadget.Tests.Common
{
    using System;
    using NUnit.Framework;
    using TimeKeeper.Model;

    //Test list 
    //1.[+] When timer shoud be raised Event
    //2.[+] Timer shoud stop on default Time
    //3.[+] Can start timer
    //4.[+] Timer shoud be independed on system time, 
    [TestFixture]
    public class TimerManagerTest
    {
        private TimerManager timerManager;
        [SetUp]
        public void SetUp()
        {
            timerManager = new TimerManager();
        }

        [Test]
        public void CanStartTimer()
        {
            //Arange

            //Act
            timerManager.StartTimer();
            //Assert
            Assert.IsFalse(timerManager.IsTimerStopped);
        }
        [Test]
        public void WhenTimerStoppedRaisedEvent()
        {
            //Arange
            bool isEventRaised = false;
            timerManager.Stopped += delegate
                                        {
                                            isEventRaised = true;
                                        };
            //Act
            timerManager.StopTimer();
            //Assert
            Assert.IsTrue(isEventRaised);
            Assert.IsTrue(timerManager.IsTimerStopped);
        }

        [Test]
        public void TimerShoudStopOnDefaultTime()
        {
            //Arange
            bool isEventRaised = false;
            timerManager.DefaultStopTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute - 1, 0);
            timerManager.Stopped += delegate
                                        {
                                            isEventRaised = true;
                                        };

            //Act
            timerManager.StartTimer();

            timerManager.DispatcherTimerTick(this, new EventArgs());

            //Assert
            Assert.IsTrue(isEventRaised);

        }

        [Test]
        public void TimerShoudBeIndependentOnSystemTime()
        {
            //Arange

            //Act
            timerManager.StartTimer();
            for (int i = 0; i < 10; i++)
            {
                timerManager.DispatcherTimerTick(this, new EventArgs());
            }
            timerManager.StopTimer();
            //Assert
            Assert.That(timerManager.ElapsedTime.Seconds,Is.EqualTo(10));
        }
    }
}