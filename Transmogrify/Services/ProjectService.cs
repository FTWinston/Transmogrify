using System.IO;
using Transmogrify.Data;
using Transmogrify.Data.Serialization;

namespace Transmogrify.Services
{
    public class ProjectService
    {
        private Project CurrentProject { get; set; } = new Project();

        public string CurrentProjectPath { get; private set; }

        public bool HasChanges { get; private set; }

        public void CreateNew()
        {
            CurrentProject = new Project();
            CurrentProjectPath = null;
        }

        public bool OpenProject(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            try
            {
                CurrentProject = ProjectSerialization.LoadFromFile(filePath);
                CurrentProjectPath = filePath;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool SaveProject()
        {
            if (CurrentProjectPath == null)
                return false;

            return SaveProject(CurrentProjectPath);
        }

        public bool SaveProject(string filePath)
        {
            try
            {
                ProjectSerialization.SaveToFile(CurrentProject, CurrentProjectPath);
                CurrentProjectPath = filePath;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public DataEndPoint[] EndPoints => CurrentProject?.EndPoints.ToArray();

        public void AddEndPoint(DataEndPoint endPoint)
        {
            if (CurrentProject.EndPoints.Contains(endPoint))
                return;

            CurrentProject.EndPoints.Add(endPoint);

            HasChanges = true;
        }

        public void RemoveEndPoint(DataEndPoint endPoint)
        {
            var didRemove = CurrentProject.EndPoints.Remove(endPoint);

            if (didRemove)
                HasChanges = true;
        }
    }
}
