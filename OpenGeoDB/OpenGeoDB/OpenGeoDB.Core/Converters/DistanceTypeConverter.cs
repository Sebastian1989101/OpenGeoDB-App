using System;
using System.Globalization;
using OpenGeoDB.Core.Model.Data;
using OpenGeoDB.Core.Resources;
using Xamarin.Forms;

namespace OpenGeoDB.Core.Converters
{
    public class DistanceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DistanceType))
                return value;

            DistanceType type = (DistanceType)value;
            switch(type)
            {
                case DistanceType.Kilometers:
                    return AppResources.DistanceTypeConverter_Kilometers;
                case DistanceType.NauticalMiles:
                    return AppResources.DistanceTypeConverter_NauticalMiles;
                case DistanceType.Miles:
                    return AppResources.DistanceTypeConverter_Miles;
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == AppResources.DistanceTypeConverter_Kilometers)
                return DistanceType.Kilometers;
            
            if (value.ToString() == AppResources.DistanceTypeConverter_NauticalMiles)
                return DistanceType.NauticalMiles;

            if (value.ToString() == AppResources.DistanceTypeConverter_Miles)
                return DistanceType.Miles;

            return value;
        }
    }
}
