using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace KSP_Chinese_Patches.ModPatches
{
    public class ThroughTheEyesPatches : AbstractPatchBase
    {
        public override string PatchName => "Through The Eyes of a Kerbal";

        public override string PatchDLLName => "ThroughTheEyes";

        private static IEnumerable<CodeInstruction> EVABoundFix_Hook_Patch(IEnumerable<CodeInstruction> codeInstructions, ILGenerator generator)
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

        public override bool IsModLoaded
        {
            get
            {
                if (!StaticMethods.IsAssemblyLoaded("ThroughTheEyes", new Version(2, 0, 4)))
                    return false;
                return true;
            }
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("FirstPerson.EVABoundFix"), "Hook"),
                    new HarmonyMethod(typeof(ThroughTheEyesPatches), nameof(ThroughTheEyesPatches.EVABoundFix_Hook_Patch)),
                    PatchType.Transpiler
                )
            };
        }
    }
}
