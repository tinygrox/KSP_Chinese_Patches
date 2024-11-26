using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System.Collections.Generic;

namespace KSP_Chinese_Patches.ModPatches
{
    public class RasterPropMonitorPatches : AbstractPatchBase
    {
        public override string PatchName => "RasterPropMonitor Core";

        public override string PatchDLLName => "RasterPropMonitor";

        private static IEnumerable<CodeInstruction> JSIExternalCameraSelector_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("visibleCameraName", "摄像机ID")) // Camera ID: 
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("showCones", "FOV 范围")) // FOV marker 
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("showCones", "关", "开", editorOnly: true))
                ;
            return matcher.InstructionEnumeration();
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("JSI.JSIExternalCameraSelector"), "OnAwake"),
                    new HarmonyMethod(typeof(RasterPropMonitorPatches), nameof(RasterPropMonitorPatches.JSIExternalCameraSelector_OnAwake_Patch)),
                    HarmonyPatchType.Transpiler
                )
            };
        }
    }
}
