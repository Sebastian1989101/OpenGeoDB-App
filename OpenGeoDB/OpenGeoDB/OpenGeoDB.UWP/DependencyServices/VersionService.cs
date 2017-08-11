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
            return Package.Current.Id.Version.ToString();
        }
    }
}