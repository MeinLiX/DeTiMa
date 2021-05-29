using System;
using System.Collections.Generic;
using DeTiMa.Models.AirDB;

namespace DeTiMa.Utils
{
    public class TicketsRandomGenerator
    {
        private static readonly List<string> AirLineCompaniesList = new()
        {
            "Bravo Airways",
            "Motor Sich Airlines",
            "SkyUp",
            "Ukraine International Airlines",
            "Yanair",
            "Austrian Airlines",
            "easyJet Europe",
            "Eurowings Europe",
            "People's",
            "BA CityFlyer",
            "British Airways",
            "Eastern Airways",
            "EasyJet UK",
            "Jet2",
            "Loganair",
            "Ryanair UK",
            "Virgin Atlantic",
            "Virgin Atlantic International",
            "Wizz Air UK",
            "Anadolujet",
            "Corendon Airlines",
            "Onur Air",
            "SunExpress",
            "Turkish Airlines",
            "Norwegian Air Norway",
            "Norwegian Air Shuttle",
            "Scandinavian Airlines",
            "Widerøe"
        };

        private static readonly List<string> AirportsList = new()
        {
            "Kyiv (KBP)",
            "Kyiv (IEV)",
            "Lviv (LWO)",
            "Ålesund Airport, Vigra (AES)",
            "Alta Airport (ALF)",
            "Aalborg (AAL)",
            "Aarhus (AAR)",
            "Luton (LTN)",
            "Stansted Mountfitchet (STN)",
            "Hillingdon (LHR)",
            "Ringway (MAN)",
            "Crawley (LGW)",
            "Aachen (AAH)",
            "Augsburg (AGB)",
            "Berlin (BER)",
            "Cologne (CGN)"
        };

        public static AirTicket GetRandomAirTicket()
        {
            Random rand = new();
            int Code = rand.Next();
            string Company = AirLineCompaniesList[rand.Next(0, AirLineCompaniesList.Count)];

            int depIdx = rand.Next(0, AirportsList.Count);
            int arrIdx = depIdx;
            while (arrIdx == depIdx)
                arrIdx = rand.Next(0, AirportsList.Count);

            string AirportOfDeparture = AirportsList[depIdx];
            TimeSpan DepartureTime = DateTime.Today.AddMinutes(rand.Next(800)).TimeOfDay;

            string AirportOfArrival = AirportsList[arrIdx];
            TimeSpan ArrivalTime = DepartureTime.Add(TimeSpan.FromMinutes(rand.Next(120, 400)));

            DateTime Date = DateTime.Today.Add(DepartureTime);
            return new(Code, Company, AirportOfDeparture, DepartureTime, AirportOfArrival, ArrivalTime, Date);
        }
    }
}
