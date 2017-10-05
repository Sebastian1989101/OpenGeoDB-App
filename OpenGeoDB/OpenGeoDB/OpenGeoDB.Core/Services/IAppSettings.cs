using OpenGeoDB.Core.Model.Data;

namespace OpenGeoDB.Core.Services
{
    public interface IAppSettings
    {
        bool OrderByZipCode { get; set; }

        int NearbyMarkerCount { get; set; }
        bool ShowZipCodeAboveNearbyMarker { get; set; }
        DistanceType DistanceType { get; set; }
    }
}