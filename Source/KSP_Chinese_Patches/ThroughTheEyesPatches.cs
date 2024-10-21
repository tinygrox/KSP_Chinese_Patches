using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class ThroughTheEyesPatches
    {
        public static IEnumerable<CodeInstruction> EVABoundFix_Hook_Patch(IEnumerable<CodeInstruction> codeInstructions, ILGenerator generator)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            Label label1 = generator.DefineLabel();
            Label label2 = generator.DefineLabel();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldarg_0))
                .AddLabels(new[] { label1 })
                .MatchStartForward(new CodeMatch(OpCodes.Stloc_0))
                .AddLabels(new[] { label2 })
                .Start();

            matcher
                .InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldnull),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Object), "op_Inequality", new[] { typeof(UnityEngine.Object), typeof(UnityEngine.Object) })),
                new CodeInstruction(OpCodes.Brtrue_S, label1),
                new CodeInstruction(OpCodes.Ldnull),
                new CodeInstruction(OpCodes.Br_S, label2)
                )
                ;
            return matcher.InstructionEnumeration();
        }
    }
}
