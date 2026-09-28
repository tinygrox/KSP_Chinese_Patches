using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;

namespace KSP_Chinese_Patches.ModPatches;

public class CorrectCoLPatches : AbstractPatchBase
{
    public override KSPAddon.Startup ApplyTiming => KSPAddon.Startup.SpaceCentre;
    public override string PatchName => "Correct CoL";
    public override string PatchDLLName => "CorrectCoL";

    public static IEnumerable<CodeInstruction> CorrectCoLGraphWindow_OnGUIPatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Static stability analysis"))
            .ThrowIfInvalid("Nah..")
            .SetOperandAndAdvance("静态稳定性分析");

        return matcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction> CorrectCoLGraphWindow_stabilityReportPatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " is stable"))
            .ThrowIfInvalid("Nah..")
            .SetOperandAndAdvance("稳定")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " is partially stable"))
            .SetOperandAndAdvance("部分稳定")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " is unstable"))
            .SetOperandAndAdvance("不稳定")
            ;

        return matcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction> CorrectCoLGraphWindow_analyzeTraitsPatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "fueled craft pitch"))
            .ThrowIfInvalid("Nah..")
            .SetOperandAndAdvance("满油俯仰稳定性：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "dry craft pitch"))
            .SetOperandAndAdvance("空油俯仰稳定性：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "fueled craft yaw"))
            .SetOperandAndAdvance("满油偏航稳定性：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "dry craft yaw"))
            .SetOperandAndAdvance("空油偏航稳定性：")
            ;

        return matcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction> CorrectCoLGraphWindow_drawGUIPatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show stock marker"))
            .ThrowIfInvalid("Nah..")
            .SetOperandAndAdvance("显示原版标识")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "pitch"))
            .SetOperandAndAdvance("俯仰")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "yaw"))
            .SetOperandAndAdvance("偏航")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "side"))
            .SetOperandAndAdvance("侧滑")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Update"))
            .SetOperandAndAdvance("刷新")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Planet"))
            .SetOperandAndAdvance("星球")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Auto-update"))
            .SetOperandAndAdvance("自动刷新")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Update the graph after any change"))
            .SetOperandAndAdvance("每次修改后自动重新计算图表")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "AoA range:"))
            .SetOperandAndAdvance("攻角范围：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "AoA and sideslip range to plot, degrees"))
            .SetOperandAndAdvance("绘制攻角(AoA)和侧滑角的范围（单位：度）")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "AoA marks:"))
            .SetOperandAndAdvance("攻角刻度：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Horizontal axis marks step, degrees"))
            .SetOperandAndAdvance("横轴刻度间隔（单位：度）")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "AoA compress:"))
            .SetOperandAndAdvance("攻角压缩：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Zero for 1:1 AoA axis, positive for quadratic compression.\nHelps to focus on important stuff near zero AoA while not loosing large-AoA behaviour"))
            .SetOperandAndAdvance("0 表示攻角坐标轴为 1:1。\n大于 0 时采用二次压缩，可放大接近 0° 的细节，同时保留大攻角表现。")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Speed:"))
            .SetOperandAndAdvance("速度：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Speed towards root part nose direction, m/s.\nEnter negative values to analyze retrograde stability."))
            .SetOperandAndAdvance("沿飞行器根部朝向（机头方向）的速度（m/s）。\n输入负值可分析倒飞（逆行）稳定性。")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Altitude:"))
            .SetOperandAndAdvance("高度：")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Meters above sea level"))
            .SetOperandAndAdvance("海平面高度（米）")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Lift to Drag ratio"))
            .SetOperandAndAdvance("升阻比")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Shows the ratio of lift to drag, higher is better"))
            .SetOperandAndAdvance("显示升力与阻力的比值，数值越高表示气动效率越好。")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Colored vertical lines"))
            .SetOperandAndAdvance("彩色竖线")
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Shows AoA on which Lift equals -(gravity + centrifugal).\nGreen line displays required AoA for level flight at current mass.\nYellow line displays required AoA for level flight with a dry craft."))
            .SetOperandAndAdvance("表示升力等于 -(重力 + 离心力) 时对应的攻角。\n绿色：当前质量维持平飞所需攻角。\n黄色：燃料耗尽（干重）时维持平飞所需攻角。")
            ;

        return matcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction> CorrectPlanetSelection_OnGUIPatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Planetary Body Selection"))
            .ThrowIfInvalid("Nah..")
            .SetOperandAndAdvance("行星天体选择")
            ;

        return matcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction> CorrectPlanetSelection_planetSelWinPatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Selected planet:"))
            .ThrowIfInvalid("Nah..")
            .SetOperandAndAdvance("选择行星：")
            .MatchEndForward(
                new CodeMatch(OpCodes.Stloc_0),
                new CodeMatch(OpCodes.Ldloc_0),
                new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.name)))
            )
            .ThrowIfInvalid("[CorrectCoLPatches] CelestialBody.name Failed")
            .SetOperandAndAdvance(AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.displayName)))
            .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LingoonaGrammarExtensions), nameof(LingoonaGrammarExtensions.LocalizeRemoveGender))))
            .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close"))
            .SetOperandAndAdvance("关闭")
            ;

        return matcher.InstructionEnumeration();
    }

    protected override void LoadAllPatchInfo()
    {
        Patches = new HashSet<HarPatchInfo>();
        Type tGraphWindow = AccessTools.TypeByName("CorrectCoL.GraphWindow");
        Type tPlanetSelection = AccessTools.TypeByName("CorrectCoL.PlanetSelection");

        Patches.Add(new HarPatchInfo(AccessTools.Method(tGraphWindow, "OnGUI"),
            new HarmonyMethod(typeof(CorrectCoLPatches), nameof(CorrectCoLPatches.CorrectCoLGraphWindow_OnGUIPatch)), HarmonyPatchType.Transpiler));

        Patches.Add(new HarPatchInfo(AccessTools.Method(tGraphWindow, "_drawGUI", new[] { typeof(int) }),
            new HarmonyMethod(typeof(CorrectCoLPatches), nameof(CorrectCoLGraphWindow_drawGUIPatch)),
            HarmonyPatchType.Transpiler));

        Patches.Add(new HarPatchInfo(AccessTools.Method(tGraphWindow, "report_stability", new[] { typeof(string), typeof(float) }),
            new HarmonyMethod(typeof(CorrectCoLPatches), nameof(CorrectCoLGraphWindow_stabilityReportPatch)),
            HarmonyPatchType.Transpiler));

        Patches.Add(new HarPatchInfo(AccessTools.Method(tGraphWindow, "analyze_traits"),
            new HarmonyMethod(typeof(CorrectCoLPatches), nameof(CorrectCoLGraphWindow_analyzeTraitsPatch)),
            HarmonyPatchType.Transpiler));

        Patches.Add(new HarPatchInfo(AccessTools.Method(tPlanetSelection, "OnGUI"),
            new HarmonyMethod(typeof(CorrectCoLPatches), nameof(CorrectCoLPatches.CorrectPlanetSelection_OnGUIPatch)), HarmonyPatchType.Transpiler));

        Patches.Add(new HarPatchInfo(AccessTools.Method(tPlanetSelection, "planetSelWin", new[] { typeof(int) }),
            new HarmonyMethod(typeof(CorrectCoLPatches), nameof(CorrectPlanetSelection_planetSelWinPatch)),
            HarmonyPatchType.Transpiler));
    }
}
