using System.Reflection;
using SkyrimRelationshipExtractor.Utilities;

namespace SkyrimRelationshipExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Skyrim Relationship Extractor for MinAi - v" + Assembly.GetExecutingAssembly().GetName().Version + Environment.NewLine);
            PrintJenkinsBuild();
            Console.ResetColor();

            try
            {

                Extractor extractor = new();

                var relationships = extractor.ParseRelationships();

                FileInfo outFile = new("out.json");

                File.WriteAllText(outFile.FullName, relationships.ToJson());

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Output written to: " + outFile.FullName);
                Console.ResetColor();
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{ex.Message}");
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.WriteLine("Press the any key to exit...");
            Console.ReadKey();

            return;
        }

        private static void PrintJenkinsBuild()
        {
            var attribs = Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>();
            var jenkins = attribs.FirstOrDefault(x => x.Key.Equals("JenkinsBuild"));
            if (jenkins != null && !string.IsNullOrWhiteSpace(jenkins.Value))
                Console.WriteLine("Jenkins Build: " + jenkins.Value);
        }
    }
}
