using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Transmogrify.Data;

namespace Transmogrify.Pages.ProjectOverviewControls
{
    /// <summary>
    /// Interaction logic for ProjectCanvas.xaml
    /// </summary>
    public partial class ProjectCanvas : UserControl
    {
        public ProjectCanvas()
        {
            InitializeComponent();
        }

        private List<ProjectEndpoint> EndpointDisplays { get; } = new List<ProjectEndpoint>();
        private List<ProjectMapping> MappingDisplays { get; } = new List<ProjectMapping>();

        public void AddEndPoint(DataEndPoint endpoint)
        {
            var color = Color.FromRgb(endpoint.Color.R, endpoint.Color.G, endpoint.Color.B);

            var endpointDisplay = new ProjectEndpoint
            {
                Fill = new SolidColorBrush(color),
                Text = endpoint.Name,
                Tag = endpoint,
            };

            // TODO: get this going again
            // endpointDisplay.MouseUp += (o, e) => SelectEndpoint(endpoint);

            EndpointDisplays.Add(endpointDisplay);
            canvas.Children.Add(endpointDisplay);
        }

        public void AddMapping(Mapping mapping)
        {
            var mappingDisplay = new ProjectMapping
            {
                Text = mapping.Name,
                Tag = mapping,
            };

            // TODO: get this going again
            // mappingDisplay.MouseUp += (o, e) => SelectMapping(mapping);

            MappingDisplays.Add(mappingDisplay);
            canvas.Children.Add(mappingDisplay);
        }

        #region positioning
        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Reposition(canvas.ActualWidth, canvas.ActualHeight);
        }

        public void Reposition(double totalWidth, double totalHeight)
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
            centerX = (totalWidth - renderWidth) * 0.5 + renderCenterX;
            centerY = (totalHeight - renderHeight) * 0.5 + renderCenterY;

            // determine the distance from this center that endpoints should be placed, so they touch the edges of the render area.
            distanceFromCenter = DetermineDistanceFromCenter
            (
                renderWidth - endpointWidth,
                renderHeight - endpointHeight,
                renderCenterX - endpointWidth * 0.5,
                renderCenterY - endpointHeight * 0.5,
                unitEndpointOffsets.First()
            );
        }

        private static double DetermineDistanceFromCenter(double boundsWidth, double boundsHeight, double startX, double startY, Point unitOffset)
        {
            // take the line from the center represented by this unit, find where it intersects each bounding line of the render area, go with the shortest distance.
            if (unitOffset.X > -0.0001 && unitOffset.X < 0.0001)
            {
                return unitOffset.Y > 0
                    ? boundsHeight - startY
                    : startY;
            }
            else if (unitOffset.Y > -0.0001 && unitOffset.Y < 0.0001)
            {
                return unitOffset.X > 0
                    ? boundsWidth - startX
                    : startX;
            }
            else
            {
                var gradient = unitOffset.Y / unitOffset.X;
                double maxDistance;

                // only consider an edge line if we're pointing toward it
                if (unitOffset.X > 0)
                    maxDistance = GetDistanceToLine(startX, startY, gradient, boundsWidth, true);
                else
                    maxDistance = GetDistanceToLine(startX, startY, gradient, 0, true);

                if (unitOffset.Y > 0)
                    maxDistance = Math.Min(maxDistance, GetDistanceToLine(startX, startY, gradient, boundsHeight, false));
                else
                    maxDistance = Math.Min(maxDistance, GetDistanceToLine(startX, startY, gradient, 0, false));

                return maxDistance;
            }
        }

        private static double GetDistanceToLine(double startX, double startY, double gradient, double lineValue, bool isVertical)
        {
            // y = mx + c, aka
            // startY = gradient * startX + c
            var c = startY - gradient * startX;

            double dx, dy;

            if (isVertical)
            {
                // intersectY = gradient * lineValue + c
                var intersectY = gradient * lineValue + c;

                dx = startX - lineValue;
                dy = startY - intersectY;
            }
            else
            {
                // lineValue = gradient * intersectX + c
                var intersectX = (lineValue - c) / gradient;

                dx = startX - intersectX;
                dy = startY - lineValue;
            }

            return Math.Sqrt(dx * dx + dy * dy);
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

            mappingDisplay.Width = Math.Sqrt(dx * dx + dy * dy);

            var angle = Math.Atan(dy / dx) * 180.0 / Math.PI;

            var group = new TransformGroup();

            group.Children.Add(new RotateTransform(angle, 0, mappingDisplay.Height / 2));
            group.Children.Add(new TranslateTransform(minX, minY - mappingDisplay.Height / 2));

            mappingDisplay.RenderTransform = group;
        }

        #endregion positioning

    }
}
