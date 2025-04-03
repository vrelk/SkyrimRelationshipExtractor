using SkyrimRelationshipExtractor.Utilities;

namespace SkyrimRelationshipExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Skyrim Relationship Extractor for MinAi - v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + Environment.NewLine);
            Console.ResetColor();

            Extractor extractor = new();

            var relationships = extractor.ParseRelationships();

            FileInfo outFile = new("out.json");

            File.WriteAllText(outFile.FullName, relationships.ToJson());

            Console.WriteLine("Output written to: " + outFile.FullName);
            Console.WriteLine();

            Console.WriteLine("Press the any key to exit...");
            Console.ReadKey();

            return;
        }
    }
}
