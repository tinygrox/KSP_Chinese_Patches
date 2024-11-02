using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSP_Chinese_Patches.PatchesInfo
{
    public abstract class AbstractPatchBase
    {
        private static int instanceCount = 0;
        public abstract string PatchName { get; }
        public abstract string PatchDLLName { get; }
        protected virtual HashSet<HarPatchInfo> Patches { get; set; }
        protected virtual void PatchApply(in Harmony harmony, in HarPatchInfo patchInfo)
        {
            switch (patchInfo.PatchType)
            {
                case PatchType.Prefix:
                    harmony.Patch(
                        original: patchInfo.TargetMethod,
                        prefix: patchInfo.PatchMethod
                     );
                    break;
                case PatchType.Postfix:
                    harmony.Patch(
                        original: patchInfo.TargetMethod,
                        postfix: patchInfo.PatchMethod
                     );
                    break;
                case PatchType.Transpiler:
                    harmony.Patch(
                        original: patchInfo.TargetMethod,
                        transpiler: patchInfo.PatchMethod
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
            sb.AppendLine($"执行总计{watch.Elapsed.TotalSeconds:F2}秒");
            UnityEngine.Debug.Log(sb.ToStringAndRelease());
        }
    }
}
