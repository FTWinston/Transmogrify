using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Transmogrify.Controls;
using Transmogrify.Data;
using Transmogrify.Data.EndPoints;
using Transmogrify.Services;

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

        ProjectService ProjectService { get; } = ServiceContainer.Resolve<ProjectService>();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ProjectService.AddEndPoint(new PlainTextEndPoint("Source 1"));
            ProjectService.AddEndPoint(new PlainTextEndPoint("Destination 1"));
            ProjectService.AddEndPoint(new PlainTextEndPoint("Source 2"));

            {
                Mapping mapping = new Mapping();
                var sourceCollection = ProjectService.EndPoints[0].PopulateCollections(mapping).First();
                var destCollection = ProjectService.EndPoints[1].PopulateCollections(mapping).First();
                mapping.Source = sourceCollection;
                mapping.Destination = destCollection;

                mapping.Outputs.Add(new MappingOutput(sourceCollection.Fields.First(), destCollection.Fields.First().Field));

                ProjectService.AddMapping(mapping);
            }

            {
                Mapping mapping = new Mapping();
                var sourceCollection = ProjectService.EndPoints[0].PopulateCollections(mapping).First();
                var destCollection = ProjectService.EndPoints[1].PopulateCollections(mapping).First();
                mapping.Source = sourceCollection;
                mapping.Destination = destCollection;

                mapping.Outputs.Add(new MappingOutput(sourceCollection.Fields.First(), destCollection.Fields.First().Field));

                ProjectService.AddMapping(mapping);
            }

            foreach (var endpoint in ProjectService.EndPoints)
            {
                var color = Color.FromArgb(endpoint.Color.A, endpoint.Color.R, endpoint.Color.G, endpoint.Color.B);

                var endpointDisplay = new ProjectEndpoint
                {
                    Fill = new SolidColorBrush(color),
                    Text = endpoint.Name,
                    Tag = endpoint,
                };

                endpointDisplay.MouseUp += (o, e) => SelectEndpoint(endpoint);

                EndpointDisplays.Add(endpointDisplay);
                projectCanvas.Children.Add(endpointDisplay);
            }
        }

        private List<ProjectEndpoint> EndpointDisplays { get; } = new List<ProjectEndpoint>();

        private void ProjectCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeCanvasElements(projectCanvas.ActualWidth, projectCanvas.ActualHeight);
        }

        private void ResizeCanvasElements(double totalWidth, double totalHeight)
        {
            var endpointWidth = totalWidth * 0.4;
            var endpointHeight = totalHeight * 0.4;

            var aspectRatio = ProjectEndpoint.AspectRatio;

            if (endpointWidth > endpointHeight * aspectRatio)
                endpointWidth = endpointHeight * aspectRatio;
            else
                endpointHeight = endpointWidth / aspectRatio;

            var canvasCenterX = totalWidth * 0.5;
            var canvasCenterY = totalHeight * 0.5;

            var radius = Math.Min(totalWidth * 0.5 - endpointWidth * 0.4, totalHeight * 0.5 - endpointHeight * 0.4);

            var angleStep = Math.PI * 2 / ProjectService.EndPoints.Length;
            var currentAngle = -angleStep / 2;

            foreach (ProjectEndpoint endpointDisplay in EndpointDisplays)
            {
                endpointDisplay.Width = endpointWidth;
                endpointDisplay.Height = endpointHeight;

                var displayCenterX = Math.Sin(currentAngle) * radius + canvasCenterX;
                var displayCenterY = -Math.Cos(currentAngle) * radius + canvasCenterY;

                Canvas.SetLeft(endpointDisplay, displayCenterX - endpointWidth * 0.5);
                Canvas.SetTop(endpointDisplay, displayCenterY - endpointHeight * 0.5);

                currentAngle += angleStep;
            }
        }

        private void SelectEndpoint(DataEndPoint endpoint)
        {

        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
