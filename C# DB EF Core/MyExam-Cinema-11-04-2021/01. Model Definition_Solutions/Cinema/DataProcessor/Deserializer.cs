namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";

        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";

        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var moviesDto = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);

            var movies = new List<Movie>();

            foreach (var mDto in moviesDto)
            {
                if (!IsValid(mDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre;
                bool isValidGenre = Enum.TryParse<Genre>(mDto.Genre, out genre);

                if (!isValidGenre)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isTimeValid = TimeSpan.TryParseExact(mDto.Duration, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None, out TimeSpan duration);

                if (!isTimeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = movies.FirstOrDefault(x => x.Title == mDto.Title);

                if (movie != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var mToAdd = new Movie
                {
                    Title = mDto.Title,
                    Genre = genre,
                    Duration = duration,
                    Rating = mDto.Rating,
                    Director = mDto.Director
                };

                movies.Add(mToAdd);
                sb.AppendLine(String.Format(SuccessfulImportMovie, mToAdd.Title, mToAdd.Genre, mToAdd.Rating.ToString("f2")));                
                               
            }
            context.Movies.AddRange(movies);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));
            var textRead = new StringReader(xmlString);
            var projectionsDto = serializer.Deserialize(textRead) as ImportProjectionDto[];

            var projections = new List<Projection>();

            foreach (var pDto in projectionsDto)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = context.Movies.FirstOrDefault(x => x.Id == pDto.MovieId);
                if (movie == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDateValid = DateTime.TryParseExact(pDto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var pToAdd = new Projection
                {
                    MovieId = movie.Id,                    
                    DateTime = date                   
                };

                projections.Add(pToAdd);
                sb.AppendLine(string.Format(SuccessfulImportProjection, movie.Title, pToAdd.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

            }
            context.Projections.AddRange(projections);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            var textRead = new StringReader(xmlString);
            var customersDto = serializer.Deserialize(textRead) as ImportCustomerDto[];

            var customers = new List<Customer>();

            foreach (var cDto in customersDto)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var cToAdd = new Customer 
                {
                    FirstName = cDto.FirstName,
                    LastName = cDto.LastName,
                    Age = cDto.Age,
                    Balance = cDto.Balance
                };

                foreach (var tDto in cDto.Tickets)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var projection = context.Projections.FirstOrDefault(x => x.Id == tDto.ProjectionId);
                    if (projection == null)
                    {
                        continue;
                    }

                    var ticket = new Ticket
                    {
                        Projection = projection,
                        Price = tDto.Price,
                        Customer = cToAdd                                               
                    };

                    cToAdd.Tickets.Add(ticket);                   
                }

                customers.Add(cToAdd);
                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, cToAdd.FirstName, cToAdd.LastName, cToAdd.Tickets.Count));
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

    }
}