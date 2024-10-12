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
            if (StaticMethods.IsAssemblyLoaded("RealAntennas"))
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
            Destroy(this);
        }
    }
}
