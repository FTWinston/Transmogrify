using System.Windows.Controls;

namespace Transmogrify.Controls
{
    /// <summary>
    /// Interaction logic for ProjectMapping.xaml
    /// </summary>
    public partial class ProjectMapping : UserControl
    {
        public ProjectMapping()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Text { get; set; }
    }
}
