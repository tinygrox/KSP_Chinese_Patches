using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class AvionicsSystemsPatches
    {
        public static IEnumerable<CodeInstruction> MASFlightComputerProxy_BodyBiome_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(ScienceUtil), nameof(ScienceUtil.GetExperimentBiome))))
                .SetOperandAndAdvance(AccessTools.Method(typeof(ScienceUtil), nameof(ScienceUtil.GetExperimentBiomeLocalized)));
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> MASFlightComputerProxy_BodyName_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(CelestialBody), nameof(CelestialBody.bodyName))))
                .SetOperandAndAdvance(AccessTools.Field(typeof(CelestialBody), nameof(CelestialBody.bodyDisplayName)))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LingoonaGrammarExtensions), nameof(LingoonaGrammarExtensions.LocalizeRemoveGender), new[] { typeof(string) })))
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> MASVesselComputer_UpdateTarget_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(ITargetable), nameof(ITargetable.GetName))))
                .SetOperandAndAdvance(AccessTools.Method(typeof(ITargetable), nameof(ITargetable.GetDisplayName)))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LingoonaGrammarExtensions), nameof(LingoonaGrammarExtensions.LocalizeRemoveGender), new[] { typeof(string) })))
                ;
            return matcher.InstructionEnumeration();
        }
    }
}
