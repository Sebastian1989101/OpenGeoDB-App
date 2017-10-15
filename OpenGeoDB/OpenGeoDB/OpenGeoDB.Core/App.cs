using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Json;
using OpenGeoDB.Core.DependencyServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OpenGeoDB.Core
{
    public class App : MvxApplication
    {
        private static Task _completedTask = Task.FromResult(false);

        public static Task CompletedTask => _completedTask;

        public override void Initialize()
        {
            Mvx.RegisterSingleton(DependencyService.Get<IDataFileService>());
            Mvx.RegisterSingleton(DependencyService.Get<IVersionService>());

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes().
                EndingWith("Repository")
                .AsTypes()
                .RegisterAsLazySingleton();

            Mvx.RegisterType<Services.IAppSettings, Services.AppSettings>();
            Mvx.RegisterType<IMvxJsonConverter, MvxJsonConverter>();
            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            Resources.AppResources.Culture = Mvx.Resolve<Services.ILocalizeService>().GetCurrentCultureInfo();

            Mvx.ConstructAndRegisterSingleton<IMvxAppStart, AppStart>();
            RegisterAppStart(Mvx.Resolve<IMvxAppStart>());
        }
    }
}
