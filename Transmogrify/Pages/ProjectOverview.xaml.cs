using System;
using System.Linq;
using System.Windows.Controls;
using Transmogrify.Data;
using Transmogrify.Data.EndPoints;
using Transmogrify.Services;

namespace Transmogrify.Pages
{
    public partial class ProjectOverview : Page
    {
        public enum OverviewState
        {
            Normal = 1,
            AddingMapping = 2,
        }

        private ProjectService ProjectService { get; } = ServiceContainer.Resolve<ProjectService>();

        private OverviewState State { get; set; } = OverviewState.Normal;

        public ProjectOverview()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!ProjectService.EndPoints.Any())
                AddDummyProject();

            projectCanvas.AddProjectElements();
        }

        private void CreateAndAddEndPoint(Type type)
        {
            var endpoint = ProjectService.CreateEndPoint(type, "New endpoint");

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

        private void OverviewRibbon_EndpointCreating(object sender, Type endpointType)
        {
            CreateAndAddEndPoint(endpointType);
        }

        private void ProjectCanvas_MappingSelected(object sender, Mapping mapping)
        {
            if (State == OverviewState.Normal)
            {
                OpenMapping(mapping);
            }
        }

        private void OpenEndpoint(DataEndPoint endpoint)
        {
            var editor = new EndpointEditor(endpoint);
            NavigationService.Navigate(editor);
        }

        private void OpenMapping(Mapping mapping)
        {
            var editor = new MappingEditor(mapping);
            NavigationService.Navigate(editor);
        }

        private void ProjectCanvas_EndpointSelected(object sender, DataEndPoint endpoint)
        {
            if (State == OverviewState.Normal)
            {
                OpenEndpoint(endpoint);
            }
            else if (State == OverviewState.AddingMapping)
            {
                if (mappingCreationSelectedEndpoint == null)
                {
                    mappingCreationSelectedEndpoint =
                    projectCanvas.HighlightEndpoint = endpoint;

                    projectCanvas.Prompt = "Select endpoint to map TO";
                }
                else
                {
                    if (endpoint == mappingCreationSelectedEndpoint)
                        return;

                    var mapping = CreateAndAddMapping(mappingCreationSelectedEndpoint, endpoint, "New mapping");

                    mappingCreationSelectedEndpoint =
                    projectCanvas.HighlightEndpoint = null;
                    projectCanvas.Prompt = null;

                    OpenMapping(mapping);
                }
            }
        }

        private Mapping CreateAndAddMapping(DataEndPoint source, DataEndPoint destination, string name)
        {
            Mapping mapping = new Mapping();
            mapping.Name = name;
            mapping.Source = source.PopulateCollections(mapping).First();
            mapping.Destination = destination.PopulateCollections(mapping).First();

            ProjectService.AddMapping(mapping);
            projectCanvas.AddMapping(mapping);

            return mapping;
        }

        private DataEndPoint mappingCreationSelectedEndpoint = null;
        private void OverviewRibbon_MappingCreating(object sender, EventArgs e)
        {
            projectCanvas.HighlightEndpoint = null;
            projectCanvas.Prompt = "Select endpoint to map FROM";

            State = OverviewState.AddingMapping;
        }

        private void OverviewRibbon_MappingCancelled(object sender, EventArgs e)
        {
            mappingCreationSelectedEndpoint =
            projectCanvas.HighlightEndpoint = null;
            projectCanvas.Prompt = null;
        }
    }
}
