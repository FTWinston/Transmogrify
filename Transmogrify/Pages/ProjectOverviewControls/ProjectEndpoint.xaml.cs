using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Transmogrify.Pages.ProjectOverviewControls
{
    /// <summary>
    /// Interaction logic for ProjectEndpoint.xaml
    /// </summary>
    public partial class ProjectEndpoint : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Brush fill;
        public Brush Fill
        {
            get => fill;
            set
            {
                fill = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Fill)));
            }
        }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        private bool highlight = false;
        public bool Highlight
        {
            get => highlight;
            set
            {
                highlight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Highlight)));
            }
        }

        private bool hovering = false;
        public bool Hovering
        {
            get => hovering;
            set
            {
                hovering = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hovering)));
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
