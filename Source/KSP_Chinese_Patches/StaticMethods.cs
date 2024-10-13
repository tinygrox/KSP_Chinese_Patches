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
        public static StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 传入的是 dll 的 name
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsAssemblyLoaded(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException("Assembly name cannot be null or empty.", nameof(assemblyName));
            }
            return AssemblyLoader.loadedAssemblies.Any(a => string.Equals(a.dllName, assemblyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
