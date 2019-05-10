using System.Collections.Generic;
using System.Windows;
using Transmogrify.Services;

namespace Transmogrify
{
    /// <summary>
    /// Interaction logic for LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        private LibraryService LibraryService { get; } = ServiceContainer.Resolve<LibraryService>();

        private List<string> LibraryPaths;

        public LibraryWindow()
        {
            InitializeComponent();
            LibraryPaths = new List<string>(LibraryService.Library);
            DataContext = LibraryPaths;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            LibraryService.UpdateLibrary(LibraryPaths);

            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
