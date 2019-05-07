using System.Windows;
using Transmogrify.Services;

namespace Transmogrify
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceContainer.Build();

            ServiceContainer
                .Resolve<LibraryService>()
                .LoadLibraryAssemblies();
        }
    }
}
