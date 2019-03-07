using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            Height = BaseHeight;
        }

        public string Text { get; set; }

        public static double BaseHeight => 30.0;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            line.Points.Clear();
            line.Points.Add(new Point(0, Height / 2));
            line.Points.Add(new Point(Width, Height / 2));
        }
    }
}
