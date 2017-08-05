namespace OpenGeoDB.Core.Model.Data
{
    public class Location
    {
        public int ID { get; set; }

        public string ZipCode { get; set; }
        public string Village { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
