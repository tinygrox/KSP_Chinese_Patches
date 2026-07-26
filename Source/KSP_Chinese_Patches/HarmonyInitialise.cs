using HarmonyLib;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

[assembly: KSPAssembly("KSP_Chinese_Patches", 1, 0)]
[assembly: KSPAssemblyDependency("HarmonyKSP", 1, 0)]

namespace KSP_Chinese_Patches;

[KSPAddon(KSPAddon.Startup.Instantly, once: true)]
public class HarmonyInitialise : MonoBehaviour
{
    private static Harmony s_staticHarmony;
    public static List<AbstractPatchBase> Patches;

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
        UnityEngine.Debug.Log(StaticMethods.sb);
        Harmony.DEBUG = true;
#endif
        if (!StaticMethods.IsAssemblyLoaded("0Harmony"))
        {
            UnityEngine.Debug.Log("[KSPCNPatches] 未发现安装有 Harmony2! DLL相关的汉化失效！");
            return;
        }
        Stopwatch Clock = Stopwatch.StartNew();
        s_staticHarmony = new Harmony("tinygrox.ChinesePatches");

        var patchTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(AbstractPatchBase).IsAssignableFrom(t) && !t.IsAbstract);
        Patches = new List<AbstractPatchBase>();
        int totalPatches = 0;
        int instantApplied = 0;

        foreach (Type patchType in patchTypes)
        {
            if (Activator.CreateInstance(patchType) is not AbstractPatchBase patchInstance)
            {
                continue;
            }

            totalPatches++;
            if(!patchInstance.IsModLoaded) continue;

            if (patchInstance.ApplyTiming == KSPAddon.Startup.Instantly)
            {
                patchInstance.ApplyPatches(s_staticHarmony);
                instantApplied++;
            }
            else
            {
                Patches.Add(patchInstance);
            }
        }
        int deferredCount = Patches.Count;
        if (deferredCount > 0)
        {
            GameEvents.onLevelWasLoaded.Add(scene => OnLevelWasLoaded((int)scene));
            UnityEngine.Debug.Log($"[KSPCNPatches] {deferredCount} 个 Mod 标记为延迟补丁，将在加载对应场景时自动应用。");
        }

        if (deferredCount <= 0)
        {
            Destroy(this);
        }

        // foreach (AbstractPatchBase patch in Patches)
        // {
        //     patch.ApplyPatches(s_staticHarmony);
        // }

        Clock.Stop();
        UnityEngine.Debug.Log($"[KSPCNPatches] 拟 Patch {totalPatches} 个 Mod，实际应用 {instantApplied} 个，延迟应用 {deferredCount} 个。总耗时：{Clock.Elapsed.TotalSeconds:F2}秒");
    }

    private void OnLevelWasLoaded(int level)
    {
        GameScenes scene = (GameScenes)level;

        if (Patches == null || Patches.Count == 0)
        {
            GameEvents.onLevelWasLoaded.Remove(scene1 => OnLevelWasLoaded(level));
            return;
        }

        for (int i = Patches.Count - 1; i >= 0; i--)
        {
            AbstractPatchBase patch = Patches[i];
            if (!MatchScene(patch.ApplyTiming, scene))
            {
                continue;
            }

            UnityEngine.Debug.Log($"[KSPCNPatches] 延迟补丁 [{patch.PatchName}] 在场景 {scene} 触发");
            patch.ApplyPatches(s_staticHarmony);
            Patches.RemoveAt(i);
        }

        if (Patches.Count <= 0)
        {
            GameEvents.onLevelWasLoaded.Remove(scene1 => OnLevelWasLoaded(level));
            Destroy(this);
        }
    }

    private static bool MatchScene(KSPAddon.Startup timing, GameScenes scene)
    {
        return timing switch
        {
            KSPAddon.Startup.EveryScene => true,

            KSPAddon.Startup.AllGameScenes =>
                scene is GameScenes.SPACECENTER
                    or GameScenes.EDITOR
                    or GameScenes.FLIGHT
                    or GameScenes.TRACKSTATION,

            KSPAddon.Startup.FlightAndEditor =>
                scene is GameScenes.FLIGHT
                    or GameScenes.EDITOR,

            KSPAddon.Startup.FlightAndKSC =>
                scene is GameScenes.FLIGHT
                    or GameScenes.SPACECENTER,

            _ => (int)timing == (int)scene
        };
    }
}
