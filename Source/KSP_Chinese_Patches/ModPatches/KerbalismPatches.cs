using HarmonyLib;
using KSP.UI.TooltipTypes;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace KSP_Chinese_Patches.ModPatches;

/// <summary>
/// 要对 Kerbalism 进行Patch要略微复杂一点，因为 Kerbalism 是动态加载的，所以要考虑 Patch 时机，Patch 执行早于加载 Kerbalism 导致 Harmony 会无法找到Kerbalism 方法，所以本项目先对 KerbalismBootstrap 进行 Patch，等待 Kerbalism 被动态加载后添加对 Kerbalism 的 Patch。
/// Kerbalism 于 3.41 版本进行了大量代码重构（甚至会影响存档），此处代码也相应做出调整
/// </summary>
public class KerbalismPatches : AbstractPatchBase
{
    public override string PatchName => "Kerbalism"; // "KerbalismBootstrap";

    public override string PatchDLLName => "Kerbalism"; //"KerbalismBootstrap";

    // public static IEnumerable<CodeInstruction> Bootstrap_Start_Patch(IEnumerable<CodeInstruction> codeInstructions, ILGenerator iL)
    // {
    //     CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
    //
    //     Label label1 = iL.DefineLabel();
    //     Label label2 = iL.DefineLabel();
    //
    //     matcher
    //         .MatchEndForward
    //         (
    //             new CodeMatch(OpCodes.Ldloc_S),
    //             new CodeMatch(OpCodes.Callvirt,
    //                 AccessTools.Method(typeof(AssemblyLoader.LoadedAssembly),
    //                     nameof(AssemblyLoader.LoadedAssembly.Load))),
    //             new CodeMatch(OpCodes.Nop),
    //             new CodeMatch(OpCodes.Nop)
    //         ).ThrowIfInvalid("KerbalismBootstrap_Start_Patch: loadedAssembly.Load() not match")
    //         .InsertAndAdvance(new CodeInstruction(OpCodes.Call,
    //             AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.ApplyKerbalismPatches))))
    //         ;
    //     return matcher.InstructionEnumeration();
    // }

    public static IEnumerable<CodeInstruction> Monitor_Indicator_supplies_UseResourceDisplayNamePatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        // => PartResourceLibrary.Instance.GetDefinition(supply.resource).displayName
        matcher
            .MatchEndForward
            (
                new CodeMatch(OpCodes.Ldstr, "{0,-18}\t{1}\t{2}"),
                new CodeMatch(OpCodes.Ldloc_S)
            )
            .InsertAndAdvance // 在 Ldloc_S 之前插入，别记错了
            (
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartResourceLibrary), nameof(PartResourceLibrary.Instance)))
            )
            .Advance(2)
            .InsertAndAdvance
            (
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(PartResourceLibrary), nameof(PartResourceLibrary.GetDefinition), new[] { typeof(string) })),

                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(PartResourceDefinition), nameof(PartResourceDefinition.displayName)))
            )
            ;
        return matcher.InstructionEnumeration();
    }

    // 汉化那些 device 的，现在最新的 Kerbalism 好像补全了一部分 Device 的 DisplayName，暂时不需要
    // GetHarmony.Patch(original: AccessTools.Method(AccessTools.TypeByName("KERBALISM.DevManager"), "Devman", new[] { AccessTools.TypeByName("KERBALISM.Panel"), typeof(Vessel) }), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.DevManager_Devman_Patch)));
    // public static IEnumerable<CodeInstruction> DevManager_Devman_Patch(IEnumerable<CodeInstruction> codeInstructions)
    // {
    //     CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
    //
    //     CodeMatch[] Device_Displayname = new CodeMatch[]
    //     {
    //         new CodeMatch(OpCodes.Ldloc_S),
    //         new CodeMatch(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("KERBALISM.DevManager+<>c__DisplayClass0_1"), "dev")),
    //         new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(AccessTools.TypeByName("KERBALISM.Device"), "DisplayName")),
    //         new CodeMatch(OpCodes.Ldloc_S)
    //     };
    //
    //     matcher
    //         .MatchEndForward(Device_Displayname)
    //         .InsertAndAdvance
    //         (
    //             new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
    //         )
    //         .MatchEndForward(Device_Displayname)
    //         .InsertAndAdvance
    //         (
    //             new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
    //         )
    //         .MatchEndForward
    //         (
    //             new CodeMatch(OpCodes.Ldloc_S),
    //             new CodeMatch(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("KERBALISM.DevManager+<>c__DisplayClass0_3"), "dev")),
    //             new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(AccessTools.TypeByName("KERBALISM.Device"), "DisplayName")),
    //             new CodeMatch(OpCodes.Ldloc_S)
    //         )
    //         .InsertAndAdvance
    //         (
    //             new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
    //         )
    //         ;
    //     return matcher.InstructionEnumeration();
    // }
    public static IEnumerable<CodeInstruction> TooltipController_CrewAC_SetTooltip_PostfixPatch(IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher
            .MatchEndForward(new CodeMatch(OpCodes.Ldstr, "<b>Career "))
            .SetOperandAndAdvance("<b>职业生涯")
            ;
        return matcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction> VesselRecovery_OnVesselRecovered_RecoverScienceDataPatch(
        IEnumerable<CodeInstruction> codeInstructions)
    {
        CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

        matcher
            .MatchEndForward(new CodeMatch(OpCodes.Ldstr, "subject value"))
            .SetOperandAndAdvance("科研项目价值")
            .MatchEndForward(new CodeMatch(OpCodes.Ldstr, "in RnD"))
            .SetOperandAndAdvance("处于研发中心")
            .MatchEndForward(new CodeMatch(OpCodes.Ldstr, "earned"))
            .SetOperandAndAdvance("科学点数")
            ;

        return matcher.InstructionEnumeration();
    }

    // private static Dictionary<string, string> LocDict = new Dictionary<string, string>()
    // {
    //     ["radiation"] = "辐射",
    //     ["pressure"] = "气压",
    //     ["temperature"] = "温度",
    //     ["habitat radiation"] = "居所辐射",
    //     ["gravioli"] = "引力子",
    //     ["<size=1><color=#00000000>00</color></size>greenhouse"] = "<size=1><color=#00000000>00</color></size>温室",
    //     ["light"] = "照明设备",
    //     ["laboratory"] = "实验室",
    //     ["drill"] = "钻探",
    //     ["emitter"] = "辐射源",
    //     ["generator"] = "发电机",
    //     ["gravity ring"] = "重力环",
    //     ["data transmission"] = "数据传输",
    //     ["sickbay"] = "医疗舱",
    //     ["<size=1><color=#00000000>01</color></size>sickbay"] = "<size=1><color=#00000000>01</color></size>医疗舱",
    //     ["antenna"] = "天线"
    // };
    // private static string KerbalismTranslator(string KerbalismNonLocalString)
    // {
    //     if (LocDict.TryGetValue(KerbalismNonLocalString.ToLowerInvariant(), out string translatedText))
    //         return translatedText;
    //     else
    //     {
    //         //UnityEngine.Debug.Log($"[KSPCNPatch] KerbalismString not in loc: \"{KerbalismNonLocalString}\"");
    //         return KerbalismNonLocalString;
    //     }
    // }
    protected override void LoadAllPatchInfo()
    {
        Patches = new HashSet<HarPatchInfo>
        {
            new HarPatchInfo
            (
                AccessTools.Method(AccessTools.TypeByName("KERBALISM.Monitor"), "Indicator_supplies", new[] { AccessTools.TypeByName("KERBALISM.Panel"), typeof(Vessel), AccessTools.TypeByName("KERBALISM.VesselData") }),
                new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.Monitor_Indicator_supplies_UseResourceDisplayNamePatch)),
                HarmonyPatchType.Transpiler
            ),
            new HarPatchInfo
            (
                AccessTools.Method(AccessTools.TypeByName("KERBALISM.TooltipController_CrewAC_SetTooltip"), "Postfix", new[] { typeof(TooltipController_CrewAC), typeof(ProtoCrewMember) }),
                new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.TooltipController_CrewAC_SetTooltip_PostfixPatch)),
                HarmonyPatchType.Transpiler
            ),
            new HarPatchInfo
            (
                AccessTools.Method(AccessTools.TypeByName("KERBALISM.VesselRecovery_OnVesselRecovered"), "RecoverScienceData", new []{ AccessTools.TypeByName("KERBALISM.KsmScienceData"), typeof(ProtoPartModuleSnapshot), typeof(ProtoVessel), typeof(bool), typeof(double).MakeByRefType() }),
                new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.VesselRecovery_OnVesselRecovered_RecoverScienceDataPatch)),
                HarmonyPatchType.Transpiler
            )
        };
    }
}
