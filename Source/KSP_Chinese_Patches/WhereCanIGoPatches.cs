using HarmonyLib;
using KSP.Localization;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSP_Chinese_Patches
{
    public class WhereCanIGoPatches : AbstractPatchBase
    {
        static Dictionary<string, string> loc = new Dictionary<string, string>()
        {
            ["No Vessel Detected"] = Localizer.Format("#CNPatches_WCIG_NoVesselDetected"),
            ["Return Trip?"] = Localizer.Format("#CNPatches_WCIG_ReturnTrip"),
            ["Payload Only"] = Localizer.Format("#CNPatches_WCIG_PayloadOnly"),
            ["Flyby: "] = Localizer.Format("#CNPatches_WCIG_Flyby"),
            ["Orbiting: "] = Localizer.Format("#CNPatches_WCIG_Orbiting"),
            ["Synchronous Orbit: "] = Localizer.Format("#CNPatches_WCIG_SynchronousOrbit"),
            ["Landing: "] = Localizer.Format("#CNPatches_WCIG_Landing"),
            ["*Assuming craft has enough chutes"] = Localizer.Format("#CNPatches_WCIG_EnoughChutes"),
            ["Close"] = Localizer.Format("#CNPatches_WCIG_Close"),
            ["No Data Available. Achieve stable orbit around "] = Localizer.Format("#CNPatches_WCIG_NoData"),
            ["Where Can I Go"] = Localizer.Format("#CNPatches_WCIG_DialogTitle"),
            ["m/s short)"] = "m/s " + Localizer.Format("#CNPatches_WCIG_short") + ")",
            ["YES"] = Localizer.Format("#CNPatches_WCIG_Yes"),
            ["YES*"] = Localizer.Format("#CNPatches_WCIG_Yes") + "*",
            ["NO"] = Localizer.Format("#CNPatches_WCIG_No"),
            ["MARGINAL"] = Localizer.Format("#CNPatches_WCIG_Marginal"),
        };

        public override string PatchName => "Where Can I Go";

        public override string PatchDLLName => "WhereCanIGo";

        public static IEnumerable<CodeInstruction> GenerateDialogLocPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            CodeMatcher matcher2 = new CodeMatcher(codeInstructions).Start();
            while (matcher.Pos < matcher.Length)
            {
                matcher2.MatchStartForward(new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(FlightGlobals), nameof(FlightGlobals.GetHomeBodyName))));
                if (matcher2.IsValid)
                {
                    matcher2.SetOperandAndAdvance(AccessTools.Method(typeof(FlightGlobals), nameof(FlightGlobals.GetHomeBodyDisplayName)));
                }
                matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr));
                if (matcher.IsValid)
                {
                    string originalString = matcher.Operand as string;
                    if (originalString != null && loc.ContainsKey(originalString))
                    {
                        matcher.SetOperandAndAdvance(loc[originalString]);
                    }
                    else
                    {
                        matcher.Advance(1);
                    }
                }
            }
            return matcher.InstructionEnumeration();
        }

        public override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>()
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoEditor"), "GenerateDialog"),
                    new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoEditor"), "GetDeltaVString"),
                    new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoFlight"), "GenerateDialog"),
                    new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.WhereCanIGoFlight"), "GetDeltaVString"),
                    new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.Utilities"), "SituationValid"),
                    new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.Utilities"), "VesselStatus"),
                    new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("WhereCanIGo.Utilities"), "GetTextColor"),
                    new HarmonyMethod(typeof(WhereCanIGoPatches), nameof(WhereCanIGoPatches.GenerateDialogLocPatch)),
                    PatchType.Transpiler
                ),
            };
        }
    }
}
