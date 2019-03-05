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

        public static double BaseLength => 150.0;

        public static double BaseHeight => 30.0;
    }
}
