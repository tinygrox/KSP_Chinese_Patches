using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace KSP_Chinese_Patches
{
    [KSPAddon(KSPAddon.Startup.Instantly, once: true)]
    public class CNPatchInit : MonoBehaviour
    {
        public void Start()
        {
            //Debug.Log(string.Join("，", Font.GetOSInstalledFontNames()));
#if DEBUG
            //foreach (var a in AssemblyLoader.loadedAssemblies)
            foreach (var a in StaticMethods.AssemblyVersionMap)
            {
                //StaticMethods.sb.AppendLine($"[KSPCNPatches] DLL: {a.name} | DLLName: {a.dllName} | Version: ({a.versionMajor}.{a.versionMinor}.{a.versionRevision})");
                StaticMethods.sb.AppendLine($"[KSPCNPatches] DLLName: {a.Key} | Version: ({a.Value.Major}.{a.Value.Minor}.{a.Value.Build}.{a.Value.Revision})");
            }
            Debug.Log(StaticMethods.sb);
            Harmony.DEBUG = true;
#endif
            if (!StaticMethods.IsAssemblyLoaded("0Harmony"))
            {
                UnityEngine.Debug.Log("[KSPCNPatches] 未发现安装有 Harmony2! DLL相关的汉化失效！");
                return;
            }
            Stopwatch Clock = Stopwatch.StartNew();
            Harmony har = new Harmony("tinygrox.ChinesePatches");

            var PatchTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(AbstractPatchBase).IsAssignableFrom(t) && !t.IsAbstract);
            List<AbstractPatchBase> Patches = new List<AbstractPatchBase>();

            foreach (Type patchType in PatchTypes)
            {
                if (Activator.CreateInstance(patchType) is AbstractPatchBase PatchInstance)
                    Patches.Add(PatchInstance);
            }

            foreach (AbstractPatchBase patch in Patches)
            {
                patch.ApplyPatches(har);
            }


            Clock.Stop();
            UnityEngine.Debug.Log($"[KSPCNPatches] Patch 全部运行完毕！共翻译了 {StaticMethods.PatchedModCount} 个 Mod，跳过了{Patches.Count - StaticMethods.PatchedModCount} 个 Mod。总耗时：{Clock.Elapsed.TotalSeconds:F2}秒");
            Destroy(this);
        }
    }
}
