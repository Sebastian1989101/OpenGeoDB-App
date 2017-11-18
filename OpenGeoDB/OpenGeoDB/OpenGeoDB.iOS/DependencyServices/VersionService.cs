using Foundation;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionService))]
namespace OpenGeoDB.iOS.DependencyServices
{
    public class VersionService : IVersionService
	{
		public string GetAppVersion()
		{
			var bundleShortVersionString = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
			var bundleVersion = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();

			return $"{bundleShortVersionString} ({bundleVersion})";
		}
	}
}
