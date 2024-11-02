using HarmonyLib;
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
    public class PlanetsideExplorationTechnologiesPatches : AbstractPatchBase
    {
        public override string PatchName => "Planetside Exploration Technologies";

        public override string PatchDLLName => "PlanetsideExplorationTechnologies";

        private static IEnumerable<CodeInstruction> ModulePETAnimation_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("statusDisplay", "状态"))
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ExtendEvent", "展开"))
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("RetractEvent", "收回"))
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("RepairEvent", "维修"))
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ToggleAction", "展开/收回"))
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("RetractAction", "收回"))
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ExtendAction", "展开"))
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETAnimation_UpdateFSM_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Extended"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_234828"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Extending..."))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_234841"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Retracted"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_234861"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Retracting..."))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_234856"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Broken!"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_234868"))
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETAnimation_Repair_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Turbine repaired with {0} repair kits"))
                .SetOperandAndAdvance("维修部件需要{0}个维修工具")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETAnimation_OnCollisionEnter_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Turbine was destroyed"))
                .SetOperandAndAdvance("风电部件已损毁")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETAnimation_Destroy_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Broken!"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_234868"))
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETAnimation_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Retracted"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_234861"))
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETTurbine_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("isActive", "开关风轮")) // Toggle Turbine
                .InsertAndAdvance(StaticMethods.Field_groupDisplayName_Instructions("isActive", "风力发电")) // Wind Turbine
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("isActive", "关", "开", flightOnly: true))

                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("flowRateDisplay", "产出电力")) // EC Output
                .InsertAndAdvance(StaticMethods.Field_groupDisplayName_Instructions("flowRateDisplay", "风力发电")) // Wind Turbine

                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("turbineEfficiencyDisplay", "发电效率")) // Efficiency
                .InsertAndAdvance(StaticMethods.Field_groupDisplayName_Instructions("turbineEfficiencyDisplay", "风力发电")) // Wind Turbine

                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("windSpeedDisplay", "当前风速")) // Current Wind Speed
                .InsertAndAdvance(StaticMethods.Field_groupDisplayName_Instructions("windSpeedDisplay", "风力发电")) // Wind Turbine

                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("windDirectionDisplay", "当前风向")) // Current Wind Direction
                .InsertAndAdvance(StaticMethods.Field_groupDisplayName_Instructions("windDirectionDisplay", "风力发电")) // Wind Turbine

                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("toggleLines", "显示风向")) // Show Wind Direction
                .InsertAndAdvance(StaticMethods.Field_groupDisplayName_Instructions("toggleLines", "风力发电")) // Wind Turbine
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("toggleLines", "关", "开", flightOnly: true))

                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ToggleEVATurbine", "开关风轮")) // Toggle Turbine

                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ForceWindUpdate", "调试: 强制刷新")) // Debug: Force Update

                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ToggleTurbine", "开关风轮")) // Toggle Turbine

                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("EnableTurbine", "启动风轮")) // Enable Turbine

                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("DisableTurbine", "关闭风轮")) // Disable Turbine
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ModulePETTurbine_UpdateTurbine_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Too little wind to generate power!"))
                .SetOperandAndAdvance("当前风速不足以发电!")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Generating Power..."))
                .SetOperandAndAdvance("正在发电...")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Turbine destroyed by excessive wind speed"))
                .SetOperandAndAdvance("风轮因风速过大而损坏")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Idling - Not generating power"))
                .SetOperandAndAdvance("闲置 - 未在发电")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETTurbine_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Minimum Wind Speed: {0} % \n"))
                .SetOperandAndAdvance("最低风速: {0} % \n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Rated for: max. {0} % \n"))
                .SetOperandAndAdvance("风速范围: 最高{0} % \n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Turbine Type: "))
                .SetOperandAndAdvance("风电类型: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Turbine Type: Tracking \n"))
                .SetOperandAndAdvance("风电类型: 追踪 \n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Turbine Type: Static \n"))
                .SetOperandAndAdvance("风电类型: 固定 \n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<color=orange>Can be repaired with {0} repair kits"))
                .SetOperandAndAdvance("<color=orange>维修需消耗{0}个维修工具")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ModulePETTurbine_GetModuleDisplayName_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Deployable Wind Turbine"))
                .SetOperandAndAdvance("可展开风力发电机")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Wind Turbine"))
                .SetOperandAndAdvance("风力发电机")
                ;
            return matcher.InstructionEnumeration();
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Constructor(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETAnimation")),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETAnimation_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETAnimation"), "OnStart", new[] { typeof(PartModule.StartState) }),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETAnimation_OnStart_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETAnimation"), "UpdateFSM"),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETAnimation_UpdateFSM_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETAnimation"), "Repair"),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETAnimation_Repair_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETAnimation"), "OnCollisionEnter", new[] { typeof(Collision) }),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETAnimation_OnCollisionEnter_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETAnimation"), "Destroy"),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETAnimation_Destroy_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETTurbine"), "OnAwake"),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETTurbine_OnAwake_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETTurbine"), "UpdateTurbine"),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETTurbine_UpdateTurbine_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETTurbine"), "GetInfo"),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETTurbine_GetInfo_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("PlanetsideExplorationTechnologies.Modules.ModulePETTurbine"), "GetModuleDisplayName"),
                    new HarmonyMethod(typeof(PlanetsideExplorationTechnologiesPatches), nameof(PlanetsideExplorationTechnologiesPatches.ModulePETTurbine_GetModuleDisplayName_Patch)),
                    PatchType.Transpiler
                )
            };
        }
    }
}
