using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace KSP_Chinese_Patches.ModPatches
{
    public class ResearchBodiesPatches : AbstractPatchBase
    {
        public override string PatchName => "ResearchBodies";

        public override string PatchDLLName => "ResearchBodies";

        private static IEnumerable<CodeInstruction> ResearchBodies_FoundBody_Patch(IEnumerable<CodeInstruction> codeInstructions)
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

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("ResearchBodies.ResearchBodiesController"), "FoundBody", new[] { typeof(int), typeof(CelestialBody), typeof(bool).MakeByRefType(), typeof(CelestialBody).MakeByRefType() }),
                    new HarmonyMethod(typeof(ResearchBodiesPatches), nameof(ResearchBodiesPatches.ResearchBodies_FoundBody_Patch)),
                    HarmonyPatchType.Transpiler
                )
            };
        }
    }
}
