using System.IO;
using System.Reflection;

namespace IFVM.TestUtilities
{
    public static class Resources
    {
        public const string Glulx_Advent = "advent.ulx";
        public const string Glulx_Glulxercise = "glulxercise.ulx";

        public static Stream LoadResource(string resourceName)
        {
            return typeof(Resources).GetTypeInfo().Assembly.GetManifestResourceStream($"IFVM.TestUtilities.Resources.{resourceName}");
        }
    }
}
