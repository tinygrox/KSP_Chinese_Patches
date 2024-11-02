using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class PhysicsRangeExtenderPatches : AbstractPatchBase
    {
        public override string PatchName => "Physics Range Extender";

        public override string PatchDLLName => "PhysicsRangeExtender";

        private static IEnumerable<CodeInstruction> Gui_DisableMod_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Disable Mod"))
                .SetOperandAndAdvance("禁用Mod")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Enable Mod"))
                .SetOperandAndAdvance("启用Mod");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> Gui_DrawCamFixMultiplier_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cam fix multiplier:"))
                .SetOperandAndAdvance("镜头修复系数:");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> Gui_DrawGlobalVesselRange_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Global range:"))
                .SetOperandAndAdvance("全局范围设置:");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> Gui_DrawSaveButton_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Apply new range"))
                .SetOperandAndAdvance("应用新范围");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> Gui_DrawTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Physics Range Extender"))
                .SetOperandAndAdvance("物理范围扩展");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PhysicsRangeExtender_UpdateNearClipPlane_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender] Flickering correction is active, near camera plane is adapting."))
                .SetOperandAndAdvance("[PhysicsRangeExtender] 闪烁/摆动修正程序已激活，正在调整镜头近裁剪面。");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PhysicsRangeExtender_UnloadLandedVessels_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender] Unloading landed vessels during active orbital fly."))
                .SetOperandAndAdvance("[PhysicsRangeExtender] 进入轨道飞行，正在卸载已着陆地面载具");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> TerrainExtender_ShowMessageTerrainStatus_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender]Extending terrain: focusing landed vessels."))
                .SetOperandAndAdvance("[PhysicsRangeExtender]扩展地形: 居中镜头到已着陆载具。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender]Extending terrain: focusing landed vessels."))
                .SetOperandAndAdvance("[PhysicsRangeExtender]扩展地形: 居中镜头到已着陆载具。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender]Extending terrain: lifting vessels."))
                .SetOperandAndAdvance("[PhysicsRangeExtender]扩展地形: 正在发射载具。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender]Extending terrain: landing vessels."))
                .SetOperandAndAdvance("[PhysicsRangeExtender]扩展地形: 正在降落载具。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender]Extending terrain: switching to previous vessel."))
                .SetOperandAndAdvance("[PhysicsRangeExtender]扩展地形: 正在切换至上一个载具。")
                ;
            return matcher.InstructionEnumeration();
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DisableMod", new[] { typeof(float) }),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DisableMod_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawCamFixMultiplier", new[] { typeof(float) }),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawCamFixMultiplier_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawGlobalVesselRange", new[] { typeof(float) }),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawGlobalVesselRange_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawSaveButton", new[] { typeof(float) }),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawSaveButton_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.Gui"), "DrawTitle"),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.Gui_DrawTitle_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.PhysicsRangeExtender"), "UpdateNearClipPlane"),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.PhysicsRangeExtender_UpdateNearClipPlane_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.PhysicsRangeExtender"), "UnloadLandedVessels"),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.PhysicsRangeExtender_UnloadLandedVessels_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PhysicsRangeExtender.TerrainExtender"), "ShowMessageTerrainStatus"),
                    new HarmonyMethod(typeof(PhysicsRangeExtenderPatches), nameof(PhysicsRangeExtenderPatches.TerrainExtender_ShowMessageTerrainStatus_Patch)),
                    PatchType.Transpiler
                ),
            };
        }
    }
}
