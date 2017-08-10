using System.Collections.Generic;
using System.Linq;

namespace OpenGeoDB.Core.Model.Data
{
    public class LocationCategoryGroup : List<IGrouping<string, Location>>
    {
        public string Category => $"{this.First().First().Village} - {this.Last().Last().Village}";
        public string ShortCategory => this.First().First().Village[0].ToString();
    }
}
