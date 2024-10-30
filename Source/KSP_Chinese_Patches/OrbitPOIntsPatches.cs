using HarmonyLib;
using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class OrbitPOIntsPatches
    {
        public static IEnumerable<CodeInstruction> ToolbarUI_DrawUI_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            // 原文太傻逼，只能先做个接口以防改来改去
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Enabled"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_Enabled"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Force Refresh"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ForceRefresh"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Focused Body Only"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_FocusedBodyOnly"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Draw Spheres"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_DrawSpheres"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Align Spheres"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_AlignSpheres"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Draw Circles"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_DrawCircles"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show POI Max Terrain Altitude On Atmospheric Bodies"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ShowPOIMaxTerrainAltitudeOnAtmoBodies"))
            #region ControlWrapperInteractionLogger.WrapButton("Reset POIs for " + this._selectedBodyName + " to defaults", null, null);
                // ControlWrapperInteractionLogger.WrapButton(Localizer.Fomat("#OrbitPOInts_RestPOIforBodies", this._selectedBodyName), null, null)
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Reset POIs for "))
                .SetOperandAndAdvance("#OrbitPOInts_RestPOIforBodies")
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Newarr, typeof(string)),
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Ldc_I4_0)
                )
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " to defaults"))
                .RemoveInstructions(2)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Stelem_Ref),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Localizer), nameof(Localizer.Format), new[] { typeof(string), typeof(string[]) })))
            #endregion
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show Options"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ShowOptions"))
                ;
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> OptionsPopup_DrawUI_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Misc"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_Misc"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Enable Debug Level Logging"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_EnableDebugLevelLogging"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use Skin"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_UseSkin"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use Opaque Background Override"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_OpaqueBackground"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use Top Right Close Button"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_UseTopRightCloseButton"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use LeftClick to toggle / RightClick for settings"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_LClickToggleRClickSetting"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "-- Danger Zone -- (no confirmation and no undo)"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_DangerZoneTips"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Reset All Standard POIs to defaults"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ResetPOI"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Remove All Custom POIs"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_RemoveAllPOI"))
                ;
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ToolbarUI_FixState_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.name))))
                .SetOperandAndAdvance(AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.displayName)))
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LingoonaGrammarExtensions), nameof(LingoonaGrammarExtensions.LocalizeRemoveGender), new[] { typeof(string) }))
                )
                ;
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> SimpleColorPicker_DrawUI_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Red"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ColorPicker_Red"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Green"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ColorPicker_Green"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Blue"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ColorPicker_Blue"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cancel"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ColorPicker_Cancel"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Default"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ColorPicker_Default"))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Save"))
                .SetOperandAndAdvance(Localizer.Format("#OrbitPOInts_ColorPicker_Save"))
                ;
            return matcher.InstructionEnumeration();
        }

        public static void POILabelPosfix(ref string __result)
        {
            if (string.IsNullOrEmpty(__result))
                return;
            if (Localizer.TryGetStringByTag($"#OrbitPOInts_{__result}", out string _v))
            {
                __result = _v;
            }
            //__result = __result == Localizer.Format($"#OrbitPOInts_{__result}") ? __result : Localizer.Format($"#OrbitPOInts_{__result}");
        }
    }
}
