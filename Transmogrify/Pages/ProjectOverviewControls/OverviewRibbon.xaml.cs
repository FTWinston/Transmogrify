using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using Transmogrify.Services;

namespace Transmogrify.Pages.ProjectOverviewControls
{
    /// <summary>
    /// Interaction logic for OverviewRibbon.xaml
    /// </summary>
    public partial class OverviewRibbon : UserControl
    {
        private LibraryService LibraryService { get; } = ServiceContainer.Resolve<LibraryService>();

        private ProjectService ProjectService { get; } = ServiceContainer.Resolve<ProjectService>();

        public event EventHandler<Type> EndpointCreating;

        public event EventHandler MappingCreating;

        public event EventHandler MappingCancelled;

        public OverviewRibbon()
        {
            InitializeComponent();

            AddEndPointTypesToMenu();
        }

        private void AddEndPointTypesToMenu()
        {
            endpointListItems.Items.Clear();

            var endpointTypes = LibraryService.GetAvailableEndpointTypes();

            foreach (var endpointType in endpointTypes)
            {
                var tmpInstance = ProjectService.CreateEndPoint(endpointType, "tmp");

                RibbonButton menuItem = new RibbonButton()
                {
                    Label = tmpInstance.TypeName + " endpoint",
                    Background = new SolidColorBrush(Color.FromRgb(tmpInstance.Color.R, tmpInstance.Color.G, tmpInstance.Color.B)),
                };

                menuItem.Click += (o, e) => EndpointCreating?.Invoke(o, endpointType);

                endpointListItems.Items.Add(menuItem);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: confirm event?

            Application.Current.Shutdown();
        }

        private void BtnAddMapping_Click(object sender, RoutedEventArgs e)
        {
            btnAddMapping.Visibility = Visibility.Collapsed;
            btnCancelMapping.Visibility = Visibility.Visible;

            MappingCreating?.Invoke(this, EventArgs.Empty);
        }

        private void BtnCancelMapping_Click(object sender, RoutedEventArgs e)
        {
            btnAddMapping.Visibility = Visibility.Visible;
            btnCancelMapping.Visibility = Visibility.Collapsed;

            MappingCancelled?.Invoke(this, EventArgs.Empty);
        }
    }
}
