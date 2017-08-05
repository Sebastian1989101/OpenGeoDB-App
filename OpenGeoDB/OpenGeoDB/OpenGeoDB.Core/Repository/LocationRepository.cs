using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Core.Model.Data;

namespace OpenGeoDB.Core.Repository
{
    public class LocationRepository
    {
        private readonly IDataFileService _fileService;

        public LocationRepository(IDataFileService fileService)
        {
            _fileService = fileService;
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
    }
}
