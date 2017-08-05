using System.IO;
using System.Threading.Tasks;
using Foundation;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataFileService))]
namespace OpenGeoDB.iOS.DependencyServices
{
    public class DataFileService : IDataFileService
    {
        public Task<string> LoadFileContentAsync()
        {
            var filepath = NSBundle.MainBundle.PathForResource("Data", "tab");
            return Task.FromResult(File.ReadAllText(filepath));
        }
    }
}
