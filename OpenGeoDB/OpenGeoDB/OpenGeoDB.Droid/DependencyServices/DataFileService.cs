using System.IO;
using System.Threading.Tasks;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.Droid.DependencyServices;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(DataFileService))]
namespace OpenGeoDB.Droid.DependencyServices
{
    public class DataFileService : IDataFileService
    {
        public Task<string> LoadFileContentAsync()
        {
            using (StreamReader reader = new StreamReader(Application.Context.Assets.Open("Data.tab")))
                return Task.FromResult(reader.ReadToEnd());
        }
    }
}