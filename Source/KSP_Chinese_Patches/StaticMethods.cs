using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace KSP_Chinese_Patches
{
    public static class StaticMethods
    {
        public static Dictionary<string, Version> AssemblyVersionMap => AssemblyLoader.loadedAssemblies.ToDictionary(a => a.dllName.ToLowerInvariant(), a => new Version(a.versionMajor, a.versionMinor, a.versionRevision));

        public static StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 传入的是 dll 的 name
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsAssemblyLoaded(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException("Assembly name cannot be null or empty.", nameof(assemblyName));
            }
            //return AssemblyLoader.loadedAssemblies.Any(a => string.Equals(a.dllName, assemblyName, StringComparison.OrdinalIgnoreCase));
            return AssemblyVersionMap.TryGetValue(assemblyName.ToLowerInvariant(), out _);
        }
        /// <summary>
        /// 传入的是 dll 的 name 和版本号
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsAssemblyLoaded(string assemblyName, Version _version)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException("Assembly name cannot be null or empty.", nameof(assemblyName));
            }

            if (_version == null)
            {
                throw new ArgumentNullException(nameof(_version), "Version cannot be null.");
            }

            return AssemblyVersionMap.TryGetValue(assemblyName.ToLowerInvariant(), out Version v) && v.Equals(_version);
        }

        /// <summary>
        /// 插入 base.Fields["targetField"].guiName = guiName 语句的 IL 指令集合
        /// </summary>
        /// <param name="targetField">KSPField的字段名</param>
        /// <param name="guiName">翻译后的guiName</param>
        /// <returns>对应的 IL 指令集合(CodeInstruction[])</returns>
        public static CodeInstruction[] Field_GuiName_Instructions(string targetField, string guiName)
        {
            return new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, targetField),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, guiName),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseField<KSPField>), nameof(BaseField<KSPField>.guiName))),
            };
        }
        /// <summary>
        /// 插入 base.Fields["targetField"].group.displayName = guiName 语句的 IL 指令集合
        /// </summary>
        /// <param name="targetField">KSPField的字段名</param>
        /// <param name="displayName">groupdisplayName的翻译</param><param>
        /// <returns>对应的 IL 指令集合(CodeInstruction[])</returns>
        public static CodeInstruction[] Field_groupDisplayName_Instructions(string targetField, string displayName)
        {
            return new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, targetField),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(BaseField), nameof(BaseField.group))),
                new CodeInstruction(OpCodes.Ldstr, displayName),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(BasePAWGroup), nameof(BasePAWGroup.displayName))),
            };
        }

        public static CodeInstruction[] Field_UIToggle_Instructions(string targetField, string disabledText, string enabledText, bool editorOnly = false, bool flightOnly = false)
        {
            var uiControlEditor = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, targetField),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlEditor))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, disabledText),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.disabledText))),

                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, targetField),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlEditor))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, enabledText),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.enabledText))),
            };
            var uiControlFlight = new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, targetField),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlFlight))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, disabledText),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.disabledText))),

                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Fields))),
                new CodeInstruction(OpCodes.Ldstr, targetField),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseFieldList<BaseField, KSPField>), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(BaseField), nameof(BaseField.uiControlFlight))),
                new CodeInstruction(OpCodes.Isinst, typeof(UI_Toggle)),
                new CodeInstruction(OpCodes.Ldstr, enabledText),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UI_Toggle), nameof(UI_Toggle.enabledText))),
            };
            List<CodeInstruction> res = new List<CodeInstruction>();

            if (!flightOnly)
                res.AddRange(uiControlEditor);

            if (!editorOnly)
                res.AddRange(uiControlFlight);

            return res.ToArray();
        }
        public static CodeInstruction[] Event_GuiName_Instructions(string targetEvent, string guiName)
        {
            return new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Events))),
                new CodeInstruction(OpCodes.Ldstr, targetEvent),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseEventList), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, guiName),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseEvent), nameof(BaseEvent.guiName)))
            };
        }
        public static CodeInstruction[] Action_GuiName_Instructions(string targetAction, string guiName)
        {
            return new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(PartModule), nameof(PartModule.Actions))),
                new CodeInstruction(OpCodes.Ldstr, targetAction),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BaseActionList), "get_Item", new[] { typeof(string) })),
                new CodeInstruction(OpCodes.Ldstr, guiName),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(BaseAction), nameof(BaseEvent.guiName)))
            };
        }

        public static IEnumerable<CodeInstruction> ReplaceLdstr(IEnumerable<CodeInstruction> codeInstructions, string original, string replacement, bool replaceAll = false)
        {
            var list = codeInstructions.ToList();
            bool replaced = false;
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var ins = list[i];
                if (ins.opcode == OpCodes.Ldstr && ins.operand is string str && str == original)
                {
                    list[i] = new CodeInstruction(OpCodes.Ldstr, replacement)
                    {
                        labels = ins.labels,
                        blocks = ins.blocks
                    };
                    replaced = true;
                    count++;
                    if (!replaceAll) break;
                }
            }

            if (!replaced)
            {
                UnityEngine.Debug.Log($"未找到字符串 \"{original}\"，未进行替换。");
            }

            return list.AsEnumerable();
        }
    }
}
