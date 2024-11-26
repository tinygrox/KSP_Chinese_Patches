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
    public class IndicatorLightsPatches : AbstractPatchBase
    {
        public override string PatchName => "IndicatorLights";

        public override string PatchDLLName => "IndicatorLights";

        public static IEnumerable<CodeInstruction> ModuleCrewIndicatorToggle_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("status", "乘员LED")) // Crew LEDs
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("status", "关", "开"))
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("OnToggleAction", "开关乘员LED")) // Toggle Crew LEDs
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("OnActivateAction", "打开乘员LED")) // Activate Crew LEDs
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("OnDeactivateAction", "关闭乘员LED")) // Deactivate Crew LEDs
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleCustomBlink_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("blinkEnabled", "模式")) // Mode
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("blinkEnabled", "连续", "闪烁"))
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("onMillis", "毫秒 开")) // Milliseconds On
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("offMillis", "毫秒 关")) // Milliseconds On
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("phase", "相位")) // Milliseconds On
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ToggleAction", "切换闪烁")) // Toggle Blinking
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ActivateAction", "启用闪烁")) // Start Blinking
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("DeactivateAction", "禁用闪烁")) // Stop Blinking
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleCustomColoredEmissive_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, ": Red"))
                .SetOperandAndAdvance(": 红")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, ": Green"))
                .SetOperandAndAdvance(": 绿")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, ": Blue"))
                .SetOperandAndAdvance(": 蓝")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, ": Brightness"))
                .SetOperandAndAdvance(": 亮度")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleIndicatorToggle_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("status", "状态")) // Status
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("status", "关", "开"))
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleToggleLED_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("status", "关", "开", editorOnly: true))
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("axisGroup", "触发"))
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ToggleAction", "切换照明")) // Toggle Light
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ActivateAction", "打开照明")) // Turn Light On
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("DeactivateAction", "关闭照明")) // Turn Light Off
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleToggleLED_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "LED Status"))
                .SetOperandAndAdvance("LED状态")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ModuleIndicatorToggle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Status"))
                .SetOperandAndAdvance("状态")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "On"))
                .SetOperandAndAdvance("开启")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Off"))
                .SetOperandAndAdvance("关闭")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Toggle"))
                .SetOperandAndAdvance("开关")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Activate"))
                .SetOperandAndAdvance("激活")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Deactivate"))
                .SetOperandAndAdvance("禁用")
                ;
            return matcher.InstructionEnumeration();
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("IndicatorLights.ModuleCrewIndicatorToggle"), "OnStart", new[]{ typeof(PartModule.StartState)}),
                    new HarmonyMethod(typeof(IndicatorLightsPatches), nameof(ModuleCrewIndicatorToggle_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("IndicatorLights.ModuleCustomBlink"), "OnStart", new[]{ typeof(PartModule.StartState)}),
                    new HarmonyMethod(typeof(IndicatorLightsPatches), nameof(this.ModuleCustomBlink_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("IndicatorLights.ModuleCustomColoredEmissive"), "OnStart", new[]{ typeof(PartModule.StartState)}),
                    new HarmonyMethod(typeof(IndicatorLightsPatches), nameof(this.ModuleCustomColoredEmissive_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("IndicatorLights.ModuleIndicatorToggle"), "OnStart", new[]{ typeof(PartModule.StartState)}),
                    new HarmonyMethod(typeof(IndicatorLightsPatches), nameof(this.ModuleIndicatorToggle_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Constructor(AccessTools.TypeByName("IndicatorLights.ModuleIndicatorToggle")),
                    new HarmonyMethod(typeof(IndicatorLightsPatches), nameof(this.ModuleIndicatorToggle_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Constructor(AccessTools.TypeByName("IndicatorLights.ModuleToggleLED")),
                    new HarmonyMethod(typeof(IndicatorLightsPatches), nameof(this.ModuleToggleLED_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("IndicatorLights.ModuleToggleLED"), "OnAwake"),
                    new HarmonyMethod(typeof(IndicatorLightsPatches), nameof(this.ModuleToggleLED_OnAwake_Patch)),
                    HarmonyPatchType.Transpiler
                ),
            };
        }
    }
}
