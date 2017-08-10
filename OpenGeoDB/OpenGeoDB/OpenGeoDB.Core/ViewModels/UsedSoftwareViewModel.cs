using System;
using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace OpenGeoDB.Core.ViewModels
{
    public class UsedSoftwareViewModel : MvxViewModel
    {
        public MvxCommand<string> OpenUrlCommand { get; }

        public UsedSoftwareViewModel()
        {
            OpenUrlCommand = new MvxCommand<string>(url => Device.OpenUri(new Uri(url)));
        }
    }
}
