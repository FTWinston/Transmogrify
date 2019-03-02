using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Transmogrify.Data;
using Transmogrify.Data.Serialization;
using Transmogrify.Engine;

namespace Transmogrify.Runner
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the path to one or more project files.");
                Console.WriteLine("Usage: dotnet Transmogrify.Runner project.json");
                Console.WriteLine("Usage: dotnet Transmogrify.Runner \"some project.json\" \"another project.json\"");
                return;
            }

            Console.WriteLine($"Ready to run {args.Length} project(s)...");
            Console.WriteLine();

            foreach (var arg in args)
            {
                RunProject(arg).Wait();
            }

            Console.WriteLine("Done!");
        }

        private async static Task RunProject(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Project file not found: {path}");
                return;
            }

            Console.WriteLine($"Loading {path}...");

            var project = ProjectSerialization.LoadFromFile(path);

            var runner = new ProjectRunner();

            Console.WriteLine($"Running {path}...");

            await runner.Run(project);

            Console.WriteLine($"Finished running {path}");
            Console.WriteLine();
        }
    }
}
