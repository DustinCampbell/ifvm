using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AstGenerator
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                var processFileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
                Console.Error.WriteLine($"Usage: \"{processFileName} <input> <output>\"");
                return -1;
            }

            var inputFileName = args[0];
            var outputFileName = args[1];

            var tree = ReadTree(inputFileName);
            WriteTree(tree, outputFileName);

            return 0;
        }

        private static Tree ReadTree(string inputFileName)
        {
            var serializer = new XmlSerializer(typeof(Tree));

            using (var reader = new XmlTextReader(inputFileName))
            {
                return (Tree)serializer.Deserialize(reader);
            }
        }

        private static void WriteTree(Tree tree, string outputFileName)
        {
            using (var writer = new StreamWriter(outputFileName))
            {
                CodeGenerator.Write(writer, tree);
            }
        }
    }
}
