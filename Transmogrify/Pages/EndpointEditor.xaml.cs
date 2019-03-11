using System.Windows.Controls;
using Transmogrify.Data;

namespace Transmogrify.Pages
{
    /// <summary>
    /// Interaction logic for EndpointEditor.xaml
    /// </summary>
    public partial class EndpointEditor : Page
    {
        public EndpointEditor(DataEndPoint endPoint)
        {
            InitializeComponent();
            EndPoint = endPoint;
            DataContext = endPoint;
        }

        public DataEndPoint EndPoint { get; }

        private void ButtonBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
