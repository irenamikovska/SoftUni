using Andreys.ViewModels.Products;
using Andreys.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Andreys.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < 4 || model.Username.Length > 10)
            {
                errors.Add($"Username is not valid. It must be between 4 and 10 characters long.");
            }


            if (model.Password.Length < 6 || model.Password.Length > 20)
            {
                errors.Add($"The provided password is not valid. It must be between 6 and 20 characters long.");
            }

            if (model.Password.Any(x => x == ' '))
            {
                errors.Add($"The provided password cannot contain whitespaces.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and its confirmation are different.");
            }

            return errors;
        }

        public ICollection<string> ValidateProduct(AddProductFormModel model)
        {
            var errors = new List<string>();

            if (model.Name.Length < 4 || model.Name.Length > 20)
            {
                errors.Add("Name must be between 4 and 20 characters long.");
            }


            if (model.Description.Length > 10)
            {
                errors.Add("The description must be max 10 characters long.");
            }
            
            if (string.IsNullOrEmpty(model.Category))
            {
                errors.Add($"Category is required.");
            }

            if (model.Category != "Shirt" && model.Category != "Denim" && model.Category != "Shorts" && model.Category != "Jacket")
            {
                errors.Add($"Category is invalid.");
            }

            if (model.Gender != "Male" && model.Gender != "Female")
            {
                errors.Add($"Gender is invalid.");
            }

            if (string.IsNullOrEmpty(model.Gender))
            {
                errors.Add($"Gender is required.");
            }

            return errors;
        }
               
    }
}
