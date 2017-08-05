﻿using System.Threading.Tasks;

namespace OpenGeoDB.Core.DependencyServices
{
    public interface IDataFileService
    {
		//Note: https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/files/#Saving_and_Loading_Files
		Task<string> LoadFileContentAsync();
    }
}
