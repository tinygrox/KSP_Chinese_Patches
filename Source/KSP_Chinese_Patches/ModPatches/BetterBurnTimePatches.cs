using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace KSP_Chinese_Patches.ModPatches
{
    public class BetterBurnTimePatches : AbstractPatchBase
    {
        private static readonly string[] NEW_TIPS = new string[] { "正在计算更好的启动加速时间...", "正在预测撞击时间...", "正在预测接近时间...", "正在添加倒计时指示器..." };
        private static readonly string GeosyncLabel = "同步轨道";// "gsync";
        private static readonly string TargetsyncLabel = "目标同步轨道"; // "tsync";
        private static readonly string FormatSeconds = "{0}秒"; // "{0}s";
        private static readonly string FormatMinutesSeconds = "{0}分{1}秒"; // "{0}m{1}s";
        private static readonly string FormatHoursMinutesSeconds = "{0}时{1}分{2}秒"; // "{0}h{1}m{2}s";
        private static readonly string FormatHoursMinutes = "{0}时{1}分"; // "{0}h{1}m";
        private static readonly string FormatHours = "{0}时"; // "{0}h";

        public override string PatchName => "BetterBurnTime";

        public override string PatchDLLName => "BetterBurnTime";

        private static IEnumerable<CodeInstruction> AtmosphereTracker_CalculateEntryTime_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Reentry"))
                .SetOperandAndAdvance("再入");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> AtmosphereTracker_CalculateExitTime_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Exit atm"))
                .SetOperandAndAdvance("脱离大气层");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> AtmosphereTracker_Recalculate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} in {1}"))
                .SetOperandAndAdvance("{1}后{0}");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> BetterBurnTime_LateUpdate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Est. Burn: "))
                .SetOperandAndAdvance("预估加速: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Est. Burn: "))
                .SetOperandAndAdvance("预估加速: ");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ClosestApproachTracker_LateUpdate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target@{0:F1}km in {1}"))
                .SetOperandAndAdvance("{1}后接近目标@{0:F1}km");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ImpactTracker_Recalculate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} in {1}"))
                .SetOperandAndAdvance("{1}后{0}");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ImpactTracker_CalculateTimeToImpact_Patch(IEnumerable<CodeInstruction> codeInstructions)
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

        private static IEnumerable<CodeInstruction> ModuleEngineBurnTime_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
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

        private static IEnumerable<CodeInstruction> ModuleEngineBurnTime_GetModuleTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "SRB burn time"))
                .SetOperandAndAdvance("固推燃烧时间");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ModuleEngineBurnTime_GetPrimaryField_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<b>Burn time:</b> {0}"))
                .SetOperandAndAdvance("<b>燃烧时间:</b> {0}");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> SplashScreen_InsertTips_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("BetterBurnTime.SplashScreen"), "NEW_TIPS")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.NEW_TIPS)))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Thanking {0}..."))
                .SetOperandAndAdvance("鸣谢{0}...");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> GeosyncTracker_LateUpdate_Patch(IEnumerable<CodeInstruction> codeInstructions)
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
        private static IEnumerable<CodeInstruction> TimeFormatter_Patch(IEnumerable<CodeInstruction> codeInstructions)
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

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.AtmosphereTracker"), "CalculateEntryTime", new[] { typeof(Vessel), typeof(string).MakeByRefType() }),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.AtmosphereTracker_CalculateEntryTime_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.AtmosphereTracker"), "CalculateExitTime", new[] { typeof(Vessel), typeof(string).MakeByRefType() }),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.AtmosphereTracker_CalculateExitTime_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.AtmosphereTracker"), "Recalculate"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.AtmosphereTracker_Recalculate_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.BetterBurnTime"), "LateUpdate"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.BetterBurnTime_LateUpdate_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ClosestApproachTracker"), "LateUpdate"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ClosestApproachTracker_LateUpdate_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ImpactTracker"), "Recalculate"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ImpactTracker_Recalculate_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ImpactTracker"), "CalculateTimeToImpact"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ImpactTracker_CalculateTimeToImpact_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ModuleEngineBurnTime"), "OnStart"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ModuleEngineBurnTime_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ModuleEngineBurnTime"), "GetModuleTitle"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ModuleEngineBurnTime_GetModuleTitle_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.ModuleEngineBurnTime"), "GetPrimaryField"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.ModuleEngineBurnTime_GetPrimaryField_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.SplashScreen"), "InsertTips", new[] { typeof(LoadingScreen.LoadingScreenState) }),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.SplashScreen_InsertTips_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.GeosyncTracker"), "LateUpdate"),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.GeosyncTracker_LateUpdate_Patch)),
                    HarmonyPatchType.Transpiler
                 ),
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("BetterBurnTime.TimeFormatter"), "format", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(BetterBurnTimePatches), nameof(BetterBurnTimePatches.TimeFormatter_Patch)),
                    HarmonyPatchType.Transpiler
                 )
            };
        }
    }
}
