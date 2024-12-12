using EventManagement.Core.ResponseDTO;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.Implementations;
using System.ComponentModel;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace EventManagement.API.Service
{
    public static class CSVHelper
    {
        //public async static Task<List<Rsvp>> ProcessCSVFile(IFormFile CSVFile, EventResponseDTO createdEvent) {
        //    List<Rsvp> rsvps = new List<Rsvp>();
        //    using (var reader = new StreamReader(CSVFile.OpenReadStream()))
        //    {
        //        string line;
        //        while ((line = await reader.ReadLineAsync()) != null)
        //        {
        //            line = line.Trim();
        //            if (!string.IsNullOrEmpty(line))
        //            {
        //                var parts = line.Split(',');
        //                if (parts.Length >= 2)
        //                {
        //                    var rsvp = new Rsvp
        //                    {
        //                        EventId = createdEvent.EventId,
        //                        Name = parts[0].Trim(),
        //                        Email = parts[1].Trim()
        //                    };
        //                    rsvps.Add(rsvp);
        //                }
        //            }                  
        //        }
        //    }
        //    return rsvps;
        //}

        public async static Task<List<Rsvp>> ProcessCSVFile(IFormFile CSVFile, EventResponseDTO createdEvent)
        {
            List<Rsvp> rsvps = new List<Rsvp>();
            using (var reader = new StreamReader(CSVFile.OpenReadStream()))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    
                }))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var rsvp = new Rsvp()
                        {
                            Name = csv.GetField<string>("Name"),
                            Email = csv.GetField<string>("Email"),
                            EventId = createdEvent.EventId,
                            Status = "Pending"
                        };
                        rsvps.Add(rsvp);
                    }
                }
            }
            return rsvps;
        }
    }
}
