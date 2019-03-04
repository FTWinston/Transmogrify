using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Transmogrify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProjectOverview : Page
    {
        public ProjectOverview()
        {
            InitializeComponent();
        }

        private void ProjectCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeCanvasElements(projectCanvas.ActualWidth, projectCanvas.ActualHeight);
        }

        private void ResizeCanvasElements(double totalWidth, double totalHeight)
        {
            var endpointWidth = totalWidth * 0.4;
            var endpointHeight = totalHeight * 0.4;

            var aspectRatio = 100.0 / 75.0;

            if (endpointWidth > endpointHeight * aspectRatio)
                endpointWidth = endpointHeight * aspectRatio;
            else
                endpointHeight = endpointWidth / aspectRatio;

            endpoint1.Width = endpointWidth;
            endpoint2.Width = endpointWidth;
            endpoint3.Width = endpointWidth;

            endpoint1.Height = endpointHeight;
            endpoint2.Height = endpointHeight;
            endpoint3.Height = endpointHeight;

            Canvas.SetLeft(endpoint1, totalWidth * 0.1);
            Canvas.SetLeft(endpoint2, totalWidth * 0.9 - endpointWidth);
            Canvas.SetLeft(endpoint3, totalWidth * 0.5 - endpointWidth / 2);

            Canvas.SetTop(endpoint1, totalHeight * 0.1);
            Canvas.SetTop(endpoint2, totalHeight * 0.1);
            Canvas.SetTop(endpoint3, totalHeight * 0.9 - endpointHeight);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
