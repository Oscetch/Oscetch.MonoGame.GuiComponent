using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Editor.Extensions
{
    internal static class AssemblyExtensions
    {
        public static IEnumerable<Assembly> GetReferencedAssembliesAtPath(this Assembly assembly, string originalAssemblyPath)
        {
            var directoryPath = Path.GetDirectoryName(originalAssemblyPath);
            yield return assembly;
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
            {
                Assembly loadedAssembly = null;
                try
                {
                    loadedAssembly = Assembly.Load(assemblyName);
                }
                catch {}
                if (loadedAssembly == null)
                {
                    yield return Assembly.LoadFrom(Path.Join(directoryPath, $"{assemblyName.Name}.dll"));
                }
                else
                {
                    yield return loadedAssembly;
                }
            }
            yield return typeof(object).Assembly;
        }
    }
}
