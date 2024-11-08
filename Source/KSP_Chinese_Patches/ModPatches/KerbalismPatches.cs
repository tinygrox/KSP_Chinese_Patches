using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSP_Chinese_Patches.ModPatches
{
    /// <summary>
    /// 要对 Kerbalism 进行Patch要略微复杂一点，因为 Kerbalism 是动态加载的，所以要考虑 Patch 时机，Patch 执行早于加载 Kerbalism 导致 Harmony 会无法找到Kerbalism 方法，所以本项目先对 KerbalismBootstrap 进行 Patch，等待 Kerbalism 被动态加载后添加对 Kerbalism 的 Patch。
    /// </summary>
    public class KerbalismPatches : AbstractPatchBase
    {
        public override string PatchName => "KerbalismBootstrap";

        public override string PatchDLLName => "KerbalismBootstrap";
        public static IEnumerable<CodeInstruction> Bootstrap_Start_Patch(IEnumerable<CodeInstruction> codeInstructions, ILGenerator iL)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            Label label1 = iL.DefineLabel();
            Label label2 = iL.DefineLabel();

            matcher
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Ldloc_3),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(AssemblyLoader.LoadedAssembly), nameof(AssemblyLoader.LoadedAssembly.Load))),
                    new CodeMatch(OpCodes.Ldloc_3)
                )
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.ApplyKerbalismPatches))))
                ;
            return matcher.InstructionEnumeration();
        }

        #region Kerbalism 医疗舱 bug 修复补丁
        // 方法来源: https://github.com/Kerbalism/Kerbalism/pull/884

        public static IEnumerable<CodeInstruction> SickBay_Start_Patch(IEnumerable<CodeInstruction> codeInstructions, ILGenerator iL)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            Label ldarg_0 = iL.DefineLabel();
            Label ldci4_0 = iL.DefineLabel();

            matcher
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldc_I4_1), // <- 插入
                    new CodeMatch(OpCodes.Ldarg_0)
                ).AddLabels(new[] { ldarg_0 })
                .Advance(-1)
                .Set(OpCodes.Ldc_I4_0, null)
                .AddLabels(new[] { ldci4_0 })
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PartModule), nameof(PartModule.isEnabled))),
                    new CodeInstruction(OpCodes.Brfalse_S, ldci4_0),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("KERBALISM.Sickbay"), "running")),
                    new CodeInstruction(OpCodes.Br_S, ldarg_0)
                )
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> SickBay_Update_Patch(IEnumerable<CodeInstruction> codeInstructions, ILGenerator iL)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            Label ldarg_0 = iL.DefineLabel();
            Label ldci4_0 = iL.DefineLabel();

            matcher
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("KERBALISM.Sickbay"), "running")),
                    new CodeMatch(OpCodes.Ldarg_0)
                )
                .AddLabels(new[] { ldarg_0 })
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Br_S, ldarg_0),
                    new CodeInstruction(OpCodes.Ldc_I4_0)
                )
                .Advance(-1)
                .AddLabels(new[] { ldci4_0 })
                .Advance(-3)
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PartModule), nameof(PartModule.isEnabled))),
                    new CodeInstruction(OpCodes.Brfalse_S, ldci4_0)
                )
                ;
            return matcher.InstructionEnumeration();
        }

        #endregion
        public static IEnumerable<CodeInstruction> Monitor_Indicator_supplies_UseResourceDisplayNamePatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Ldstr, "{0,-18}\t{1}\t{2}"),
                    new CodeMatch(OpCodes.Ldloc_S)
                )
                .InsertAndAdvance
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
        public static IEnumerable<CodeInstruction> Sensor_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Call, AccessTools.Method(AccessTools.TypeByName("KERBALISM.Lib"), "SpacesOnCaps", new[] { typeof(string) })),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName)))
                )
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
                )
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> Telemetry_RenderEnvironment_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Ldc_I4_S),
                    new CodeMatch(OpCodes.Ldc_I4_S),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(string), nameof(string.Replace), new[] { typeof(char), typeof(char) })),
                    new CodeMatch(OpCodes.Ldarg_1)
                )
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
                )
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> DevManager_Devman_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            CodeMatch[] Device_Displayname = new CodeMatch[]
            {
                new CodeMatch(OpCodes.Ldloc_S),
                new CodeMatch(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("KERBALISM.DevManager+<>c__DisplayClass0_1"), "dev")),
                new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(AccessTools.TypeByName("KERBALISM.Device"), "DisplayName")),
                new CodeMatch(OpCodes.Ldloc_S)
            };

            matcher
                .MatchEndForward(Device_Displayname)
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
                )
                .MatchEndForward(Device_Displayname)
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
                )
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("KERBALISM.DevManager+<>c__DisplayClass0_3"), "dev")),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(AccessTools.TypeByName("KERBALISM.Device"), "DisplayName")),
                    new CodeMatch(OpCodes.Ldloc_S)
                )
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KerbalismPatches), nameof(KerbalismPatches.KerbalismTranslator), new[] { typeof(string) }))
                )
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ProcessDevice_Tooltip_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchEndForward(new CodeMatch(OpCodes.Ldstr, "Process capacity :"))
                .SetOperandAndAdvance("处理能力:")
                ;
            return matcher.InstructionEnumeration();
        }

        private static void ApplyKerbalismPatches()
        {
            if (GetHarmony == null) throw new ArgumentNullException($"[KerbalismPatch]HarmonyInstance is NULL");

            GetHarmony.Patch(original: AccessTools.Method(AccessTools.TypeByName("KERBALISM.Sickbay"), "Start"), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.SickBay_Start_Patch)));

            GetHarmony.Patch(original: AccessTools.Method(AccessTools.TypeByName("KERBALISM.Sickbay"), "Update"), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.SickBay_Update_Patch)));

            GetHarmony.Patch(original: AccessTools.Method(AccessTools.TypeByName("KERBALISM.Monitor"), "Indicator_supplies", new[] { AccessTools.TypeByName("KERBALISM.Panel"), typeof(Vessel), AccessTools.TypeByName("KERBALISM.VesselData") }), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.Monitor_Indicator_supplies_UseResourceDisplayNamePatch)));

            GetHarmony.Patch(original: AccessTools.Method(AccessTools.TypeByName("KERBALISM.Sensor"), "OnStart", new[] { typeof(PartModule.StartState) }), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.Sensor_OnStart_Patch)));

            GetHarmony.Patch(original: AccessTools.Method(AccessTools.TypeByName("KERBALISM.Telemetry"), "Render_environment", new[] { AccessTools.TypeByName("KERBALISM.Panel"), typeof(Vessel), AccessTools.TypeByName("KERBALISM.VesselData") }), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.Telemetry_RenderEnvironment_Patch)));

            GetHarmony.Patch(original: AccessTools.Method(AccessTools.TypeByName("KERBALISM.DevManager"), "Devman", new[] { AccessTools.TypeByName("KERBALISM.Panel"), typeof(Vessel) }), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.DevManager_Devman_Patch)));

            GetHarmony.Patch(original: AccessTools.PropertyGetter(AccessTools.TypeByName("KERBALISM.ProcessDevice"), "Tooltip"), transpiler: new HarmonyMethod(typeof(KerbalismPatches), nameof(KerbalismPatches.ProcessDevice_Tooltip_Patch)));
        }
        // 我知道很硬，那没办法了，为了性能
        private static Dictionary<string, string> LocDict = new Dictionary<string, string>()
        {
            ["radiation"] = "辐射",
            ["pressure"] = "气压",
            ["temperature"] = "温度",
            ["habitat radiation"] = "居所辐射",
            ["gravioli"] = "引力子",
            ["<size=1><color=#00000000>00</color></size>greenhouse"] = "<size=1><color=#00000000>00</color></size>温室",
            ["light"] = "照明设备",
            ["laboratory"] = "实验室",
            ["drill"] = "钻探",
            ["emitter"] = "辐射源",
            ["generator"] = "发电机",
            ["gravity ring"] = "重力环",
            ["data transmission"] = "数据传输",
            ["sickbay"] = "医疗舱",
            ["<size=1><color=#00000000>01</color></size>sickbay"] = "<size=1><color=#00000000>01</color></size>医疗舱",
            ["antenna"] = "天线"
        };
        private static string KerbalismTranslator(string KerbalismNonLocalString)
        {
            if (LocDict.TryGetValue(KerbalismNonLocalString.ToLowerInvariant(), out string translatedText))
                return translatedText;
            else
            {
                //UnityEngine.Debug.Log($"[KSPCNPatch] KerbalismString not in loc: \"{KerbalismNonLocalString}\"");
                return KerbalismNonLocalString;
            }
        }
        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("KerbalismBootstrap.Bootstrap"), "Start"),
                    new HarmonyMethod(typeof(KerbalismPatches), nameof(this.Bootstrap_Start_Patch)),
                    PatchType.Transpiler
                )
            };
        }
    }
}
