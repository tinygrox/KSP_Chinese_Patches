using HarmonyLib;
using KSP.IO;
using KSP.Localization;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSP_Chinese_Patches
{
    public class DPAIPatches : AbstractPatchBase
    {
        public static int GlyphMaxLength = 65535;

        public override string PatchName => "Docking Port Alignment Indicator";

        public override string PatchDLLName => "DockingPortAlignmentIndicator";

        private static IEnumerable<CodeInstruction> DPAI_loadTexturesPatches_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            //CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            CodeMatcher matcher = new CodeMatcher(codeInstructions);

            matcher.MatchStartForward(
                new CodeMatch(OpCodes.Ldstr, "DialogPlain.png"),
                new CodeMatch(OpCodes.Ldnull),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(File), nameof(File.ReadAllBytes)).MakeGenericMethod(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator")))
                );
            if (matcher.IsValid)
            {
                matcher.SetOperandAndAdvance("BitMapCNFont.png");
                matcher.Advance(1);
                matcher.SetOperandAndAdvance(AccessTools.Method(typeof(File), nameof(File.ReadAllBytes)).MakeGenericMethod(typeof(DPAIPatches)));
            }
            matcher.MatchStartForward(
                new CodeMatch(OpCodes.Ldstr, "DialogPlain.dat"),
                new CodeMatch(OpCodes.Ldnull),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(TextReader), nameof(TextReader.CreateForType)).MakeGenericMethod(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator")))
                );
            if (matcher.IsValid)
            {
                matcher.SetOperandAndAdvance("BitMapCNFont.dat");
                matcher.Advance(1);
                matcher.SetOperandAndAdvance(AccessTools.Method(typeof(TextReader), nameof(TextReader.CreateForType)).MakeGenericMethod(typeof(DPAIPatches)));
            }
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> DPAI_drawIndicatorContentsToTexture_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "DST"))
                .SetOperandAndAdvance("距离")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "CVEL"))
                .SetOperandAndAdvance("相对速度")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "CDST"))
                .SetOperandAndAdvance("对接距离");
            return matcher.InstructionEnumeration();
        }

        #region >>>> 这些 Patch 用于修改 DPAI 的 Glyph 数组长度
        // NavyFish.BitmapFont
        // yahei4096.dat - 27807
        //                          32768
        // 用 id 的值作为索引，我说怎么长度 32768 也不够
        private static IEnumerable<CodeInstruction> Bitmap_FontbuildGlyphs_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldc_I4, 0x100)).ThrowIfNotMatch("No Match for [Ldc_I4, 0x100]!")
                .SetOperandAndAdvance(GlyphMaxLength);
            return matcher.InstructionEnumeration();
        }
        // nmd 还有高手
        private static IEnumerable<CodeInstruction> Bitmap_getGlyphFromID_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldc_I4, 0xFF)).ThrowIfNotMatch("No Match for [Ldc_I4, 0xFF]!")
                .SetOperandAndAdvance(GlyphMaxLength);
            return matcher.InstructionEnumeration();
        }

        #endregion
        private static IEnumerable<CodeInstruction> DPAI_createBlizzyButton_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show/Hide Docking Port Alignment Indicator"))
                .SetOperandAndAdvance("显示/隐藏对接口对齐指示器");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> DPAI_determineTargetPortName_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No Vessel Targeted"))
                .SetOperandAndAdvance("未设置载具为目标")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target Out of Range"))
                .SetOperandAndAdvance("目标超出距离")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No Port Targeted"))
                .SetOperandAndAdvance("未设置对接口目标")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> DPAI_drawRenderedGaugeTexture_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Settings"))
                .SetOperandAndAdvance("设         置")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> DPAI_drawRPMText_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "TGT:"))
                .SetOperandAndAdvance("目标:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "REF:"))
                .SetOperandAndAdvance("参考系:")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> DPAI_drawSettingsWindowContents_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Display HUD Target Port Icon"))
                .SetOperandAndAdvance("显示目标对接口图标")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Display when using RPM"))
                .SetOperandAndAdvance("使用RPM时显示")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "HUD Target Port Icon Size:"))
                .SetOperandAndAdvance("目标对接口图标大小:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Enable Auto Targeting (and Cycling)"))
                .SetOperandAndAdvance("自动设置/切换目标")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Exclude Docked Ports"))
                .SetOperandAndAdvance("非对接口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Restrict Docking Ports"))
                .SetOperandAndAdvance("仅对接口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "GUI Scale:"))
                .SetOperandAndAdvance("界面缩放:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Invert Alignment X"))
                .SetOperandAndAdvance("反转对齐 X 轴")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Invert Translation X"))
                .SetOperandAndAdvance("反转平移 X 轴")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Invert Alignment Y"))
                .SetOperandAndAdvance("反转对齐 Y 轴")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Invert Translation Y"))
                .SetOperandAndAdvance("反转平移 Y 轴")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Invert Roll Direction"))
                .SetOperandAndAdvance("反转滚动方向")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Always use Stock Toolbar"))
                .SetOperandAndAdvance("总是使用原版图标栏");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> DPAI_getReferencePortName_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "None"))
                .SetOperandAndAdvance("无");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> DPAI_onGaugeDraw_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "DPAI Settings"))
                .SetOperandAndAdvance("DPAI设置");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModuleDockingNodeNamed_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher.InsertAndAdvance(
                // Fields["portName"].guiName = 对接口名称 // Port Name
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "portName"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "对接口名称"),
                //new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Localizer), nameof(Localizer.Format), new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),
                // Events["renameDockingPort"].guiName = 重命名对接口 // Rename Port
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Events))),
                new CodeInstruction(OpCodes.Ldstr, "renameDockingPort"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseEventList), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "重命名对接口"),
                //new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Localizer), nameof(Localizer.Format), new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseEvent), nameof(BaseEvent.guiName))));
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModuleDockingNodeNamed_DisplayForNode_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Rename Docking Port"))
                .SetOperandAndAdvance("重命名对接口");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModuleDockingNodeNamed_onRenameDialogDraw_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Name:"))
                .SetOperandAndAdvance("名称:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Done"))
                .SetOperandAndAdvance("完成");
            return matcher.InstructionEnumeration();
        }

        public override bool IsModLoaded
        {
            get
            {
                if (!StaticMethods.IsAssemblyLoaded(PatchDLLName) || !StaticMethods.IsAssemblyLoaded("moduledockingnodenamed"))
                {
                    Debug.Log($"[KSPCNPatches] 未检测到Mod [{PatchName}] 或 [moduledockingnodenamed] 已跳过");
                    return false;
                }
                return true;
            }
        }
        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "loadTextures"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_loadTexturesPatches_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawIndicatorContentsToTexture"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawIndicatorContentsToTexture_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.BitmapFont"), "buildGlyphs"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.Bitmap_FontbuildGlyphs_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.BitmapFont"), "getGlyphFromID", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.Bitmap_getGlyphFromID_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "createBlizzyButton"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_createBlizzyButton_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "determineTargetPortName"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_determineTargetPortName_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawRenderedGaugeTexture", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawRenderedGaugeTexture_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawRPMText", new[] { typeof(int), typeof(int) }),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawRPMText_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "drawSettingsWindowContents", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_drawSettingsWindowContents_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "getReferencePortName"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_getReferencePortName_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.DockingPortAlignmentIndicator"), "onGaugeDraw"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.DPAI_onGaugeDraw_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.ModuleDockingNodeNamed"), "OnAwake"),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.ModuleDockingNodeNamed_OnAwake_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.RenameWindow"), "DisplayForNode", new[] { AccessTools.TypeByName("NavyFish.ModuleDockingNodeNamed") }),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.ModuleDockingNodeNamed_DisplayForNode_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("NavyFish.RenameWindow"), "onRenameDialogDraw", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(DPAIPatches), nameof(DPAIPatches.ModuleDockingNodeNamed_onRenameDialogDraw_Patch)),
                    PatchType.Transpiler
                ),
            };
        }
    }
}
