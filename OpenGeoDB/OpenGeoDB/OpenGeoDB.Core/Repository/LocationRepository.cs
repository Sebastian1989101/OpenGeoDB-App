using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.Repository
{
    public class LocationRepository
    {
        private readonly IDataFileService _fileService;
        private readonly IAppSettings _appSettings;

        public LocationRepository(IDataFileService fileService, IAppSettings appSettings)
        {
			_fileService = fileService;
            _appSettings = appSettings;
        }

        public async Task<Location[]> GetAllAsync()
        {
            string content = await _fileService.LoadFileContentAsync();
			if (string.IsNullOrEmpty(content))
				return null;

            string[] lines = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines == null || lines.Length == 0)
                return null;

            List<Location> locations = new List<Location>();
            foreach (var line in lines)
            {
                Match match = Regex.Match(line, @"^(?<id>\d+)[\s\t]+(?<zip>\d{5})[\s\t]+(?<lon>[0-9]+(\.[0-9]+)?)[\s\t]+(?<lat>[0-9]+(\.[0-9]+)?)[\s\t]+(?<vil>.+)$");
                if (match.Success)
                {
                    locations.Add(new Location
	                    {
                            ID = Convert.ToInt32(match.Groups["id"].Value),
                            
	                        ZipCode = match.Groups["zip"].Value, 
	                        Village = match.Groups["vil"].Value,

	                        Latitude = Convert.ToDouble(match.Groups["lat"].Value, CultureInfo.InvariantCulture),
                            Longitude = Convert.ToDouble(match.Groups["lon"].Value, CultureInfo.InvariantCulture)
                        });
                }
            }

            return locations.ToArray();
        }

        public async Task<Location[]> GetNearbyEntries(Location location, int neighboursCount = 10, bool includeLocationParameter = true)
        {
            //Note (sk): Maybe replace return array with IEnumerable?
            List<Location> allLocations = (await GetAllAsync()).ToList();
            allLocations.RemoveAll(l => l.ZipCode == location.ZipCode);

            if (includeLocationParameter)
            {
                location.Distance = 0;

                List<Location> tempLocations = allLocations.OrderBy(l => DistanceBetween(location, l)).ToList();
                tempLocations.Insert(0, location);

                return tempLocations.GetRange(0, neighboursCount + 1).ToArray();
            }

            return allLocations.OrderBy(l => DistanceBetween(location, l))
                               .ToList().GetRange(0, neighboursCount)
                               .ToArray();
        }

        private double DistanceBetween(Location start, Location destination)
        {
            double startLat = Math.PI * start.Latitude / 180;
            double destLat = Math.PI * destination.Latitude / 180;
            double combinedLng = Math.PI * (start.Longitude - destination.Longitude) / 180;

            double distance = Math.Sin(startLat) * Math.Sin(destLat) +
                              Math.Cos(startLat) * Math.Cos(destLat) *
                              Math.Cos(combinedLng);

            distance = Math.Acos(distance);
            distance = distance * 180 / Math.PI;
            distance = distance * 60 * 1.1515;

            destination.DistanceType = _appSettings.DistanceType;
            switch (destination.DistanceType)
            {
                case DistanceType.Kilometers: 
                    destination.Distance = distance * 1.609344;
                    break;
                case DistanceType.NauticalMiles: 
                    destination.Distance = distance * 0.8684;
                    break;
                case DistanceType.Miles: 
                    destination.Distance = distance;
                    break;
            }

			return destination.Distance;
        }
    }
}
