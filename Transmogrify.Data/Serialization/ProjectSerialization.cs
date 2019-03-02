using Newtonsoft.Json;
using System.IO;

namespace Transmogrify.Data.Serialization
{
    public static class ProjectSerialization
    {
        public static Project LoadFromFile(string filePath)
        {
            var strProject = File.ReadAllText(filePath);

            return LoadFromString(strProject);
        }

        public static Project LoadFromString(string strProject)
        {
            var settings = GetSerializerSettings();

            var project = JsonConvert.DeserializeObject<Project>(strProject, settings);

            return project;
        }

        public static string SerializeProject(Project project)
        {
            return JsonConvert.SerializeObject(project, Formatting.Indented, GetSerializerSettings());
        }

        public static void SaveToFile(Project project, string filePath)
        {
            var strProject = SerializeProject(project);

            File.WriteAllText(filePath, strProject);
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {

            };

            return settings;
        }
    }
}
