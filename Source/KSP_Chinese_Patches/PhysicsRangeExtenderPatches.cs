﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class PhysicsRangeExtenderPatches
    {
        public static IEnumerable<CodeInstruction> Gui_DisableMod_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Disable Mod"))
                .SetOperandAndAdvance("禁用Mod")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Enable Mod"))
                .SetOperandAndAdvance("启用Mod");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> Gui_DrawCamFixMultiplier_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cam fix multiplier:"))
                .SetOperandAndAdvance("镜头修复系数:");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> Gui_DrawGlobalVesselRange_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Global range:"))
                .SetOperandAndAdvance("全局范围设置:");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> Gui_DrawSaveButton_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Apply new range"))
                .SetOperandAndAdvance("应用新范围");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> Gui_DrawTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Physics Range Extender"))
                .SetOperandAndAdvance("物理范围扩展");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> PhysicsRangeExtender_UpdateNearClipPlane_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender] Flickering correction is active, near camera plane is adapting."))
                .SetOperandAndAdvance("[PhysicsRangeExtender] 闪烁/摆动修正程序已激活，正在调整镜头近裁剪面。");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> PhysicsRangeExtender_UnloadLandedVessels_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[PhysicsRangeExtender] Unloading landed vessels during active orbital fly."))
                .SetOperandAndAdvance("[PhysicsRangeExtender] 进入轨道飞行，正在卸载已着陆地面载具");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> TerrainExtender_ShowMessageTerrainStatus_Patch(IEnumerable<CodeInstruction> codeInstructions)
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
    }
}