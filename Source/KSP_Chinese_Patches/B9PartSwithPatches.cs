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
    public class B9PartSwithPatches : AbstractPatchBase
    {
        public override string PatchName => "B9 Part Switch";

        public override string PatchDLLName => "B9PartSwitch";

        public static IEnumerable<CodeInstruction> ModuleB9PartInfo_SetupGUI_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.InsertAndAdvance(
                // (base.Fields["showInfo"].uiControlEditor as UI_Toggle).disabledText = "隐藏"
                // (base.Fields["showInfo"].uiControlEditor as UI_Toggle).enabledText = "显示"
                // (base.Fields["showInfo"].uiControlFlight as UI_Toggle).disabledText = "隐藏"
                // (base.Fields["showInfo"].uiControlFlight as UI_Toggle).enabledText = "显示"
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showInfo"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlEditor))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, "隐藏"), // Hidden
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.disabledText))),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showInfo"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlEditor))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, "显示"), // Enabled
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.enabledText))),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showInfo"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlFlight))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, "隐藏"), // Hidden
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.disabledText))),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showInfo"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlFlight))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, "显示"), // Enabled
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.enabledText))),
                // base.Fields["showInfo"].guiName = "部件信息"
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showInfo"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "部件信息"), // Part Info
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName)))
                )
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Mass"))
                .SetOperandAndAdvance("质量")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Mass (Dry)"))
                .SetOperandAndAdvance("质量(干)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cost"))
                .SetOperandAndAdvance("成本")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cost (Dry)"))
                .SetOperandAndAdvance("成本(干")
                .InsertAndAdvance(
                // base.Fields["wetMass"].guiName = "质量(湿)"
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "wetMass"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "质量(湿)"), // Mass (Wet)
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),

                // base.Fields["wetCost"].guiName = "成本(湿)"
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "wetCost"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "成本(湿)"), // Cost (Wet)
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),

                // base.Fields["maxTemp"].guiName = "最高温度"
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "maxTemp"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "最高温度"), // Max Temp
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),

                // base.Fields["skinMaxTemp"].guiName = "表面最高温度"
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "skinMaxTemp"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "表面最高温度"), // Max Skin Temp
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),

                // base.Fields["crashTolerance"].guiName = "撞击可承受"
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "crashTolerance"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "撞击可承受"), // Crash Tolerance
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName)))
                );
            return matcher.InstructionEnumeration();
        }
        public override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo(
                    AccessTools.Method(AccessTools.TypeByName("B9PartSwitch.ModuleB9PartInfo"), "SetupGUI"),
                    new HarmonyMethod(typeof(B9PartSwithPatches), nameof(B9PartSwithPatches.ModuleB9PartInfo_SetupGUI_Patch)),
                    PatchType.Transpiler
                 )
            };
        }
    }
}
