using System.Windows.Controls;
using System.Windows.Media;

namespace Transmogrify.Pages.ProjectOverviewControls
{
    /// <summary>
    /// Interaction logic for ProjectEndpoint.xaml
    /// </summary>
    public partial class ProjectEndpoint : UserControl
    {
        public Brush Fill { get; set; }

        public string Text { get; set; }

        private bool highlight = false;
        public bool Highlight
        {
            get => highlight;
            set
            {
                highlight = value;
                DataContext = null;
                DataContext = this; // Needed to rebind, for some reason
            }
        }

        private bool hovering = false;
        public bool Hovering
        {
            get => hovering;
            set
            {
                hovering = value;
                DataContext = null;
                DataContext = this; // Needed to rebind, for some reason
            }
        }

        public const double AspectRatio = 100.0 / 75.0;

        public ProjectEndpoint()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Ellipse_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Hovering = true;
            e.Handled = true;
        }

        private void Ellipse_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Hovering = false;
            e.Handled = true;
        }
    }
}
