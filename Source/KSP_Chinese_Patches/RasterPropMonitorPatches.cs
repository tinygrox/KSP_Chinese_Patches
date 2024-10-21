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
    public class RasterPropMonitorPatches
    {
        public static IEnumerable<CodeInstruction> JSIExternalCameraSelector_OnAwake_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "visibleCameraName"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "摄像机ID"), // Camera ID: 
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),

                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showCones"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, "FOV 范围"), // FOV marker 
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),

                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showCones"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlEditor))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, "关"),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.disabledText))),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, "showCones"),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlEditor))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, "开"),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.enabledText)))
                )
                ;
            return matcher.InstructionEnumeration();
        }
    }
}
