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
            ProjectService.AddEndPoint(new PlainTextEndPoint("Destination 2"));

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
                var destCollection = ProjectService.EndPoints[2].PopulateCollections(mapping).First();
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

            foreach (var mapping in ProjectService.Mappings)
            {
                var mappingDisplay = new ProjectMapping
                {
                    Text = "Mapping name",
                    Tag = mapping,
                };

                mappingDisplay.MouseUp += (o, e) => SelectMapping(mapping);

                MappingDisplays.Add(mappingDisplay);
                projectCanvas.Children.Add(mappingDisplay);
            }
        }

        private List<ProjectEndpoint> EndpointDisplays { get; } = new List<ProjectEndpoint>();
        private List<ProjectMapping> MappingDisplays { get; } = new List<ProjectMapping>();

        #region positioning
        private void ProjectCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeCanvasElements(projectCanvas.ActualWidth, projectCanvas.ActualHeight);
        }

        private void ResizeCanvasElements(double totalWidth, double totalHeight)
        {
            PositionEndpoints(totalWidth, totalHeight);
            PositionMappings();
        }

        private void PositionEndpoints(double totalWidth, double totalHeight)
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

            var distanceFromCenter = Math.Min(totalWidth * 0.5 - endpointWidth * 0.4, totalHeight * 0.5 - endpointHeight * 0.4);

            var angleStep = Math.PI * 2 / ProjectService.EndPoints.Length;
            var currentAngle = -angleStep / 2;

            foreach (ProjectEndpoint endpointDisplay in EndpointDisplays)
            {
                endpointDisplay.Width = endpointWidth;
                endpointDisplay.Height = endpointHeight;

                var displayOffsetX = Math.Sin(currentAngle) * distanceFromCenter;
                var displayOffsetY = -Math.Cos(currentAngle) * distanceFromCenter / ProjectEndpoint.AspectRatio;

                Canvas.SetLeft(endpointDisplay, displayOffsetX + canvasCenterX - endpointWidth * 0.5);
                Canvas.SetTop(endpointDisplay, displayOffsetY + canvasCenterY - endpointHeight * 0.5);

                currentAngle += angleStep;
            }
        }

        private void PositionMappings()
        {
            foreach (ProjectMapping mappingDisplay in MappingDisplays)
            {
                PositionMapping(mappingDisplay);
            }
        }

        private void PositionMapping(ProjectMapping mappingDisplay)
        {
            var mapping = mappingDisplay.Tag as Mapping;

            var endpointDisplay1 = EndpointDisplays.First(d => d.Tag == mapping.Source.EndPoint);
            var endpointDisplay2 = EndpointDisplays.First(d => d.Tag == mapping.Destination.EndPoint);

            var x1 = Canvas.GetLeft(endpointDisplay1) + endpointDisplay1.Width / 2;
            var x2 = Canvas.GetLeft(endpointDisplay2) + endpointDisplay2.Width / 2;
            var y1 = Canvas.GetTop(endpointDisplay1) + endpointDisplay1.Height / 2;
            var y2 = Canvas.GetTop(endpointDisplay2) + endpointDisplay2.Height / 2;

            var dx = Math.Abs(x2 - x1);
            var dy = Math.Abs(y2 - y1);

            var minX = Math.Min(x1, x2);
            var minY = Math.Min(y1, y2);

            var midX = minX + dx / 2;
            var midY = minY + dy / 2;

            var length = Math.Sqrt(dx * dx + dy * dy);
            var scale = length / ProjectMapping.BaseLength;

            var angle = Math.Atan(dy / dx) * 180.0 / Math.PI;

            var group = new TransformGroup();

            group.Children.Add(new ScaleTransform(scale, 1));
            group.Children.Add(new TranslateTransform(minX, minY - ProjectMapping.BaseHeight / 2));
            group.Children.Add(new RotateTransform(angle, midX * scale, midY));

            mappingDisplay.RenderTransform = group;
        }

        #endregion positioning

        private void SelectEndpoint(DataEndPoint endpoint)
        {

        }

        private void SelectMapping(Mapping mapping)
        {

        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
