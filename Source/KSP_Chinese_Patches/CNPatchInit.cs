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
            if (!StaticMethods.IsAssemblyLoaded("0Harmony"))
            {
                Debug.Log("未发现安装有 0Harmony! DLL相关的汉化失效！");
                return;
            }

            Harmony har = new Harmony("tinygrox.ChinesePatches");
            if (StaticMethods.IsAssemblyLoaded("WhereCanIGo"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [Where Can I Go]! 应用翻译...");
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
            if (StaticMethods.IsAssemblyLoaded("SmartStage"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [Smart Stage]! 应用翻译...");
                har.Patch
                    (
                    original: AccessTools.Constructor(
                        AccessTools.TypeByName("SmartStage.AscentPlot"),
                        new[] {
                            typeof(List<>).MakeGenericType(AccessTools.TypeByName("SmartStage.Sample")),
                            typeof(List<>).MakeGenericType(AccessTools.TypeByName("SmartStage.StageDescription")),
                            typeof(int),
                            typeof(int)
                        }),
                    transpiler: new HarmonyMethod(typeof(SmartStagePatches), nameof(SmartStagePatches.AscentPlotLocPatch))
                    );

                var SmartStageType = AccessTools.TypeByName("SmartStage.MainWindow");

                har.Patch(
                    original: AccessTools.Method(SmartStageType, "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(SmartStagePatches), nameof(SmartStagePatches.MainWindow_OnGUILocPatch))
                    );

                har.Patch(
                    original: AccessTools.Method(SmartStageType, "drawStagesWindow", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(SmartStagePatches), nameof(SmartStagePatches.MainWindow_drawStagesWindowLocPatch))
                    );
                har.Patch(
                    original: AccessTools.Method(SmartStageType, "drawWindow", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(SmartStagePatches), nameof(SmartStagePatches.MainWindow_drawWindowLocPatch))
                    );
                har.Patch(
                    original: AccessTools.Method(SmartStageType, "<planets>m__0", new[] { typeof(CelestialBody) }),
                    transpiler: new HarmonyMethod(typeof(SmartStagePatches), nameof(SmartStagePatches.MainWindow_PlanetDisplayNamePatch))
                    );
            }
            Destroy(this);
        }
    }
}
