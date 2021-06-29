using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Models.Trips;
using SharedTrip.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;
        public TripsController(ApplicationDbContext data, IValidator validator)
        {
            this.data = data;
            this.validator = validator;
    }

        [Authorize]
        public HttpResponse Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddTripFormModel model)
        {
            var modelErrors = this.validator.ValidateTrip(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var trip = new Trip
            {                
                DepartureTime = DateTime.ParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Description = model.Description,
                EndPoint = model.EndPoint,
                ImagePath = model.ImagePath,
                Seats = model.Seats,
                StartPoint = model.StartPoint,
            };

            this.data.Trips.Add(trip);

            this.data.SaveChanges();

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public HttpResponse All()
        {
                       
            var trips = this.data.Trips.Select(x => new AllTripsViewModel
            {
                DepartureTime = x.DepartureTime,
                EndPoint = x.EndPoint,
                StartPoint = x.StartPoint,
                Id = x.Id,
                Seats = x.Seats,
                UsedSeats = x.UserTrips.Count(),
            }).ToList();

            return this.View(trips);
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var trip = this.data.Trips.Where(x => x.Id == tripId)
                .Select(x => new DetailsTripViewModel
                {
                    DepartureTime = x.DepartureTime,
                    Description = x.Description,
                    EndPoint = x.EndPoint,
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    Seats = x.Seats,
                    StartPoint = x.StartPoint,
                    UsedSeats = x.UserTrips.Count(),
                })
                .FirstOrDefault();
           
            return this.View(trip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {

            if (!this.HasAvailableSeats(tripId))
            {
                return this.Error("No seats available.");
            }

            var userWithTrip = this.data.UserTrips.Any(x => x.UserId == this.User.Id && x.TripId == tripId);
            
            if (userWithTrip)
            {
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            var userTrip = new UserTrip
            {
                TripId = tripId,
                UserId = this.User.Id,
            };

            this.data.UserTrips.Add(userTrip);
            this.data.SaveChanges();          
                        
            return this.Redirect("/Trips/All");

        }
        public bool HasAvailableSeats(string tripId)
        {
            var trip = this.data.Trips.Where(x => x.Id == tripId)
                .Select(x => new { x.Seats, TakenSeats = x.UserTrips.Count() })
                .FirstOrDefault();
            
            var availableSeats = trip.Seats - trip.TakenSeats;
            
            return availableSeats > 0;
        }


    }
}
