using System.Windows.Controls;
using Transmogrify.Data;

namespace Transmogrify.Pages
{
    public partial class MappingEditor : Page
    {
        public MappingEditor(Mapping mapping)
        {
            InitializeComponent();
            Mapping = mapping;
            DataContext = mapping;
        }

        public Mapping Mapping { get; }

        private void ButtonBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
