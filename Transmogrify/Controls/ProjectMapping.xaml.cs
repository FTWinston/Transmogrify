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

        private bool? curveDownward;
        public bool? CurveDownward
        {
            get { return curveDownward; }
            set
            {
                curveDownward = value;

                if (curveDownward.HasValue)
                {
                    label.VerticalContentAlignment = curveDownward.Value
                        ? VerticalAlignment.Top
                        : VerticalAlignment.Bottom;
                }
                else
                    label.VerticalContentAlignment = VerticalAlignment.Center;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            double endY, midY;

            if (curveDownward.HasValue)
            {
                if (curveDownward.Value)
                {
                    endY = BaseHeight / 2;
                    midY = Height - endY;
                }
                else
                {
                    midY = BaseHeight / 2;
                    endY = Height - midY;
                }
            }
            else
            {
                endY = midY = Height / 2;
            }

            line.Points.Clear();
            line.Points.Add(new Point(0, endY));
            line.Points.Add(new Point(Width / 3, midY));
            line.Points.Add(new Point(2 * Width / 3, midY));
            line.Points.Add(new Point(Width, endY));
        }
    }
}
