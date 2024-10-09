using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace KSP_Chinese_Patches
{
    [KSPAddon(KSPAddon.Startup.Instantly, once: true)]
    public class CNPatchInit : MonoBehaviour
    {
        public void Start()
        {
            Harmony har = new Harmony("tinygrox.ChinesePatches");
            List<AssemblyLoader.LoadedAssembly> assemblies = new List<AssemblyLoader.LoadedAssembly>();
            foreach (AssemblyLoader.LoadedAssembly loadedAssembly in AssemblyLoader.loadedAssemblies)
            {
                assemblies.Add(loadedAssembly);
            }
            if (AssemblyLoader.loadedAssemblies.Any(a => a.assembly.GetName().Name == "WhereCanIGo"))
            {
                Debug.Log("Found Where Can I Go!");
                har.Patch(
                    //original: AccessTools.TypeByName("WhereCanIGo.WhereCanIGoEditor").GetMethod("GenerateDialog", BindingFlags.NonPublic),
                    original: AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoEditor"), "GenerateDialog"),
                    transpiler: new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)));
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoEditor"), "GetDeltaVString"),
                    transpiler: new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)));

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoFlight"), "GenerateDialog"),
                    transpiler: new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)));
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoFlight"), "GetDeltaVString"),
                    transpiler: new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)));

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.Utilities"), "SituationValid"),
                    transpiler: new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)));
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.Utilities"), "VesselStatus"),
                    transpiler: new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)));
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.Utilities"), "GetTextColor"),
                    transpiler: new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)));
            }
            Destroy(this);
        }
    }
}
