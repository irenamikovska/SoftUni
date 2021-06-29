using SharedTrip.Models.Trips;
using SharedTrip.Models.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedTrip.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel input)
        {
            var errors = new List<string>();

            if (input.Username == null || input.Username.Length < 5 || input.Username.Length > 20)
            {
                errors.Add("Username is not valid. It must be between 5 and 20 characters long.");
            }

            if (input.Email == null || !Regex.IsMatch(input.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                errors.Add("Email is not a valid e-mail address.");
            }

            if (input.Password == null || input.Password.Length < 6 || input.Password.Length > 20)
            {
                errors.Add("The password is not valid. It must be between 6 and 20 characters long.");
            }                     

            if (input.Password != input.ConfirmPassword)
            {
                errors.Add("Password and its confirmation are different.");
            }
                     
            return errors;
        }
        public ICollection<string> ValidateTrip(AddTripFormModel input)
        {
            var errors = new List<string>();

            if (input.StartPoint == null)
            {
                errors.Add("Startpoint is required.");
            }

            if (input.EndPoint == null)
            {
                errors.Add("Endpoint is required.");
            }

            if (input.Seats < 2 || input.Seats > 6)
            {
                errors.Add("Seats is not valid. It must be between 2 and 6.");
            }

            if (input.Description == null || input.Description.Length > 80)
            {
                errors.Add("Description is not valid. It must be maximum 80 characters long.");
            }

            if (!DateTime.TryParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                errors.Add("Invalid departure time. Please use dd.MM.yyyy HH:mm format.");
            }
          
                   
            return errors;
        }

    }
}
