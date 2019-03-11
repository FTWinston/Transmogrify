using System;
using System.Windows;
using System.Windows.Controls;

namespace Transmogrify.Pages.ProjectOverviewControls
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

        public static double BaseHeight => 18;

        private int curve;
        public int Curve
        {
            get { return curve; }
            set
            {
                curve = value;

                if (curve == 0)
                {
                    label.VerticalAlignment = VerticalAlignment.Center;

                    Height = BaseHeight;
                }
                else
                {
                    label.VerticalAlignment = curve > 0
                        ? VerticalAlignment.Bottom
                        : VerticalAlignment.Top;

                    Height = BaseHeight * (Math.Abs(curve) + 1);
                }
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            double endY, midY;

            if (curve == 0)
            {
                endY = midY = Height / 2;
            }
            else if (curve > 0)
            {
                endY = BaseHeight / 2;
                midY = Height - endY;
            }
            else
            {
                midY = BaseHeight / 2;
                endY = Height - midY;
            }

            line.Points.Clear();
            line.Points.Add(new Point(0, endY));
            line.Points.Add(new Point(Width / 3, midY));
            line.Points.Add(new Point(2 * Width / 3, midY));
            line.Points.Add(new Point(Width, endY));
        }
    }
}
