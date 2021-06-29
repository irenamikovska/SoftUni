using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Models.Trips
{
    public class AllTripsViewModel
    {
        public string Id { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DepartureTimeAsString => this.DepartureTime.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
        public int AvailableSeats => this.Seats - this.UsedSeats;
        public int Seats { get; set; }
        public int UsedSeats { get; set; }

    }
}
