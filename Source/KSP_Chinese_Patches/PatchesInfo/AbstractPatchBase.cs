using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KSP_Chinese_Patches.PatchesInfo
{
    public abstract class AbstractPatchBase
    {
        private static int instanceCount = 0;
        /// <summary>
        /// 要 Patch 的 Mod 名称，一般以 CKAN 的名字为主
        /// </summary>
        public abstract string PatchName { get; }
        /// <summary>
        /// 要 Patch 的 Mod 的 DLL 文件名，作为 KSP 加载此程序集的名称
        /// </summary>
        public abstract string PatchDLLName { get; }
        protected virtual HashSet<HarPatchInfo> Patches { get; set; }

        private static Harmony thisHarmony;
        protected static Harmony GetHarmony { get { return thisHarmony; } }
        protected virtual void PatchApply(in Harmony harmony, in HarPatchInfo patchInfo)
        {
            switch (patchInfo.PatchType)
            {
                case HarmonyPatchType.Prefix:
                    harmony.Patch(
                        original: patchInfo.TargetMethod,
                        prefix: patchInfo.PatchMethod
                     );
                    break;
                case HarmonyPatchType.Postfix:
                    harmony.Patch(
                        original: patchInfo.TargetMethod,
                        postfix: patchInfo.PatchMethod
                     );
                    break;
                case HarmonyPatchType.Transpiler:
                    harmony.Patch(
                        original: patchInfo.TargetMethod,
                        transpiler: patchInfo.PatchMethod
                     );
                    break;
                case HarmonyPatchType.Finalizer:
                    harmony.Patch(
                        original: patchInfo.TargetMethod,
                        finalizer: patchInfo.PatchMethod
                    );
                    break;
                default:
                    return;
            }
        }
        protected abstract void LoadAllPatchInfo();
        /// <summary>
        /// 检测Mod是否已经加载，已加载则返回 true，反之 false
        /// </summary>
        public virtual bool IsModLoaded
        {
            get
            {
                if (!StaticMethods.IsAssemblyLoaded(PatchDLLName))
                {
                    UnityEngine.Debug.Log($"[KSPCNPatches] 未检测到 [{PatchName}] 已跳过\n");
                    return false;
                }
                return true;
            }
        }
        public static int GetCount => instanceCount;
        public virtual void ApplyPatches(in Harmony harmony)
        {
            if (!IsModLoaded) return;
            thisHarmony ??= harmony;
            Stopwatch watch = Stopwatch.StartNew();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\n[KSPCNPatches] 已找到 [{PatchName}]! 应用翻译...");
            LoadAllPatchInfo();

            foreach (HarPatchInfo patch in Patches)
            {
                try
                {
                    PatchApply(harmony, patch);
                    sb.AppendLine($"[{PatchName}] '{patch.PatchMethod.method.Name}' 已应用");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"[{PatchName}] 应用 '{patch.PatchMethod.method.Name}' 时发生错误，错误信息：\n{ex.ToString()}");
                    continue;
                }
            }
            instanceCount++;
            watch.Stop();
            sb.AppendLine($"'{PatchName}' 共计 {Patches.Count} 个Patch。执行总计{watch.Elapsed.TotalSeconds:F2}秒");
            UnityEngine.Debug.Log(sb.ToStringAndRelease());
        }
    }
}
