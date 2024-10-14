using KSP.IO;
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
        public static Dictionary<string, Version> AssemblyVersionMap => AssemblyLoader.loadedAssemblies.ToDictionary(a => a.dllName.ToLowerInvariant(), a => new Version(a.versionMajor, a.versionMinor, a.versionRevision));

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
            //return AssemblyLoader.loadedAssemblies.Any(a => string.Equals(a.dllName, assemblyName, StringComparison.OrdinalIgnoreCase));
            return AssemblyVersionMap.TryGetValue(assemblyName.ToLowerInvariant(), out _);
        }
        /// <summary>
        /// 传入的是 dll 的 name 和版本号
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsAssemblyLoaded(string assemblyName, Version _version)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException("Assembly name cannot be null or empty.", nameof(assemblyName));
            }

            if (_version == null)
            {
                throw new ArgumentNullException(nameof(_version), "Version cannot be null.");
            }

            return AssemblyVersionMap.TryGetValue(assemblyName.ToLowerInvariant(), out Version v) && v.Equals(_version);
        }
    }
}
