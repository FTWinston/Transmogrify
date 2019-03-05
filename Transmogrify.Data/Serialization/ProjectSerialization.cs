using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public static string DetermineConfigPath(string projectPath)
        {
            var pathExtensionLength = new FileInfo(projectPath).Extension.Length;
            return projectPath.Insert(projectPath.Length - pathExtensionLength, "config.");
        }

        public static void LoadConfigFromFile(Project project, string filePath)
        {
            var strConfig = File.ReadAllText(filePath);

            LoadConfigFromString(project, strConfig);
        }

        public static void LoadConfigFromString(Project project, string strConfig)
        {
            var settings = GetSerializerSettings();

            // TODO: the objects aren't going to be correctly-typed here...
            // Don't see how to get around this except to re-serialize them and then deserialize again in the loop
            var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(strConfig, settings);

            foreach (var configByName in config)
            {
                var endPoint = project.EndPoints.FirstOrDefault(ep => ep.Name == configByName.Key);
                if (endPoint == null)
                    continue;

                if (!endPoint.TrySetConfiguration(configByName.Value))
                    throw new Exception($"Failed to parse {configByName.Key} configuration");
            }
        }

        public static string SerializeProjectConfig(Project project)
        {
            var objSettings = new Dictionary<string, object>();
            foreach (var endPoint in project.EndPoints)
                objSettings.Add(endPoint.Name, endPoint.GetConfiguration());

            return JsonConvert.SerializeObject(objSettings, Formatting.Indented, GetSerializerSettings());
        }

        public static void SaveConfigToFile(Project project, string filePath)
        {
            var strSettings = SerializeProjectConfig(project);

            File.WriteAllText(filePath, strSettings);
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
