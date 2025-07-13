using Oscetch.ScriptComponent;
using System;
using System.IO;
using System.Reflection;

namespace Editor.Extensions
{
    public static class ScriptReferenceExtensions
    {
        public static Type ToType(this ScriptReference reference)
        {
            if (!File.Exists(reference.DllPath)) return null;
            var baseScriptReferenceName = Path.GetFileName(reference.DllPath);
            var baseScriptAssembly = Assembly.LoadFrom(reference.DllPath);
            baseScriptAssembly.GetReferencedAssembliesAtPath(reference.DllPath);
            return baseScriptAssembly.GetType(reference.ScriptClassName);
        }
    }
}
