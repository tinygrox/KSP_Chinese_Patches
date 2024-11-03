using HarmonyLib;
using KSP.Localization;
using KSP_Chinese_Patches.PatchesInfo;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace KSP_Chinese_Patches.ModPatches
{
    public class AtmosphereAutopilotPatches : AbstractPatchBase
    {
        public override string PatchName => "AtmosphereAutopilot (Fly-By-Wire)";

        public override string PatchDLLName => "AtmosphereAutopilot";

        public static IEnumerable<CodeInstruction> AutoGuiAttr_Constructor_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Stfld, AccessTools.Field(AccessTools.TypeByName("AtmosphereAutopilot.AutoGuiAttr"), "value_name")))
                .Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Localizer), nameof(Localizer.Format), new[] { typeof(string) })));
            return matcher.InstructionEnumeration();
        }
        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Constructor(AccessTools.TypeByName("AtmosphereAutopilot.AutoGuiAttr"), new[] { typeof(string), typeof(bool), typeof(string) }),
                    new HarmonyMethod(typeof(AtmosphereAutopilotPatches), nameof(AtmosphereAutopilotPatches.AutoGuiAttr_Constructor_Patch)),
                    PatchType.Transpiler
                )
            };
        }
    }
}
