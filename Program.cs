﻿using System;

using System.Collections.Generic;
using System.Net.Http;

using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MathProject_Capstone_
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
            List<Itineraries> listOfFights= new List<Itineraries>();
            ApiKeys apiKeys= new ApiKeys();
            

           
            var client = new HttpClient();
            var request = new HttpRequestMessage
        {
	        Method = HttpMethod.Get,
	        RequestUri = new Uri("https://skyscanner44.p.rapidapi.com/search?adults=1&origin=ORD&destination=PHX&departureDate=2022-10-11&currency=USD"),
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
	        //Console.WriteLine(body);
            TrravelData travInfo = JsonConvert.DeserializeObject<TrravelData>(body.ToString()); 
            //istOfFights.Add(travInfo);
            //System.Console.WriteLine(travInfo.itineraries.buckets.ToString()); 
            listOfFights.Add(travInfo.itineraries);
            
        }
        foreach (Itineraries its in listOfFights)
        {
            foreach (Bucket bkt in its.buckets)
            {
                string flightId=bkt.id.ToString();
                //System.Console.WriteLine(bkt.id.ToString());
                foreach (Item flight in bkt.items)
                {
                    string price=flight.price.formatted.ToString();
                    foreach (Leg flightInfo in flight.legs)
                    {
                        string stopCount= flightInfo.stopCount.ToString();
                        //String[] toks= flightInfo.departure.ToString().Split("\t");
                        string departureDate=flightInfo.departure.ToString();
                        //string departureTime=toks[1];
                        System.Console.WriteLine(flightInfo.departure.ToString());
                       // String[] toks2= flightInfo.arrival.Split("\t");
                        string arrival=flightInfo.arrival.ToString();
                        //string arrivalTime=toks2[1];
                       // System.Console.WriteLine(toks2);
                        string flightTime=flightInfo.durationInMinutes.ToString();
                        string airlineNames= "";
                        foreach (Marketing airline in flightInfo.carriers.marketing)
                        {
                            airlineNames+=airline.name.ToString();
                            airlineNames+=";";
                        } 
                       
                    }
                }
            
            }
            
            
        }
          
        
        }

       
    }
    public class Price
    {
        public int raw { get; set; }
        public string formatted { get; set; }
    }

    public class Origin
    {
        public string id { get; set; }
        public string name { get; set; }
        public string displayCode { get; set; }
        public string city { get; set; }
        public bool isHighlighted { get; set; }
    }

    public class Destination
    {
        public string id { get; set; }
        public string name { get; set; }
        public string displayCode { get; set; }
        public string city { get; set; }
        public bool isHighlighted { get; set; }
    }

    public class Marketing
    {
        public int id { get; set; }
        public string logoUrl { get; set; }
        public string name { get; set; }
    }

    public class Operating
    {
        public int id { get; set; }
        public string logoUrl { get; set; }
        public string name { get; set; }
    }

    public class Carriers
    {
        public IList<Marketing> marketing { get; set; }
        public string operationType { get; set; }
        public IList<Operating> operating { get; set; }
    }

    public class MarketingCarrier
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alternateId { get; set; }
        public int allianceId { get; set; }
    }

    public class OperatingCarrier
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alternateId { get; set; }
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

    public class Eco
    {
        public double ecoContenderDelta { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public Price price { get; set; }
        public IList<Leg> legs { get; set; }
        public bool isSelfTransfer { get; set; }
        public Eco eco { get; set; }
        public bool isMashUp { get; set; }
        public bool hasFlexibleOptions { get; set; }
        public double score { get; set; }
        public string deeplink { get; set; }
        public IList<string> tags { get; set; }
    }

    public class Bucket
    {
        public string id { get; set; }
        public string name { get; set; }
        public IList<Item> items { get; set; }
    }

    public class Itineraries
    {
        public IList<Bucket> buckets { get; set; }
    }

    public class Context
    {
        public string status { get; set; }
        public string sessionId { get; set; }
        public int totalResults { get; set; }
    }

    public class TrravelData
    {
        public Itineraries itineraries { get; set; }
        public Context context { get; set; }
    
}}
