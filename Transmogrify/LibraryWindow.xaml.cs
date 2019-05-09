using System.Windows;

namespace Transmogrify
{
    /// <summary>
    /// Interaction logic for LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        public LibraryWindow()
        {
            // Don't know why but properties on the markup are being ignored.
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Height = 350;
            Width = 400;
            Title = "Transmogrify library resources";
        }
    }
}
