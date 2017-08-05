using System;

namespace OpenGeoDB.Core.Model.Data
{
    public class Location : IEquatable<string>
    {
        public int ID { get; set; }

        public string ZipCode { get; set; }
        public string Village { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool Equals(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return true;
            
            return $"{ZipCode} {Village}".ToLower().Contains(filter?.ToLower());
        }
    }
}
