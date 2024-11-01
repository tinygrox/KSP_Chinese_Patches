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

            matcher
                // (base.Fields["showInfo"].uiControlEditor as UI_Toggle).disabledText = "隐藏"
                // (base.Fields["showInfo"].uiControlEditor as UI_Toggle).enabledText = "显示"
                // (base.Fields["showInfo"].uiControlFlight as UI_Toggle).disabledText = "隐藏"
                // (base.Fields["showInfo"].uiControlFlight as UI_Toggle).enabledText = "显示"
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("showInfo", "隐藏", "显示"))
                // base.Fields["showInfo"].guiName = "部件信息"
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("showInfo", "部件信息"))
                // base.Fields["wetMass"].guiName = "质量(湿)"
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("wetMass", "质量(湿)"))
                // base.Fields["wetCost"].guiName = "成本(湿)"
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("wetCost", "成本(湿)"))
                // base.Fields["maxTemp"].guiName = "最高温度"
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("maxTemp", "最高温度"))
                // base.Fields["skinMaxTemp"].guiName = "表面最高温度"
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("skinMaxTemp", "表面最高温度"))
                // base.Fields["crashTolerance"].guiName = "撞击可承受"
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("crashTolerance", "撞击可承受"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Mass"))
                .SetOperandAndAdvance("质量")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Mass (Dry)"))
                .SetOperandAndAdvance("质量(干)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cost"))
                .SetOperandAndAdvance("成本")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cost (Dry)"))
                .SetOperandAndAdvance("成本(干)")
                ;
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
