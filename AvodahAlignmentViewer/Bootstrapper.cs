using AvodahAlignmentViewer.ViewModels;
using Caliburn.Micro;
using System.Windows;

namespace AvodahAlignmentViewer
{
    public class Bootstrapper : BootstrapperBase
    {

        public Bootstrapper()
        {
            Initialize();

            LogManager.GetLog = type => new DebugLog(type);
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync(typeof(ShellViewModel));
        }
    }
}
