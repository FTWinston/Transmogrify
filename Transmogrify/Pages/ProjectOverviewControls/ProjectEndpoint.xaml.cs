using System.Windows.Controls;
using System.Windows.Media;

namespace Transmogrify.Pages.ProjectOverviewControls
{
    /// <summary>
    /// Interaction logic for ProjectEndpoint.xaml
    /// </summary>
    public partial class ProjectEndpoint : UserControl
    {
        public ProjectEndpoint()
        {
            InitializeComponent();
            DataContext = this;
        }

        public const double AspectRatio = 100.0 / 75.0;

        public Brush Fill { get; set; }

        public string Text { get; set; }
    }
}
