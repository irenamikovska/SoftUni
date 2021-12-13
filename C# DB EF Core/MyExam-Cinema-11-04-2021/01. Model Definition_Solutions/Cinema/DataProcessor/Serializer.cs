namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                 .Where(x => x.Rating >= rating && x.Projections.Any(t => t.Tickets.Count >= 1))
                 .ToArray()
                 .OrderByDescending(x => x.Rating)
                 .ThenByDescending(x => x.Projections.Sum(s => s.Tickets.Sum(p => p.Price)))
                 .Select(x => new
                 {
                     MovieName = x.Title,
                     Rating = x.Rating.ToString("f2"),
                     TotalIncomes = x.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                     Customers = x.Projections.SelectMany(p => p.Tickets
                          .Select(t => new
                          {
                               FirstName = t.Customer.FirstName,
                               LastName = t.Customer.LastName,
                               Balance = t.Customer.Balance.ToString("f2")
                          })
                         .ToArray())
                         .OrderByDescending(a => a.Balance)
                         .ThenBy(a => a.FirstName)
                         .ThenBy(a => a.LastName)
                         .ToArray()
                 })
                 .Take(10)
                 .ToArray();

            string json = JsonConvert.SerializeObject(movies, Formatting.Indented);
            return json;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
             var customers = context.Customers
                .ToArray()
                .Where(x => x.Age >= age)
                .OrderByDescending(x => x.Tickets.Sum(t => t.Price))
                .Select(c => new ExportCustomerDto()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = c.Tickets.Sum(t => t.Price).ToString("f2"),
                    SpentTime = TimeSpan.FromSeconds(c.Tickets.Sum(t => t.Projection.Movie.Duration.TotalSeconds)).ToString(@"hh\:mm\:ss")
                })
                .Take(10)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCustomerDto[]), new XmlRootAttribute("Customers"));
            var textWriter = new StringWriter();
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(textWriter, customers, ns);
            var result = textWriter.ToString().TrimEnd();
            return result;

        }
    }
}