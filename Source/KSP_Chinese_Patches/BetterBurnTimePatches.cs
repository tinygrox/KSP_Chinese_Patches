using HarmonyLib;
using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class BetterBurnTimePatches
    {
        private static readonly string[] NEW_TIPS = new string[] { "正在计算更好的启动加速时间...", "正在预测撞击时间...", "正在预测接近时间...", "正在添加倒计时指示器..." };
        private static readonly string GeosyncLabel = "同步轨道";// "gsync";
        private static readonly string TargetsyncLabel = "目标同步轨道"; // "tsync";
        private static readonly string FormatSeconds = "{0}秒"; // "{0}s";
        private static readonly string FormatMinutesSeconds = "{0}分{1}秒"; // "{0}m{1}s";
        private static readonly string FormatHoursMinutesSeconds = "{0}时{1}分{2}秒"; // "{0}h{1}m{2}s";
        private static readonly string FormatHoursMinutes = "{0}时{1}分"; // "{0}h{1}m";
        private static readonly string FormatHours = "{0}时"; // "{0}h";
        public static IEnumerable<CodeInstruction> AtmosphereTracker_CalculateEntryTime_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Reentry"))
                .SetOperandAndAdvance("再入");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> AtmosphereTracker_CalculateExitTime_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Exit atm"))
                .SetOperandAndAdvance("脱离大气层");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> AtmosphereTracker_Recalculate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} in {1}"))
                .SetOperandAndAdvance("{1}后{0}");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> BetterBurnTime_LateUpdate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Est. Burn: "))
                .SetOperandAndAdvance("预估加速: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Est. Burn: "))
                .SetOperandAndAdvance("预估加速: ");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ClosestApproachTracker_LateUpdate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target@{0:F1}km in {1}"))
                .SetOperandAndAdvance("{1}后接近目标@{0:F1}km");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ImpactTracker_Recalculate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} in {1}"))
                .SetOperandAndAdvance("{1}后{0}");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ImpactTracker_CalculateTimeToImpact_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Impact"))
                .SetOperandAndAdvance("撞击")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Splash"))
                .SetOperandAndAdvance("溅落")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Touchdown"))
                .SetOperandAndAdvance("着陆"); // 或者 "点地"？
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ModuleEngineBurnTime_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "burnTimeDisplay"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "燃烧时间"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName)))
                );
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ModuleEngineBurnTime_GetModuleTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "SRB burn time"))
                .SetOperandAndAdvance("固推燃烧时间");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ModuleEngineBurnTime_GetPrimaryField_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<b>Burn time:</b> {0}"))
                .SetOperandAndAdvance("<b>燃烧时间:</b> {0}");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> SplashScreen_InsertTips_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.SplashScreen"), "NEW_TIPS")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.NEW_TIPS)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Thanking {0}..."))
                .SetOperandAndAdvance("鸣谢{0}...");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> GeosyncTracker_LateUpdate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.GeosyncTracker"), "GEOSYNC_LABEL")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(GeosyncLabel)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.GeosyncTracker"), "TARGETSYNC_LABEL")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(TargetsyncLabel)))
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> TimeFormatter_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "formatSeconds")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(FormatSeconds)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "formatSeconds")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(FormatSeconds)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "formatMinutesSeconds")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(FormatMinutesSeconds)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "formatHoursMinutesSeconds")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(FormatHoursMinutesSeconds)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "formatHoursMinutes")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(FormatHoursMinutes)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "formatHours")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(FormatHours)));
            return matcher.InstructionEnumeration();
        }
    }
}
