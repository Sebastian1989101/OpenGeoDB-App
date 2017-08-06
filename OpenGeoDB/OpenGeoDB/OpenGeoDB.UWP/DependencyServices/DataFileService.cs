using System;
using System.Threading.Tasks;
using Windows.Storage;
using OpenGeoDB.Core.DependencyServices;
using OpenGeoDB.UWP.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(DataFileService))]
namespace OpenGeoDB.UWP.DependencyServices
{
    public class DataFileService : IDataFileService
    {
        public async Task<string> LoadFileContentAsync()
        {
            var dataFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Data.tab"));
            var content = await FileIO.ReadTextAsync(dataFile);
            return content;
        }
    }
}