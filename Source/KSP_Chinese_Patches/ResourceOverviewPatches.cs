using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class ResourceOverviewPatches
    {
        public static IEnumerable<CodeInstruction> ResourceOverview_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No command pod"))
                .SetOperandAndAdvance("无指令舱")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Has command seat"))
                .SetOperandAndAdvance("有指令椅")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "ProbeCore"))
                .SetOperandAndAdvance("探测器核心")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Resource Overview"))
                .SetOperandAndAdvance("资源总览")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ResourceOverview_drawGui_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Vessel Type:"))
                .SetOperandAndAdvance("载具类型：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Total Mass:"))
                .SetOperandAndAdvance("总质量：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Dry Mass:"))
                .SetOperandAndAdvance("干质量：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Crew Capacity:"))
                .SetOperandAndAdvance("乘员容量：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Part Count:"))
                .SetOperandAndAdvance("部件数量：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "TWR:"))
                .SetOperandAndAdvance("推重比：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No resources to display!"))
                .SetOperandAndAdvance("载具没有可显示的资源！")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> SettingWindow_drawGui_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show Total Mass"))
                .SetOperandAndAdvance("显示总质量")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show Dry Mass"))
                .SetOperandAndAdvance("显示干质量")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show Crew Capacity"))
                .SetOperandAndAdvance("显示乘员容量")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show Part Count"))
                .SetOperandAndAdvance("显示部件数量")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show TWR"))
                .SetOperandAndAdvance("显示推重比")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use Stock Skin"))
                .SetOperandAndAdvance("使用原版皮肤")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use Bold Font"))
                .SetOperandAndAdvance("字体加粗")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use Compact Spacing"))
                .SetOperandAndAdvance("缩小字间距")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Font Size:"))
                .SetOperandAndAdvance("字体大小：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Label/Data Space:"))
                .SetOperandAndAdvance("标签/数据间空格：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Window Transparency:"))
                .SetOperandAndAdvance("窗口透明度：")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Save"))
                .SetOperandAndAdvance("保存")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close"))
                .SetOperandAndAdvance("关闭")
                ;
            return matcher.InstructionEnumeration();
        }
    }
}
