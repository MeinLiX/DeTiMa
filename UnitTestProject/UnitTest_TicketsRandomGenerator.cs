using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace UnitTestProject
{
    public class UnitTest_TicketsRandomGenerator
    {
        private DeTiMa.Models.AirDB.AirTicket _randomTicket;

        private static readonly Regex regex = new(@"[a-zA-Z_ ]+\([A-Z]{3,4}\)");

        [SetUp]
        public void Setup()
        {
            _randomTicket = DeTiMa.Utils.TicketsRandomGenerator.GetRandomAirTicket();
        }

        [Test]
        public void Test_TimeOfFly()
        {
            DateTime dateOfDeparture = _randomTicket.Date; //Date equal to Departure Time

            Assert.Less(dateOfDeparture.Ticks, dateOfDeparture.Add(_randomTicket.ArrivalTime).Ticks); 
        }

        [Test]
        public void Test_AiroportofArrivalNameValidator()
        {
            Assert.IsTrue(regex.IsMatch(_randomTicket.AirportOfArrival));
        }

        [Test]
        public void Test_AiroportofDepartureNameValidator()
        {
            Assert.IsTrue(regex.IsMatch(_randomTicket.AirportOfDeparture));
        }
    }
}