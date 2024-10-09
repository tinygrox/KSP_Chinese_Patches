using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSP_Chinese_Patches
{
    public static class StaticMethods
    {
        public static bool IsAssemblyLoaded(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException("Assembly name cannot be null or empty.", nameof(assemblyName));
            }
            return AssemblyLoader.loadedAssemblies.Any(a => string.Equals(a.name, assemblyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
