using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Editor.Extensions
{
    public static class StringExtensions
    {
        public static string FindFileInPath(this string path, string endsWith, List<string> except = null)
        {
            var fileMatch = Directory.GetFiles(path).FirstOrDefault(x => x.EndsWith(endsWith));
            if (fileMatch != null) return fileMatch;
            foreach (var dir in Directory.GetDirectories(path))
            {
                if (except?.Any(dir.EndsWith) == true) continue;
                var childMatch = FindFileInPath(dir, endsWith, except);
                if (childMatch != null) return childMatch;
            }
            return null;
        }
    }
}
