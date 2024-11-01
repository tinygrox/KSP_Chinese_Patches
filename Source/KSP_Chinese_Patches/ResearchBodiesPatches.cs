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
    public class ResearchBodiesPatches : AbstractPatchBase
    {
        public override string PatchName => "ResearchBodies";

        public override string PatchDLLName => "ResearchBodies";

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

        public override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ResearchBodies.ResearchBodiesController"), "FoundBody", new[] { typeof(int), typeof(CelestialBody), typeof(bool).MakeByRefType(), typeof(CelestialBody).MakeByRefType() }),
                    new HarmonyMethod(typeof(ResearchBodiesPatches), nameof(ResearchBodiesPatches.ResearchBodies_FoundBody_Patch)),
                    PatchType.Transpiler
                )
            };
        }
    }
}
