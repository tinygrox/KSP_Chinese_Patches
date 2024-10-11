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
            if (StaticMethods.IsAssemblyLoaded("RealAntennas"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [RealAntennas]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "OnAwake"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.OnAwakePatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]OnAwake 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "RecalculateFields"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RecalculateFieldsLocPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]RecalculateFields 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "PermanentShutdownEvent"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PermanentShutdownEventLocPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]PermanentShutdownEvent 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "ApplyTLColoring"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ApplyTLColoringLocPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]ApplyTLColoring 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "GetInfo"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.GetInfoLocPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]GetInfo 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_OnGUILocPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_GUIDisplayLocPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI+<>c"), "<GUIDisplay>b__44_0", new[] { typeof(CelestialBody) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_GetDisplayNamePatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]<GUIDisplay>b__44_0 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "RenderPanel"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_RenderPanelPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]RenderPanel 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "FireOnce"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_FireOncePatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]FireOnce 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.RACommNetScenario+<NotifyDisabled>d__35"), "MoveNext"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_NotifyDisabledPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]NotifyDisabled 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.RealAntennasUI"), "WindowGUI", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RealAntennasUI_WindowGUIPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]WindowGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.TechLevelInfo"), "ToString"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.TechLevelInfo_ToStringPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]TechLevelInfo 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.MapUI.NetUIConfigurationWindow"), "WindowGUI", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.NetUIConfigurationWindow_WindowGUIPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]NetUIConfigurationWindow_WindowGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.MapUI.SignalToolTipController"), "UpdateList"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.UpdateList_Patch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]UpdateList 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Network.ConnectionDebugger"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ConnectionDebugger_OnGUI_Patch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]ConnectionDebugger_OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Network.ConnectionDebugger"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ConnectionDebugger_GUIDisplay_Patch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]ConnectionDebugger_GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.AntennaTargetGUI"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.AntennaTargetGUI_OnGUI_Patch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]AntennaTargetGUI_OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.AntennaTargetGUI"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.AntennaTargetGUI_GUIDisplay_Patch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]AntennaTargetGUI_GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.RemoteAntennaControlUI"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RemoteAntennaControlUI_OnGUI_Patch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]RemoteAntennaControlUI_OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.RemoteAntennaControlUI"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RemoteAntennaControlUI_GUIDisplay_Patch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]RemoteAntennaControlUI_GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Antenna.Encoder"), "ToString"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.Encoder_ToStringPatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]Encoder_ToString 已应用！");

                har.Patch(
                    original: AccessTools.PropertyGetter(AccessTools.TypeByName("RealAntennas.RAParameters"), "Title"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RA_SettingTitlePatch)));
                Debug.Log("[KSPCNPatches] [RealAntennas]RealAntennas.RAParameters get_Title 已应用！");

            }
            Destroy(this);
        }
    }
}
