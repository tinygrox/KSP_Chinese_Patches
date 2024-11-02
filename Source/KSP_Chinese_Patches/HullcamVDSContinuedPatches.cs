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
    public class HullcamVDSContinuedPatches : AbstractPatchBase
    {
        public override string PatchName => "HullcamVDS Continued";

        public override string PatchDLLName => "HullcamVDSContinued";

        private static IEnumerable<CodeInstruction> CameraFilterBlackAndWhiteFilm_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Contrast:"))
                .SetOperandAndAdvance("对比度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Brightness:"))
                .SetOperandAndAdvance("亮度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Vignette:"))
                .SetOperandAndAdvance("边缘淡化:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Scratches:"))
                .SetOperandAndAdvance("磨损度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Dust:"))
                .SetOperandAndAdvance("灰尘:")
                ;
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> CameraFilterColorFilm_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Brightness:"))
                .SetOperandAndAdvance("亮度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Vignette:"))
                .SetOperandAndAdvance("边缘淡化:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Scratches:"))
                .SetOperandAndAdvance("磨损度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Dust:"))
                .SetOperandAndAdvance("灰尘:")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> CameraFilterColorHiResTV_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Brightness:"))
                .SetOperandAndAdvance("亮度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "CRT Mesh:"))
                .SetOperandAndAdvance("CRT网格:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Snow:"))
                .SetOperandAndAdvance("雪花噪点:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "V-Hold Bar:"))
                .SetOperandAndAdvance("垂直同步条:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "V-Hold Speed:"))
                .SetOperandAndAdvance("垂直同步速度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "V-Hold Frequency:"))
                .SetOperandAndAdvance("垂直同步频率:")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> CameraFilterColorLoResTV_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Brightness:"))
                .SetOperandAndAdvance("亮度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "CRT Mesh:"))
                .SetOperandAndAdvance("CRT网格:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Snow:"))
                .SetOperandAndAdvance("雪花噪点:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "V-Hold Bar:"))
                .SetOperandAndAdvance("垂直同步条:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "V-Hold Speed:"))
                .SetOperandAndAdvance("垂直同步速度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "V-Hold Frequency:"))
                .SetOperandAndAdvance("垂直同步频率:")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> CameraFilterNightVision_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Boost:"))
                .SetOperandAndAdvance("增强:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Contrast:"))
                .SetOperandAndAdvance("对比度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Brightness:"))
                .SetOperandAndAdvance("亮度:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "CRT Mesh:"))
                .SetOperandAndAdvance("CRT网格:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Snow:"))
                .SetOperandAndAdvance("雪花噪点:")
                ;
            return matcher.InstructionEnumeration();
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterBlackAndWhiteFilm"), "OptionControls"),
                    new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterBlackAndWhiteFilm_OptionControls_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterColorFilm"), "OptionControls"),
                    new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterColorFilm_OptionControls_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterColorHiResTV"), "OptionControls"),
                    new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterColorHiResTV_OptionControls_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterColorLoResTV"), "OptionControls"),
                    new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterColorLoResTV_OptionControls_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("HullcamVDS.CameraFilterNightVision"), "OptionControls"),
                    new HarmonyMethod(typeof(HullcamVDSContinuedPatches), nameof(HullcamVDSContinuedPatches.CameraFilterNightVision_OptionControls_Patch)),
                    PatchType.Transpiler
                ),
            };
        }
    }
}
