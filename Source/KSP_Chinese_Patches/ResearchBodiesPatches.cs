using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class ResearchBodiesPatches
    {
        public static IEnumerable<CodeInstruction> ResearchBodies_FoundBody_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Added "))
                .SetOperandAndAdvance("发现天体！已添加 ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " science points !"))
                .SetOperandAndAdvance(" 科学点数！")
                ;
            return matcher.InstructionEnumeration();
        }
    }
}
