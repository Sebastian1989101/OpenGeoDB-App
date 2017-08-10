using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionService))]
namespace OpenGeoDB.Droid.DependencyServices
{
    public class VersionService : IVersionService
    {
		public string GetAppVersion()
		{
			var packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0);
			return $"{packageInfo.VersionName} ({packageInfo.VersionCode})";
		}
    }
}
