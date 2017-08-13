using Windows.ApplicationModel;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.UWP.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionService))]
namespace OpenGeoDB.UWP.DependencyServices
{
    public class VersionService : IVersionService
    {
        public string GetAppVersion()
        {
            PackageVersion version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }
    }
}