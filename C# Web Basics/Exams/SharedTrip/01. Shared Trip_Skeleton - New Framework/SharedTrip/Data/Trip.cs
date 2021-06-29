using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Data
{
    public class Trip
    {
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string EndPoint { get; set; }
        public DateTime DepartureTime { get; set; }
        public int Seats { get; set; }

        [Required]
        [MaxLength(80)]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ICollection<UserTrip> UserTrips { get; set; } = new HashSet<UserTrip>();

    }
}
