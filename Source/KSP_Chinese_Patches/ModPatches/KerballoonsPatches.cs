using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches.ModPatches
{
    public class KerballoonsPatches : AbstractPatchBase
    {
        public override string PatchName => "KerBalloons Continued";

        public override string PatchDLLName => "KerBalloons";

        public static IEnumerable<CodeInstruction> KBModuleDataRecorder_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("recordingSeconds", "记录频率")) // Recording Frequency
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("recordingActive", "记录")) // Record
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("recordingActive", "关", "开"))
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("readoutInfo", "状态")) // Status
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> KBModuleEnviroSensor_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("readoutInfo", "数据")) // Data
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("Toggle", "开关显示")) // Toggle Display
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleKerBalloon_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("repackBalloon", "重新打包气球")) // Repack Balloon
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("inflateBalloon", "气球充气")) // Inflate Balloon
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("deflateBalloon", "气球放气")) // Deflate Balloon
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ShowInfo", "显示气球信息")) // Show Balloon Info
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ConfigureBalloon", "配置气球")) // Configure Balloon
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("inflateAction", "气球充气")) // Inflate Balloon
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("deflateAction", "气球放气")) // Deflate Balloon
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleKerBalloon_InfoBalloonWin_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Recommended Body: "))
                .SetOperandAndAdvance("建议天体: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nBalloon Size: Size"))
                .SetOperandAndAdvance("\n气球大小: 尺寸 ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nMin pressure: "))
                .SetOperandAndAdvance("\n最低气压: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nMax pressure: "))
                .SetOperandAndAdvance("\n最高气压: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nMax lift: "))
                .SetOperandAndAdvance("\n最大升力: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nMax payload ("))
                .SetOperandAndAdvance("\n最大载荷 (")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n  at Max pressure: "))
                .SetOperandAndAdvance("\n  于最高气压: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "t (at "))
                .SetOperandAndAdvance("t (于 ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n  at Min pressure: "))
                .SetOperandAndAdvance("\n  于最低气压: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "t (at "))
                .SetOperandAndAdvance("t (于 ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close"))
                .SetOperandAndAdvance("关闭")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleKerBalloon_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "See Balloon Info window for specifics"))
                .SetOperandAndAdvance("打开气球信息窗口查看更多信息")
                ;
            return matcher.InstructionEnumeration();
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("KerBalloons.KBModuleDataRecorder"),"OnAwake"),
                    new HarmonyMethod(typeof(KerballoonsPatches), nameof(this.KBModuleDataRecorder_OnAwake_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("KerBalloons.KBModuleEnviroSensor"),"OnAwake"),
                    new HarmonyMethod(typeof(KerballoonsPatches), nameof(this.KBModuleEnviroSensor_OnAwake_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("KerBalloons.ModuleKerBalloon"),"OnStart", new[]{ typeof(PartModule.StartState)}),
                    new HarmonyMethod(typeof(KerballoonsPatches), nameof(this.ModuleKerBalloon_OnStart_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("KerBalloons.ModuleKerBalloon"),"InfoBalloonWin", new[]{ typeof(int)}),
                    new HarmonyMethod(typeof(KerballoonsPatches), nameof(this.ModuleKerBalloon_InfoBalloonWin_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("KerBalloons.ModuleKerBalloon"),"GetInfo"),
                    new HarmonyMethod(typeof(KerballoonsPatches), nameof(this.ModuleKerBalloon_GetInfo_Patch)),
                    PatchType.Transpiler
                ),
            };
        }
    }
}
