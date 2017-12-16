using System.Collections.Generic;
using System.Linq;
using OpenGeoDB.Core.Services;

namespace OpenGeoDB.Core.Model.Data
{
    public class LocationCategoryGroup : List<IGrouping<string, Location>>
    {
        private IAppSettings _appSettings;

        public string Category => _appSettings.OrderByZipCode ? 
                                  $"{this.First().First().ZipCode} - {this.Last().Last().ZipCode}" :
                                  $"{this.First().First().Village} - {this.Last().Last().Village}";
        
        public string ShortCategory => _appSettings.OrderByZipCode ? 
                                       this.First().First().ZipCode[0].ToString() : 
                                       this.First().First().Village[0].ToString();

        public LocationCategoryGroup(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }
    }
}
