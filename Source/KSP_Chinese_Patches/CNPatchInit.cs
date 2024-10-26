using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace KSP_Chinese_Patches
{
    [KSPAddon(KSPAddon.Startup.Instantly, once: true)]
    public class CNPatchInit : MonoBehaviour
    {
        public void Start()
        {
            //Debug.Log(string.Join("，", Font.GetOSInstalledFontNames()));
#if DEBUG
            //foreach (var a in AssemblyLoader.loadedAssemblies)
            foreach (var a in StaticMethods.AssemblyVersionMap)
            {
                //StaticMethods.sb.AppendLine($"[KSPCNPatches] DLL: {a.name} | DLLName: {a.dllName} | Version: ({a.versionMajor}.{a.versionMinor}.{a.versionRevision})");
                StaticMethods.sb.AppendLine($"[KSPCNPatches] DLLName: {a.Key} | Version: ({a.Value.Major}.{a.Value.Minor}.{a.Value.Build}.{a.Value.Revision})");
            }
            Debug.Log(StaticMethods.sb);
#endif
            if (!StaticMethods.IsAssemblyLoaded("0Harmony"))
            {
                Debug.Log("[KSPCNPatches] 未发现安装有 Harmony2! DLL相关的汉化失效！");
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
            if (StaticMethods.IsAssemblyLoaded("RealAntennas", new Version(2, 6, 0)))
            {
                Debug.Log("[KSPCNPatches] 已找到 [RealAntennas]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "OnAwake"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.OnAwakePatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]OnAwake 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "RecalculateFields"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RecalculateFieldsLocPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]RecalculateFields 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "PermanentShutdownEvent"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PermanentShutdownEventLocPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]PermanentShutdownEvent 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "ApplyTLColoring"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ApplyTLColoringLocPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]ApplyTLColoring 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "GetInfo"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.GetInfoLocPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]GetInfo 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_OnGUILocPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_GUIDisplayLocPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI+<>c"), "<GUIDisplay>b__44_0", new[] { typeof(CelestialBody) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_GetDisplayNamePatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]<GUIDisplay>b__44_0 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "RenderPanel"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_RenderPanelPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]RenderPanel 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "FireOnce"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_FireOncePatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]FireOnce 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.RACommNetScenario+<NotifyDisabled>d__35"), "MoveNext"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_NotifyDisabledPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]NotifyDisabled 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.RealAntennasUI"), "WindowGUI", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RealAntennasUI_WindowGUIPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]WindowGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.TechLevelInfo"), "ToString"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.TechLevelInfo_ToStringPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]TechLevelInfo 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.MapUI.NetUIConfigurationWindow"), "WindowGUI", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.NetUIConfigurationWindow_WindowGUIPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]NetUIConfigurationWindow_WindowGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.MapUI.SignalToolTipController"), "UpdateList"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.UpdateList_Patch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]UpdateList 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Network.ConnectionDebugger"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ConnectionDebugger_OnGUI_Patch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]ConnectionDebugger_OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Network.ConnectionDebugger"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ConnectionDebugger_GUIDisplay_Patch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]ConnectionDebugger_GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.AntennaTargetGUI"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.AntennaTargetGUI_OnGUI_Patch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]AntennaTargetGUI_OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.AntennaTargetGUI"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.AntennaTargetGUI_GUIDisplay_Patch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]AntennaTargetGUI_GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.RemoteAntennaControlUI"), "OnGUI"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RemoteAntennaControlUI_OnGUI_Patch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]RemoteAntennaControlUI_OnGUI 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.RemoteAntennaControlUI"), "GUIDisplay", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RemoteAntennaControlUI_GUIDisplay_Patch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]RemoteAntennaControlUI_GUIDisplay 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("RealAntennas.Antenna.Encoder"), "ToString"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.Encoder_ToStringPatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]Encoder_ToString 已应用！");

                har.Patch(
                    original: AccessTools.PropertyGetter(AccessTools.TypeByName("RealAntennas.RAParameters"), "Title"),
                    transpiler: new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RA_SettingTitlePatch)));
                Debug.Log("\t[KSPCNPatches] [RealAntennas]RealAntennas.RAParameters get_Title 已应用！");

            }
            if (StaticMethods.IsAssemblyLoaded("BetterBurnTime"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [BetterBurnTime]! 应用翻译...");
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.AtmosphereTracker"), "CalculateEntryTime", new[] { typeof(Vessel), typeof(string).MakeByRefType() }),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.AtmosphereTracker_CalculateEntryTime_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]AtmosphereTracker_CalculateEntryTime 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.AtmosphereTracker"), "CalculateExitTime", new[] { typeof(Vessel), typeof(string).MakeByRefType() }),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.AtmosphereTracker_CalculateExitTime_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]AtmosphereTracker_CalculateExitTime 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.AtmosphereTracker"), "Recalculate"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.AtmosphereTracker_Recalculate_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]AtmosphereTracker_Recalculate 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.BetterBurnTime"), "LateUpdate"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.BetterBurnTime_LateUpdate_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]BetterBurnTime_LateUpdate 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ClosestApproachTracker"), "LateUpdate"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ClosestApproachTracker_LateUpdate_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]ClosestApproachTracker_LateUpdate 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ImpactTracker"), "Recalculate"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ImpactTracker_Recalculate_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]ImpactTracker_Recalculate 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ImpactTracker"), "CalculateTimeToImpact"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ImpactTracker_CalculateTimeToImpact_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]ImpactTracker_CalculateTimeToImpact 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ModuleEngineBurnTime"), "OnStart"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ModuleEngineBurnTime_OnStart_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]ModuleEngineBurnTime_OnStart 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ModuleEngineBurnTime"), "GetModuleTitle"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ModuleEngineBurnTime_GetModuleTitle_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]ModuleEngineBurnTime_GetModuleTitle 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ModuleEngineBurnTime"), "GetPrimaryField"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ModuleEngineBurnTime_GetPrimaryField_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]ModuleEngineBurnTime_GetPrimaryField 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.SplashScreen"), "InsertTips", new[] { typeof(LoadingScreen.LoadingScreenState) }),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.SplashScreen_InsertTips_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]SplashScreen_InsertTips_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.GeosyncTracker"), "LateUpdate"),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.GeosyncTracker_LateUpdate_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]GeosyncTracker_LateUpdate 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "format", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.TimeFormatter_Patch)));
                Debug.Log("\t[KSPCNPatches] [BetterBurnTime]TimeFormatter_format 已应用！");
            }
            if (StaticMethods.IsAssemblyLoaded("B9PartSwitch"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [B9PartSwitch]! 应用翻译...");
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("B9PartSwitch.ModuleB9PartInfo"), "SetupGUI"),
                    transpiler: new HarmonyMethod(typeof(B9PartSwithPatches), nameof(B9PartSwithPatches.ModuleB9PartInfo_SetupGUI_Patch)));
                Debug.Log("\t[KSPCNPatches] [B9PartSwitch]ModuleB9PartInfo_SetupGUI 已应用！");
            }
            if (StaticMethods.IsAssemblyLoaded("PhysicsRangeExtender"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [PhysicsRangeExtender]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DisableMod", new[] { typeof(float) }),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DisableMod_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]DisableMod已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawCamFixMultiplier", new[] { typeof(float) }),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawCamFixMultiplier_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]DrawCamFixMultiplier已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawGlobalVesselRange", new[] { typeof(float) }),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawGlobalVesselRange_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]DrawGlobalVesselRange已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawSaveButton", new[] { typeof(float) }),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawSaveButton_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]DrawSaveButton已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawTitle"),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawTitle_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]DrawTitle已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.PhysicsRangeExtender"), "UpdateNearClipPlane"),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.PhysicsRangeExtender_UpdateNearClipPlane_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]UpdateNearClipPlane 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.PhysicsRangeExtender"), "UnloadLandedVessels"),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.PhysicsRangeExtender_UnloadLandedVessels_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]UnloadLandedVessels 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.TerrainExtender"), "ShowMessageTerrainStatus"),
                    transpiler: new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.TerrainExtender_ShowMessageTerrainStatus_Patch)));
                Debug.Log("\t[KSPCNPatches] [PhysicsRangeExtender]ShowMessageTerrainStatus 已应用！");
            }
            if (StaticMethods.IsAssemblyLoaded("[x]_Science!"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [[x] Science!]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.Body"), "FigureOutType"),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.Body_FigureOutType_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]Body_FigureOutType_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Constructor(AccessTools.TypeByName("ScienceChecklist.HelpWindow"), new[] { AccessTools.TypeByName("ScienceChecklist.ScienceChecklistAddon") }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.HelpWindow_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]HelpWindow_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.HelpWindow"), "DrawWindowContents", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.HelpWindow_DrawWindowContents_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]HelpWindow_DrawWindowContents_Patch 已应用！");

                har.Patch(
                    original: AccessTools.PropertyGetter(AccessTools.TypeByName("ScienceChecklist.ScienceInstance"), "Description"),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ScienceInstance_Description_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ScienceInstance_Description_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.ScienceWindow"), "Draw"),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ScienceWindow_Draw_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ScienceWindow_Draw_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.ScienceWindow"), "DrawControls", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ScienceWindow_DrawControls_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ScienceWindow_DrawControls_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.ScienceWindow"), "DrawTitleBarButtons", new[] { typeof(Rect), typeof(bool) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ScienceWindow_DrawTitleBarButtons_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ScienceWindow_DrawTitleBarButtons_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Constructor(AccessTools.TypeByName("ScienceChecklist.SettingsWindow"), new[] { AccessTools.TypeByName("ScienceChecklist.ScienceChecklistAddon") }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.SettingsWindow_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]SettingsWindow_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.SettingsWindow"), "DrawWindowContents", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.SettingsWindow_DrawWindowContents_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]SettingsWindow_DrawWindowContents_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Constructor(AccessTools.TypeByName("ScienceChecklist.ShipStateWindow"), new[] { AccessTools.TypeByName("ScienceChecklist.ScienceChecklistAddon") }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ShipStateWindow_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ShipStateWindow_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.ShipStateWindow"), "DrawBody"),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ShipStateWindow_DrawBody_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ShipStateWindow_DrawBody_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.ShipStateWindow"), "DrawVessel"),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ShipStateWindow_DrawVessel_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ShipStateWindow_DrawVessel_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Constructor(AccessTools.TypeByName("ScienceChecklist.Situation"), new[] { AccessTools.TypeByName("ScienceChecklist.Body"), typeof(ExperimentSituations), typeof(string), typeof(string) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.Situation_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ShipStateWindow_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.Situation"), "ToString", new[] { typeof(ExperimentSituations) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.Situation_ToString_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]Situation_ToString_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Constructor(AccessTools.TypeByName("ScienceChecklist.StatusWindow"), new[] { AccessTools.TypeByName("ScienceChecklist.ScienceChecklistAddon") }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.StatusWindow_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]StatusWindow_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.StatusWindow"), "DrawExperiment", new[] { AccessTools.TypeByName("ScienceChecklist.ScienceInstance"), typeof(Rect) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.StatusWindow_DrawExperiment_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]StatusWindow_DrawExperiment_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.StatusWindow"), "DrawWindowContents", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.StatusWindow_DrawWindowContents_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]StatusWindow_DrawWindowContents_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.StatusWindow"), "MakeSituationToolTip"),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.StatusWindow_MakeSituationToolTip_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]StatusWindow_MakeSituationToolTip_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ScienceChecklist.StatusWindow"), "UpdateSituation"),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.StatusWindow_UpdateSituation_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]StatusWindow_UpdateSituation_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Constructor(AccessTools.TypeByName("ScienceChecklist.xResourceData"), new[] { typeof(string) }),
                    transpiler: new HarmonyMethod(typeof(xSciencePatches), nameof(xSciencePatches.ResourcesName_Patch)));
                Debug.Log("\t[KSPCNPatches] [[x] Science!]ResourcesName_Patch 已应用！");
            }

            #region * DPAI 翻译相关
            if (StaticMethods.IsAssemblyLoaded("DockingPortAlignmentIndicator"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [DockingPortAlignmentIndicator]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "loadTextures"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_loadTexturesPatches_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_loadTexturesPatches_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawIndicatorContentsToTexture"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawIndicatorContentsToTexture_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_drawIndicatorContentsToTexture_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.BitmapFont"), "buildGlyphs"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.Bitmap_FontbuildGlyphs_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]Bitmap_FontbuildGlyphs_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.BitmapFont"), "getGlyphFromID", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.Bitmap_getGlyphFromID_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]Bitmap_getGlyphFromID_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "createBlizzyButton"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_createBlizzyButton_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_createBlizzyButton_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "determineTargetPortName"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_determineTargetPortName_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_determineTargetPortName_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawRenderedGaugeTexture", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawRenderedGaugeTexture_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_drawRenderedGaugeTexture_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawRPMText", new[] { typeof(int), typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawRPMText_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_drawRPMText_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawSettingsWindowContents", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawSettingsWindowContents_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_drawSettingsWindowContents_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "getReferencePortName"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_getReferencePortName_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_getReferencePortName_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "onGaugeDraw"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_onGaugeDraw_Patch)));
                Debug.Log("\t[KSPCNPatches] [DockingPortAlignmentIndicator]DPAI_onGaugeDraw_Patch 已应用！");
            }
            if (StaticMethods.IsAssemblyLoaded("moduledockingnodenamed"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [moduledockingnodenamed]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.ModuleDockingNodeNamed"), "OnAwake"),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.ModuleDockingNodeNamed_OnAwake_Patch)));
                Debug.Log("\t[KSPCNPatches] [moduledockingnodenamed]ModuleDockingNodeNamed_OnAwake_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.RenameWindow"), "DisplayForNode", new[] { AccessTools.TypeByName("NavyFish.ModuleDockingNodeNamed") }),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.ModuleDockingNodeNamed_DisplayForNode_Patch)));
                Debug.Log("\t[KSPCNPatches] [moduledockingnodenamed]ModuleDockingNodeNamed_DisplayForNode_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("NavyFish.RenameWindow"), "onRenameDialogDraw", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.ModuleDockingNodeNamed_onRenameDialogDraw_Patch)));
                Debug.Log("\t[KSPCNPatches] [moduledockingnodenamed]ModuleDockingNodeNamed_onRenameDialogDraw_Patch 已应用！");
            }
            #endregion

            if (StaticMethods.IsAssemblyLoaded("HullcamVDSContinued"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [HullcamVDSContinued]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterBlackAndWhiteFilm"), "OptionControls"),
                    transpiler: new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterBlackAndWhiteFilm_OptionControls_Patch)));
                Debug.Log("\t[KSPCNPatches] [HullcamVDSContinued]CameraFilterBlackAndWhiteFilm_OptionControls_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterColorFilm"), "OptionControls"),
                    transpiler: new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterColorFilm_OptionControls_Patch)));
                Debug.Log("\t[KSPCNPatches] [HullcamVDSContinued]CameraFilterColorFilm_OptionControls_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterColorHiResTV"), "OptionControls"),
                    transpiler: new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterColorHiResTV_OptionControls_Patch)));
                Debug.Log("\t[KSPCNPatches] [HullcamVDSContinued]CameraFilterColorHiResTV_OptionControls_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterColorLoResTV"), "OptionControls"),
                    transpiler: new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterColorLoResTV_OptionControls_Patch)));
                Debug.Log("\t[KSPCNPatches] [HullcamVDSContinued]CameraFilterColorLoResTV_OptionControls_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterNightVision"), "OptionControls"),
                    transpiler: new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterNightVision_OptionControls_Patch)));
                Debug.Log("\t[KSPCNPatches] [HullcamVDSContinued]CameraFilterNightVision_OptionControls_Patch 已应用！");
            }

            if (StaticMethods.IsAssemblyLoaded("RasterPropMonitor"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [RasterPropMonitor]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("JSI.JSIExternalCameraSelector"), "OnAwake"),
                    transpiler: new HarmonyMethod(typeof(RasterPropMonitorPatches), nameof(RasterPropMonitorPatches.JSIExternalCameraSelector_OnAwake_Patch)));
                Debug.Log("\t[KSPCNPatches] [RasterPropMonitor]JSIExternalCameraSelector_OnAwake_Patch 已应用！");
            }
            if (StaticMethods.IsAssemblyLoaded("ThroughTheEyes", new Version(2, 0, 4)))
            {
                Debug.Log("[KSPCNPatches] 已找到 [ThroughTheEyes]! 执行修复 Patch...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("FirstPerson.EVABoundFix"), "Hook"),
                    transpiler: new HarmonyMethod(typeof(ThroughTheEyesPatches), nameof(ThroughTheEyesPatches.EVABoundFix_Hook_Patch)));
                Debug.Log("\t[KSPCNPatches] [ThroughTheEyes]EVABoundFix_Hook_Patch 已应用！");
            }
            if (StaticMethods.IsAssemblyLoaded("AvionicsSystems"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [AvionicsSystems]! 应用翻译...");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("AvionicsSystems.MASFlightComputerProxy"), "BodyBiome", new[] { typeof(object), typeof(double), typeof(double) }),
                    transpiler: new HarmonyMethod(typeof(AvionicsSystemsPatches), nameof(AvionicsSystemsPatches.MASFlightComputerProxy_BodyBiome_Patch)));
                Debug.Log("\t[KSPCNPatches] [AvionicsSystems]MASFlightComputerProxy_BodyBiome_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("AvionicsSystems.MASFlightComputerProxy"), "BodyName", new[] { typeof(object) }),
                    transpiler: new HarmonyMethod(typeof(AvionicsSystemsPatches), nameof(AvionicsSystemsPatches.MASFlightComputerProxy_BodyName_Patch)));
                Debug.Log("\t[KSPCNPatches] [AvionicsSystems]MASFlightComputerProxy_BodyName_Patch 已应用！");

                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("AvionicsSystems.MASVesselComputer"), "UpdateTarget"),
                    transpiler: new HarmonyMethod(typeof(AvionicsSystemsPatches), nameof(AvionicsSystemsPatches.MASVesselComputer_UpdateTarget_Patch)));
                Debug.Log("\t[KSPCNPatches] [AvionicsSystems]MASVesselComputer_UpdateTarget_Patch 已应用！");
            }

            if (StaticMethods.IsAssemblyLoaded("ResourceOverview"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [ResourceOverview]! 应用翻译...");
                har.Patch(
                    original: AccessTools.Constructor(AccessTools.TypeByName("ResourceOverview.ResourceOverview")),
                    transpiler: new HarmonyMethod(typeof(ResourceOverviewPatches), nameof(ResourceOverviewPatches.ResourceOverview_Patch)));
                Debug.Log("\t[KSPCNPatches] [ResourceOverview]ResourceOverview_Patch 已应用！");
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ResourceOverview.ResourceOverview"), "drawGui", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(ResourceOverviewPatches), nameof(ResourceOverviewPatches.ResourceOverview_drawGui_Patch)));
                Debug.Log("\t[KSPCNPatches] [ResourceOverview]ResourceOverview_drawGui_Patch 已应用！");
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ResourceOverview.SettingsWindow"), "drawGui", new[] { typeof(int) }),
                    transpiler: new HarmonyMethod(typeof(ResourceOverviewPatches), nameof(ResourceOverviewPatches.SettingWindow_drawGui_Patch)));
                Debug.Log("\t[KSPCNPatches] [ResourceOverview]SettingWindow_drawGui_Patch 已应用！");

            }
            if (StaticMethods.IsAssemblyLoaded("ResearchBodies"))
            {
                Debug.Log("[KSPCNPatches] 已找到 [ResearchBodies]! 应用翻译...");
                har.Patch(
                    original: AccessTools.Method(AccessTools.TypeByName("ResearchBodies.ResearchBodiesController"), "FoundBody", new[] { typeof(int), typeof(CelestialBody), typeof(bool).MakeByRefType(), typeof(CelestialBody).MakeByRefType() }),
                    transpiler: new HarmonyMethod(typeof(ResearchBodiesPatches), nameof(ResearchBodiesPatches.ResearchBodies_FoundBody_Patch)));
                Debug.Log("\t[KSPCNPatches] [ResearchBodies]ResearchBodies_FoundBody_Patch 已应用！");
            }
            Destroy(this);
        }
    }
}
