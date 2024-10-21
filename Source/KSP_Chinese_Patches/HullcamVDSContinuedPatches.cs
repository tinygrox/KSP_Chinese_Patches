using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class HullcamVDSContinuedPatches
    {
        public static IEnumerable<CodeInstruction> CameraFilterBlackAndWhiteFilm_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
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
        public static IEnumerable<CodeInstruction> CameraFilterColorFilm_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
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

        public static IEnumerable<CodeInstruction> CameraFilterColorHiResTV_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
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

        public static IEnumerable<CodeInstruction> CameraFilterColorLoResTV_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
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

        public static IEnumerable<CodeInstruction> CameraFilterNightVision_OptionControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
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
    }
}
