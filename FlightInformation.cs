namespace MathProject_Capstone_
{
    public class FlightInformation
    {
        

        

        public string departureCity { get; set; }

        

        public string arrivalCity { get; set; }
        public string date { get; set; }
        public FlightInformation(string departureCity,string arrivalCity,string date)
        {
            this.departureCity = departureCity;
            this.arrivalCity = arrivalCity;
            this.date = date;
        }
    }
    
}