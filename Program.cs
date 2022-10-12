using System;

using System.Collections.Generic;
using System.IO;
using System.Net.Http;

using System.Threading.Tasks;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using System.Threading;

namespace MathProject_Capstone_
{
    class Program
    {
        
        
        static async Task Main(string[] args)
        {
            List<string> fileNames= new List<string>();
            //fileNames.Add("/Users/cristian/MTH4990/MathProject-Capstone-/flightNeeded2.txt");
            //fileNames.Add("/Users/cristian/MTH4990/MathProject-Capstone-/flightsNeeded.txt");
            //fileNames.Add("/Users/cristian/MTH4990/MathProject-Capstone-/flightsNeeded3.txt");
           // fileNames.Add("/Users/cristian/MTH4990/MathProject-Capstone-/flightsNeeded4.txt");
            fileNames.Add("/Users/cristian/MTH4990/MathProject-Capstone-/flightsNeeded5.txt");
            fileNames.Add("/Users/cristian/MTH4990/MathProject-Capstone-/flightsNeeded6.txt");
            for (int i = 0; i < fileNames.Count; i++)
            {
                List<Itineraries> listOfFights= new List<Itineraries>();
                ApiKeys apiKeys= new ApiKeys();
                string fileName=fileNames[i];
                FileIO fileIO= new FileIO(fileName);
                string[] data=fileIO.readDataInToStringList();
                List<FlightInformation> flightInformation=storeTheData(data);
                List<string> csvFlights=new List<string>();
                await makeApiCallsAsync(flightInformation,apiKeys,listOfFights);
                appendNewFlights(csvFlights,listOfFights);
                writeOutToCSV(csvFlights);
                System.Console.WriteLine("Done with file:{0}",i+1);
                Thread.Sleep(60000);
                
            }
        
        
        }

        // private static async Task makeApiCallsAsync2(List<FlightInformation> flightInformation, ApiKeys apiKeys, List<Itineraries> listOfFights)
        // {
        //     foreach (FlightInformation flight in flightInformation)
        //     {
                
        //         string url=String.Format("https://skyscanner44.p.rapidapi.com/search-extended?adults=1&origin={0}&destination={1}&departureDate={2}&currency=USD",flight.arrivalCity,flight.departureCity,flight.date);
                
        //         var client = new HttpClient();
        //         var request = new HttpRequestMessage
        //     {
	    //         Method = HttpMethod.Get,
	    //         RequestUri = new Uri(url),
	    //         Headers =
	    //         {
		//             { "X-RapidAPI-Key", apiKeys.key.ToString() },
		//             { "X-RapidAPI-Host", apiKeys.Host.ToString() },
	    //         },
        //     };
        //     using (var response = await client.SendAsync(request))
        //     {
	    //         response.EnsureSuccessStatusCode();
	    //         string body =  response.Content.ReadAsStringAsync().Result;
        //         TravvelData travInfo = JsonConvert.DeserializeObject<TravvelData>(body.ToString()); 
        //         //System.Console.WriteLine("hi:{0}",travInfo); 
        //         listOfFights.Add(travInfo.itineraries);
        //        //System.Console.WriteLine("Number of results:{0}", travInfo.itineraries.results);
        //         //System.Console.WriteLine(body);
            
        //     }
        //     }
        // }

        private static async Task makeApiCallsAsync(List<FlightInformation> flightInformation, ApiKeys apiKeys, List<Itineraries> listOfFights)
        {
            foreach (FlightInformation flight in flightInformation)
            {
                
                string url=String.Format("https://skyscanner44.p.rapidapi.com/search-extended?adults=1&origin={0}&destination={1}&departureDate={2}&currency=USD",flight.departureCity,flight.arrivalCity,flight.date);
                
                var client = new HttpClient();
                var request = new HttpRequestMessage
            {
	            Method = HttpMethod.Get,
	            RequestUri = new Uri(url),
	            Headers =
	            {
		            { "X-RapidAPI-Key", apiKeys.key.ToString() },
		            { "X-RapidAPI-Host", apiKeys.Host.ToString() },
	            },
            };
            using (var response = await client.SendAsync(request))
            {
	            response.EnsureSuccessStatusCode();
	            string body =  response.Content.ReadAsStringAsync().Result;
                TravvelData travInfo = JsonConvert.DeserializeObject<TravvelData>(body.ToString()); 
                listOfFights.Add(travInfo.itineraries);            
            }
            }
        }

        private static List<FlightInformation> storeTheData(string[] data)
        {
            List<FlightInformation> flightInformation= new List<FlightInformation>();

            foreach (string item in data)
            {
                string[]toks=item.Split(",");
                flightInformation.Add(new FlightInformation(toks[0],toks[1],toks[2]));    
            }
            return flightInformation;
        }

        private static void writeOutToCSV(List<string> csvFlights)
        {
            using (FileStream fs = new FileStream("SavedFlightsAll.csv",FileMode.Append, FileAccess.Write))
            {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (String s in csvFlights)
            {
                sw.WriteLine(s.ToString());
            }
            //sw.Close();
            }
            }
        
        }

        private static void appendNewFlights(List<string> csvFlights, List<Itineraries> listOfFights)
        {
           // System.Console.WriteLine(listOfFights.Count);
            
            
            foreach (Itineraries itineary in listOfFights)
            {
                
                foreach (Result flight in itineary.results)
            {
                string flightId=flight.id.ToString();
                double priceCost=0;
                
                foreach (PricingOption flightCost in flight.pricing_options)
                {
                        double amount = flightCost.price.amount;
                        priceCost+= amount;
                    
                }
                
                foreach (Leg infomation in flight.legs)
                {
                   
                    //FlightId   ,Price  ,StopCount  ,Departue  ,Arrival  ,FlightTime  ,Airline(s)  ,DepartureCity,ArrivalCity
                    string stopCount=infomation.stopCount.ToString();
                    string departureDate=infomation.departure.ToString();
                    //var daysInDifference= infomation.departure-DateTime.Now;
                    string arrivalDate=infomation.arrival.ToString();
                    string flightTime=infomation.durationInMinutes.ToString();
                    string airlineNames="";
                    Carriers s=infomation.carriers;
                    
                    foreach(Marketing airline in s.marketing)
                    {
                        airlineNames+=",";
                        airlineNames+=airline.name;
                        
                    }
                    string origin=infomation.origin.name.ToString();
                    string destination=infomation.destination.name.ToString();
                    string[] flightValue=new string[]{flightId,priceCost.ToString(),stopCount,departureDate,arrivalDate,flightTime,airlineNames,origin,destination,DateTime.Now.ToString("MM/dd/yyyy")};
                    string csvString= string.Join(",",flightValue); 
                    //System.Console.WriteLine("\n{0}",csvString);
                    csvFlights.Add(csvString); 
                }
                
            }
        
            
            
        }
        }
    
    public class Origin
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayCode { get; set; }
    }

    public class Destination
    {
        public int id { get; set; }
        public string name { get; set; }
        public string displayCode { get; set; }
    }

    public class Marketing
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Carriers
    {
        public IList<Marketing> marketing { get; set; }
        public string operationType { get; set; }
    }

    public class MarketingCarrier
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alternate_di { get; set; }
        public int allianceId { get; set; }
    }

    public class OperatingCarrier
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alternate_di { get; set; }
        public int allianceId { get; set; }
    }

    public class Segment
    {
        public string id { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public DateTime departure { get; set; }
        public DateTime arrival { get; set; }
        public int durationInMinutes { get; set; }
        public string flightNumber { get; set; }
        public MarketingCarrier marketingCarrier { get; set; }
        public OperatingCarrier operatingCarrier { get; set; }
    }

    public class Leg
    {
        public string id { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public int durationInMinutes { get; set; }
        public int stopCount { get; set; }
        public bool isSmallestStops { get; set; }
        public DateTime departure { get; set; }
        public DateTime arrival { get; set; }
        public int timeDeltaInDays { get; set; }
        public Carriers carriers { get; set; }
        public IList<Segment> segments { get; set; }
    }

    public class RatingBreakdown
    {
        public double reliable_prices { get; set; }
        public double clear_extra_fees { get; set; }
        public double customer_service { get; set; }
        public double ease_of_booking { get; set; }
        public double other { get; set; }
    }

    public class Agent
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool is_carrier { get; set; }
        public string update_status { get; set; }
        public bool optimised_for_mobile { get; set; }
        public bool live_update_allowed { get; set; }
        public string rating_status { get; set; }
        public double rating { get; set; }
        public int feedback_count { get; set; }
        public RatingBreakdown rating_breakdown { get; set; }
    }

    public class Price
    {
        public double amount { get; set; }
        public string update_status { get; set; }
        public object last_updated  { get; set; }
        public int quote_age { get; set; }
    }

    public class PricingOption
    {
        public IList<Agent> agents { get; set; }
        public Price price { get; set; }
        public string url { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public IList<Leg> legs { get; set; }
        public IList<PricingOption> pricing_options { get; set; }
        public string deeplink { get; set; }
    }

    public class Itineraries
    {
        public IList<Result> results { get; set; }
    }

    public class Context
    {
        public string status { get; set; }
        public string sessionId { get; set; }
    }

    public class TravvelData
    {
        public Itineraries itineraries { get; set; }
        public Context context { get; set; }
    }
    }
}
