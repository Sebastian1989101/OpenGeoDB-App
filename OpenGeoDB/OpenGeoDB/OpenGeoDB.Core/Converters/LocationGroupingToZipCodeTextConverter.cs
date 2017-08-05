using System;
using System.Globalization;
using System.Linq;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Resources;
using Xamarin.Forms;

namespace OpenGeoDB.Core.Converters
{
    public class LocationGroupingToZipCodeTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grouping = value as IGrouping<string, Location>;
            if (grouping == null)
                return null;

            string zipCodes = string.Join(", ", grouping.Select(grp => grp.ZipCode).OrderBy(zip => zip));
            if (string.IsNullOrEmpty(zipCodes))
                return null;

            int lastIndex = zipCodes.LastIndexOf(',');
            if (lastIndex > 0)
                zipCodes = zipCodes.Remove(lastIndex, 1).Insert(lastIndex, AppResources.LocationGroupingToZipCodeTextConverter_And);

            return zipCodes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}