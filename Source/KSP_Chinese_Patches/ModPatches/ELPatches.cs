using HarmonyLib;
using KSP.Localization;
using KSP_Chinese_Patches.PatchesInfo;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace KSP_Chinese_Patches.ModPatches
{
    public class ELPatches : AbstractPatchBase
    {
        public override string PatchName => "Extraplanetary Launchpads";
        public override string PatchDLLName => "Launchpad";
        // 资源显示翻译
        private static IEnumerable<CodeInstruction> ELResourceLine_Resource_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(AccessTools.TypeByName("ExtraplanetaryLaunchpads.IResourceLine"), "ResourceName"))
                )
                .Advance(1)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ELPatches), nameof(ELPatches.getResourceName), new[] { typeof(string) }))
                )
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ELControlReference_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("MakeReference", Localizer.Format("#autoLOC_6001360"))) // Control From Here
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ToggleReference", "切换基准")) // Toggle Reference
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ELConverter_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("furnaceTemp", "熔炉温度")) // Furnace Temp
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELConverter_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " at 50% efficiency"))
                .SetOperandAndAdvance("处于50%效率工作")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n\n<color=#bada55>Mass flow: {0:0.00} {1}/{2}</color>"))
                .SetOperandAndAdvance("\n\n<color=#bada55>质流量: {0:0.00} {1}/{2}</color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n\n<color=#bada55>Heat flow: {0:0.00} {1}/{2}</color>"))
                .SetOperandAndAdvance("\n\n<color=#bada55>热流量: {0:0.00} {1}/{2}</color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n\n<color=#bada55>Inputs:</color>"))
                .SetOperandAndAdvance("\n\n<color=#bada55>输入:</color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n<color=#bada55>Outputs:</color>"))
                .SetOperandAndAdvance("\n<color=#bada55>输出:</color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "broken configuration"))
                .SetOperandAndAdvance("出错的配置")
                ;
            return matcher.InstructionEnumeration();
        }

        private static void GetModuleDisplayNamePostfixPatch(ref string __result)
        {
            __result = "EL转换装置";
        }

        private static IEnumerable<CodeInstruction> ELConverter_PostProcess_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "% eff."))
                .SetOperandAndAdvance("% 效率")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Operating"))
                .SetOperandAndAdvance("运转中")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELDisposablePad_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("PadName", "发射台名称")) // Pad name
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("HideUI", "隐藏界面")) // Hide UI
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ShowUI", "显示界面")) // Show UI
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ShowRenameUI", "重命名")) // Rename
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELConstructionSkill_GetDescription_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "She"))
                .SetOperandAndAdvance("她")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "He"))
                .SetOperandAndAdvance("他")
                .MatchStartForward(new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELConstructionSkill"), "skills")))
                .SetOperandAndAdvance(AccessTools.Field(typeof(ELPatches), nameof(ELPatches.cnskills)))
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELExtendingLaunchClamp_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("ReleaseClamp", "松开爪钩")) // Release Clamp
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("Release", "松开爪钩")) // Release Clamp
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELExtractor_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n\n<color=#bada55>Inputs:</color>"))
                .SetOperandAndAdvance("\n\n<color=#bada55>输入:</color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n<color=#bada55>Outputs:</color>"))
                .SetOperandAndAdvance("\n<color=#bada55>输出:</color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n<color=#bada55>Requirements:</color>"))
                .SetOperandAndAdvance("\n<color=#bada55>需要:</color>")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELExtractor_PostProcess_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "stalled"))
                .SetOperandAndAdvance("关机")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELExtractor_PrepareRecipe_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "no ground contact"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_259789"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "insufficient abundance"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_259819"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Inactive"))
                .SetOperandAndAdvance(Localizer.Format("#autoLOC_259826"))
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELLaunchpad_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("PadName", "发射台名称")) // Pad name
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("HideUI", "隐藏界面")) // Hide UI
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ShowUI", "显示界面")) // Show UI
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ShowRenameUI", "重命名")) // Rename
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELLaunchpad_GetModuleTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "EL Launchpad"))
                .SetOperandAndAdvance("EL 发射台")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELLaunchpad_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Launchpad"))
                .SetOperandAndAdvance("发射台")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELRecycler_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("status", "状态")) // State
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("Activate", "启动回收装置")) // Activate Recycler
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("Deactivate", "关闭回收装置")) // Deactivate Recycler
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELRecycler_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Recycler:\n"))
                .SetOperandAndAdvance("回收装置:\n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "rate: {0:G2}t/s"))
                .SetOperandAndAdvance("速率: {0:G2}t/s")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELRecycler_GetPrimaryField_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Recycling Rate: {0:G2}t/s"))
                .SetOperandAndAdvance("回收速率: {0:G2}t/s")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELRecycler_Activate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Active"))
                .SetOperandAndAdvance("激活")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELRecycler_Deactivate_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Inactive"))
                .SetOperandAndAdvance("关闭")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELSurveySkill_GetDescription_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "She"))
                .SetOperandAndAdvance("她")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "He"))
                .SetOperandAndAdvance("他")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} can use survey sites out to {1}m."))
                .SetOperandAndAdvance("{0}可以使用靠近{1}m内的测量站.")
                ;
            return matcher.InstructionEnumeration();
        }

        // 资源采用displayName
        private static IEnumerable<CodeInstruction> EL_Utils_PrintResource_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldarg_1),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(ResourceRatio), nameof(ResourceRatio.ResourceName)))
                )
                .Advance(1)
                .SetOpcodeAndAdvance(OpCodes.Ldloc_0)
                .SetAndAdvance(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(PartResourceDefinition), nameof(PartResourceDefinition.displayName)))

                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "day"))
                .SetOperandAndAdvance("天")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "hr"))
                .SetOperandAndAdvance("时")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "m"))
                .SetOperandAndAdvance("分")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "s"))
                .SetOperandAndAdvance("秒")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> EL_Utils_TimeSpanString_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(
                    new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(AccessTools.TypeByName("ExtraplanetaryLaunchpads.EL_Utils"), "time_formats"))
                )
                .SetOperandAndAdvance(AccessTools.Field(typeof(ELPatches), nameof(ELPatches.time_formats)))
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELSurveyStation_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("StationName", "发射台名称")) // Pad Name
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("range", "范围")) // Range
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("HideUI", "隐藏界面")) // Hide UI
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ShowUI", "显示界面")) // Show UI
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("ShowRenameUI", "重命名")) // Rename
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELSurveyStation_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Survey Station"))
                .SetOperandAndAdvance("测量站")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELSurveyStation_GetModuleTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "EL Survey Station"))
                .SetOperandAndAdvance("EL 测量站")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELSurveyStake_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("RenameVessel", "重命名桩")) // Rename Stake
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELSurveyStake_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Survey Stake"))
                .SetOperandAndAdvance("测量桩")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELSurveyStake_GetModuleTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "EL Survey Stake"))
                .SetOperandAndAdvance("EL 测量桩")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELTarget_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("SetAsTarget", Localizer.Format("#autoLOC_6001448"))) // Set as Target
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("UnsetTarget", Localizer.Format("#autoLOC_6001449"))) // Unset Target
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELTarget_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Targetable"))
                .SetOperandAndAdvance("可设置为目标")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ELTarget_GetModuleTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "EL Target"))
                .SetOperandAndAdvance("EL 目标")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ELWorkshop_OnStart_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("_Productivity", "生产率")) // Productivity
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("VesselProductivity", "载具生产率")) // Vessel Productivity
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ELWorkshop_GetInfo_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Workshop: productivity factor {0:G2}"))
                .SetOperandAndAdvance("工作车间: 生产率系数 {0:G2}")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ELWorkshop_GetInfoGetPrimaryField_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Productivity Factor: {0:G2}"))
                .SetOperandAndAdvance("生产率系数 {0:G2}")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ELWorkshop_GetModuleTitle_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "EL Workshop"))
                .SetOperandAndAdvance("EL 工作车间")
                ;
            return matcher.InstructionEnumeration();
        }
        // FSM - 有限状态机
        private static IEnumerable<CodeInstruction> RecyclerFSM_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Off"))
                .SetOperandAndAdvance("关闭")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Idle"))
                .SetOperandAndAdvance("闲置")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Captured Idle"))
                .SetOperandAndAdvance("闲置就绪")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Processing Part"))
                .SetOperandAndAdvance("处理部件")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Transferring Resources"))
                .SetOperandAndAdvance("输送资源")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Enabled"))
                .SetOperandAndAdvance("启用")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Disabled"))
                .SetOperandAndAdvance("禁用")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Enter Field"))
                .SetOperandAndAdvance("进场")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Part Selected"))
                .SetOperandAndAdvance("部件已选取")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Resources Collected"))
                .SetOperandAndAdvance("资源已收集")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Resources Transferred"))
                .SetOperandAndAdvance("资源已输送")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Parts Exhausted"))
                .SetOperandAndAdvance("部件已耗尽")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> RecyclerFSM_get_CurrentState_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "FSM not started"))
                .SetOperandAndAdvance("FSM未启动")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ELShipInfoWindow_SetResource_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward
                (
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("ExtraplanetaryLaunchpads.BuildResource"), "name"))
                )
                .Advance(1)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ELPatches), nameof(ELPatches.getResourceName), new[] { typeof(string) }))
                )
                ;
            return matcher.InstructionEnumeration();
        }

        private static string[] time_formats = new[] { "{0:F0}年{1:000}天{2:00}时{3:00}分{4:00}秒", "{1:F0}天{2:00}时{3:00}分{4:00}秒", "{2:F0}时{3:00}分{4:00}秒", "{3:F0}分{4:00}秒", "{4:F0}秒" };

        private static string[] cnskills = new[] {
            "可以在完备的工作车间中工作。", // " can work in a fully equipped workshop."
            "可以在任何工作车间中工作。", // " can work in any workshop."
            "可以为完备的工作车间增加产出。", // " is always productive in a fully equipped workshop."
            "可以为任何工作车间增加产出", // " is always productive in any workshop."
            // 纳闷了这还是英文吗，给我写哪去了
            "可以使任何工作车间的工作人员增加技能", // " enables skilled workers in any workshop."
            "可以使非技术工作人员在完备的工作车间增加技能", // " enables unskilled workers in a fully equipped workshop."
        };
        private static string getResourceName(string resourceName)
        {
            PartResourceDefinition res = PartResourceLibrary.Instance.GetDefinition(resourceName);
            if (res != null)
            {
                return res.displayName;
            }
            return resourceName;
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELResourceLine"), "Resource", new[] { AccessTools.TypeByName("ExtraplanetaryLaunchpads.IResourceLine") }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELResourceLine_Resource_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELControlReference"), "OnStart"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELControlReference_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELConverter"), "OnStart"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELConverter_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELConverter"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELConverter_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELConverter"), "GetModuleDisplayName"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.GetModuleDisplayNamePostfixPatch)),
                    HarmonyPatchType.Postfix
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELConverter"), "PostProcess", new[] { typeof(ConverterResults), typeof(double) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELConverter_PostProcess_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELDisposablePad"), "OnAwake"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELDisposablePad_OnAwake_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELConstructionSkill"), "GetDescription"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELConstructionSkill_GetDescription_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELExtendingLaunchClamp"), "OnAwake"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELExtendingLaunchClamp_OnAwake_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELExtractor"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELExtractor_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELExtractor"), "PostProcess", new[] { typeof(ConverterResults), typeof(double) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELExtractor_PostProcess_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELExtractor"), "PrepareRecipe", new[] { typeof(double) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELExtractor_PrepareRecipe_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELLaunchpad"), "OnAwake"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELLaunchpad_OnAwake_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELLaunchpad"), "GetModuleTitle"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELLaunchpad_GetModuleTitle_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELLaunchpad"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELLaunchpad_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELRecycler"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELRecycler_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELRecycler"), "GetPrimaryField"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELRecycler_GetPrimaryField_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELRecycler"), "OnStart", new[] { typeof(PartModule.StartState) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELRecycler_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELRecycler"), "Activate"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELRecycler_Activate_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELRecycler"), "Deactivate"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELRecycler_Deactivate_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELSurveySkill"), "GetDescription"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELSurveySkill_GetDescription_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.EL_Utils"), "PrintResource", new[] { typeof(StringBuilder), typeof(ResourceRatio), typeof(string) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.EL_Utils_PrintResource_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.EL_Utils"), "TimeSpanString", new[] { typeof(double) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.EL_Utils_TimeSpanString_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELSurveyStake"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELSurveyStake_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELSurveyStake"), "GetModuleTitle"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELSurveyStake_GetModuleTitle_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELSurveyStake"), "OnStart", new[] { typeof(PartModule.StartState) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELSurveyStake_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELSurveyStation"), "OnAwake"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELSurveyStation_OnAwake_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELSurveyStation"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELSurveyStation_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELSurveyStation"), "GetModuleTitle"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELSurveyStation_GetModuleTitle_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELTarget"), "OnStart", new[] { typeof(PartModule.StartState) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELTarget_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELTarget"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELTarget_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELTarget"), "GetModuleTitle"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELTarget_GetModuleTitle_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELWorkshop"), "OnStart", new[] { typeof(PartModule.StartState) }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELWorkshop_OnStart_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELWorkshop"), "GetInfo"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELWorkshop_GetInfo_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELWorkshop"), "GetPrimaryField"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELWorkshop_GetInfoGetPrimaryField_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELWorkshop"), "GetModuleTitle"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELWorkshop_GetModuleTitle_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Constructor(AccessTools.TypeByName("RecyclerFSM"), new[]{ AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELRecycler") }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.RecyclerFSM_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.PropertyGetter(AccessTools.TypeByName("RecyclerFSM"), "CurrentState"),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.RecyclerFSM_get_CurrentState_Patch)),
                    HarmonyPatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELShipInfoWindow"), "SetResource",new[] { AccessTools.TypeByName("ExtraplanetaryLaunchpads.ELTextInfoLine"), AccessTools.TypeByName("ExtraplanetaryLaunchpads.BuildResource") }),
                    new HarmonyMethod(typeof(ELPatches), nameof(ELPatches.ELShipInfoWindow_SetResource_Patch)),
                    HarmonyPatchType.Transpiler
                ),
            };
        }
    }
}
