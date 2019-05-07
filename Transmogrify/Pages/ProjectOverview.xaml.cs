using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using Transmogrify.Data;
using Transmogrify.Data.EndPoints;
using Transmogrify.Pages.ProjectOverviewControls;
using Transmogrify.Services;

namespace Transmogrify.Pages
{
    public partial class ProjectOverview : Page
    {
        public ProjectOverview()
        {
            InitializeComponent();
        }

        ProjectService ProjectService { get; } = ServiceContainer.Resolve<ProjectService>();

        LibraryService LibraryService { get; } = ServiceContainer.Resolve<LibraryService>();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            AddEndPointTypesToMenu();

            if (!ProjectService.EndPoints.Any())
                AddDummyProject();

            foreach (var mapping in ProjectService.Mappings)
            {
                projectCanvas.AddMapping(mapping);
            }

            foreach (var endpoint in ProjectService.EndPoints)
            {
                projectCanvas.AddEndPoint(endpoint);
            }
        }

        private void AddEndPointTypesToMenu()
        {
            endpointListItems.Items.Clear();

            var endpointTypes = LibraryService.GetAvailableEndpointTypes();

            foreach (var endpointType in endpointTypes)
            {
                var tmpInstance = CreateEndPoint(endpointType, "tmp");

                RibbonButton menuItem = new RibbonButton()
                {
                    Label = endpointType.Name.Replace("EndPoint", " endpoint"), // TODO: need a better way of getting a name. An attribute, maybe?
                    Background = new SolidColorBrush(Color.FromRgb(tmpInstance.Color.R, tmpInstance.Color.G, tmpInstance.Color.B)),
                };

                menuItem.Click += (o, e) => CreateAndAddEndPoint(endpointType);

                endpointListItems.Items.Add(menuItem);
            }
        }

        private DataEndPoint CreateEndPoint(Type type, string name)
        {
            return Activator.CreateInstance(type, new[] { name }) as DataEndPoint;
        }

        private void CreateAndAddEndPoint(Type type)
        {
            var endpoint = CreateEndPoint(type, "New endpoint");

            ProjectService.AddEndPoint(endpoint);
            projectCanvas.AddEndPoint(endpoint);

            projectCanvas.Reposition(projectCanvas.ActualWidth, projectCanvas.ActualHeight);
        }

        private void AddDummyProject()
        {
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
        }

        private void SelectEndpoint(DataEndPoint endpoint)
        {
            var editor = new EndpointEditor(endpoint);
            NavigationService.Navigate(editor);
        }

        private void SelectMapping(Mapping mapping)
        {
            var editor = new MappingEditor(mapping);
            NavigationService.Navigate(editor);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
