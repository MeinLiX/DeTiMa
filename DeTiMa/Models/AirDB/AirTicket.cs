using System;
using System.ComponentModel.DataAnnotations;

namespace DeTiMa.Models.AirDB
{
    public class AirTicket
    {
        public AirTicket()
        {

        }

        public AirTicket(int code, string company, string airportOfDeparture, TimeSpan departureTime, string airportOfArrival, TimeSpan arrivalTime, DateTime date)
        {
            Code = code;
            Company = company ?? throw new ArgumentNullException(nameof(company));
            AirportOfDeparture = airportOfDeparture ?? throw new ArgumentNullException(nameof(airportOfDeparture));
            DepartureTime = departureTime;
            AirportOfArrival = airportOfArrival ?? throw new ArgumentNullException(nameof(airportOfArrival));
            ArrivalTime = arrivalTime;
            Date = date;
        }

        [Key]
        [Required]
        public int Code { get; set; }

        [Required]
        [MaxLength(64, ErrorMessage = "The maximum length of the company field is 64")]
        [MinLength(1, ErrorMessage = "The minimum length of the company field is 1")]
        public string Company { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z_ ]+\([A-Z]{3,4}\)", ErrorMessage = "Invalid airoport!")]
        public string AirportOfDeparture { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z_ ]+\([A-Z]{3,4}\)", ErrorMessage = "Invalid airoport!")]
        public string AirportOfArrival { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

    }
}
