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
                mapping.Name = "Mapping 1";
                var sourceCollection = ProjectService.EndPoints[0].PopulateCollections(mapping).First();
                var destCollection = ProjectService.EndPoints[1].PopulateCollections(mapping).First();
                mapping.Source = sourceCollection;
                mapping.Destination = destCollection;

                mapping.Outputs.Add(new MappingOutput(sourceCollection.Fields.First(), destCollection.Fields.First().Field));

                ProjectService.AddMapping(mapping);
            }

            {
                Mapping mapping = new Mapping();
                mapping.Name = "Mapping 2";
                var sourceCollection = ProjectService.EndPoints[0].PopulateCollections(mapping).First();
                var destCollection = ProjectService.EndPoints[2].PopulateCollections(mapping).First();
                mapping.Source = sourceCollection;
                mapping.Destination = destCollection;

                mapping.Outputs.Add(new MappingOutput(sourceCollection.Fields.First(), destCollection.Fields.First().Field));

                ProjectService.AddMapping(mapping);
            }

            foreach (var mapping in ProjectService.Mappings)
            {
                var mappingDisplay = new ProjectMapping
                {
                    Text = mapping.Name,
                    Tag = mapping,
                };

                mappingDisplay.MouseUp += (o, e) => SelectMapping(mapping);

                MappingDisplays.Add(mappingDisplay);
                projectCanvas.Children.Add(mappingDisplay);
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
            DetermineEndpointSize(totalWidth, totalHeight, out double endpointWidth, out double endpointHeight);

            // We precalculate these angles so that we can account for "unused space"
            // e.g. with 3 endpoints, one will angle "straight down" but none will angle "straight up", leaving space at the top

            var angleStep = Math.PI * 2 / EndpointDisplays.Count;
            var startAngle = -angleStep / 2;
            var unitOffsets = Enumerable.Range(0, EndpointDisplays.Count)
                .Select(index => startAngle + angleStep * index) // get the angle to travel out from the center when placing each endpoint
                .Select(angle => new Point(Math.Sin(angle), -Math.Cos(angle))) // determine the x & y offset for each angle
                .ToArray();

            DetermineCenterAndOffsetDistance(totalWidth, totalHeight, endpointWidth, endpointHeight, unitOffsets, out double centerX, out double centerY, out double distanceFromCenter);

            int iEndpoint = 0;
            foreach (ProjectEndpoint endpointDisplay in EndpointDisplays)
            {
                endpointDisplay.Width = endpointWidth;
                endpointDisplay.Height = endpointHeight;

                var offset = unitOffsets[iEndpoint++];

                var displayOffsetX = offset.X * distanceFromCenter;
                var displayOffsetY = offset.Y * distanceFromCenter;// / ProjectEndpoint.AspectRatio;

                Canvas.SetLeft(endpointDisplay, displayOffsetX + centerX - endpointWidth * 0.5);
                Canvas.SetTop(endpointDisplay, displayOffsetY + centerY - endpointHeight * 0.5);
            }
        }

        private static void DetermineEndpointSize(double totalWidth, double totalHeight, out double endpointWidth, out double endpointHeight)
        {
            endpointWidth = totalWidth * 0.25;
            endpointHeight = totalHeight * 0.25;

            var aspectRatio = ProjectEndpoint.AspectRatio;

            if (endpointWidth > endpointHeight * aspectRatio)
                endpointWidth = endpointHeight * aspectRatio;
            else
                endpointHeight = endpointWidth / aspectRatio;
        }

        private static void DetermineCenterAndOffsetDistance(double totalWidth, double totalHeight, double endpointWidth, double endpointHeight, Point[] unitEndpointOffsets, out double centerX, out double centerY, out double distanceFromCenter)
        {
            // determine aspect ratio of useable window area
            var edgePadding = Math.Min(totalWidth * 0.05, totalHeight * 0.05);
            var useableWidth = Math.Max(totalWidth - edgePadding - edgePadding, 1);
            var useableHeight = Math.Max(totalHeight - edgePadding - edgePadding, 1);
            var useableAspectRatio = useableWidth / useableHeight;

            // determine aspect ratio of bounds endpoints will be placed in
            var minUnitOffsetX = unitEndpointOffsets.Min(o => o.X);
            var minUnitOffsetY = unitEndpointOffsets.Min(o => o.Y);
            var maxUnitOffsetX = unitEndpointOffsets.Max(o => o.X);
            var maxUnitOffsetY = unitEndpointOffsets.Max(o => o.Y);

            var unitOffsetWidth = Math.Max(maxUnitOffsetX - minUnitOffsetX, 0.01);
            var unitOffsetHeight = Math.Max(maxUnitOffsetY - minUnitOffsetY, 0.01);

            var renderAspectRatio = unitOffsetWidth / unitOffsetHeight;

            // determine biggest possible width & height for render area
            double renderWidth, renderHeight;

            if (renderAspectRatio >= useableAspectRatio)
            {
                renderWidth = useableWidth;
                renderHeight = useableWidth / renderAspectRatio;
            }
            else
            {
                renderHeight = useableHeight;
                renderWidth = useableHeight * renderAspectRatio;
            }

            // determine where the "center" fits into the render area, as it may not be the middle of it
            var renderCenterX = renderWidth * Math.Abs(minUnitOffsetX) / unitOffsetWidth;
            var renderCenterY = renderHeight * Math.Abs(minUnitOffsetY) / unitOffsetHeight;

            // assuming the render area is to be centered, determine where the "center" fits on the window area
            centerX = renderCenterX - renderWidth / 2 + totalWidth / 2;
            centerY = renderCenterY - renderHeight / 2 + totalHeight / 2;

            // determine how much to scale up the distances based on the unit endpoints not filling a full [-1, 1] range
            var scale = Math.Max(unitOffsetWidth, unitOffsetHeight) / 2;
            if (scale == 0)
                scale = 1;

            // determine how far from the "center" endpoints should be placed
            var maxDistanceX = totalWidth * 0.5 - endpointWidth * 0.5 - edgePadding;
            var maxDistanceY = totalHeight * 0.5 - endpointHeight * 0.5 - edgePadding;

            distanceFromCenter = Math.Min(maxDistanceX, maxDistanceY) / scale;
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
