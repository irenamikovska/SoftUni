namespace CarShop.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using CarShop.Models.Cars;
    using CarShop.Models.Issues;
    using CarShop.Models.Users;

    using static Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel input)
        {
            var errors = new List<string>();

            if (input.Username == null || input.Username.Length < 4 || input.Username.Length > 20)
            {
                errors.Add("Username is not valid. It must be between 4 and 20 characters long.");
            }

            if (input.Email == null || !Regex.IsMatch(input.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                errors.Add("Email is not a valid e-mail address.");
            }

            if (input.Password == null || input.Password.Length < 5 || input.Password.Length > 20)
            {
                errors.Add("The password is not valid. It must be between 5 and 20 characters long.");
            }

            if (input.Password != null && input.Password.Any(x => x == ' '))
            {
                errors.Add("The password cannot contain whitespaces.");
            }

            if (input.Password != input.ConfirmPassword)
            {
                errors.Add("Password and its confirmation are different.");
            }

            if (input.UserType != "Client" && input.UserType != "Mechanic")
            {
                errors.Add("The user can be either a client or a mechanic.");
            }

            return errors;
        }

        public ICollection<string> ValidateCar(AddCarFormModel input)
        {
            var errors = new List<string>();

            if (input.Model == null || input.Model.Length < 5 || input.Model.Length > 20)
            {
                errors.Add("Model is not valid. It must be between 5 and 20 characters long.");
            }

            if (input.Year < 1900 || input.Year > 2100)
            {
                errors.Add("Year is not valid. It must be between 1900 and 2100.");
            }

            if (input.Image == null || !Uri.IsWellFormedUriString(input.Image, UriKind.Absolute))
            {
                errors.Add("Image url is not valid. It must be a valid URL.");
            }

            if (input.PlateNumber == null || !Regex.IsMatch(input.PlateNumber, @"[A-Z]{2}[0-9]{4}[A-Z]{2}"))
            {
                errors.Add("Plate number is not valid. It should be in 'XX0000XX' format.");
            }

            return errors;
        }

        public ICollection<string> ValidateIssue(AddIssueFormModel input)
        {
            var errors = new List<string>();

            if (input.CarId == null)
            {
                errors.Add("Car ID cannot be empty.");
            }

            if (input.Description == null || input.Description.Length < 5)
            {
                errors.Add("Description is not valid. It must have more than 5 characters.");
            }

            return errors;
        }
    }
}
