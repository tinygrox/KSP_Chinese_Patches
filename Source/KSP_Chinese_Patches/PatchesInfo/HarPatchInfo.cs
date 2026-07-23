using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace KSP_Chinese_Patches.PatchesInfo;

public class HarPatchInfo
{
    public MethodBase TargetMethod { get; }
    public HarmonyMethod PatchMethod { get; }

    public HarmonyPatchType PatchType { get; }

    public HarPatchInfo(MethodBase methodBase, HarmonyMethod patchMethod, HarmonyPatchType patchType,
        [CallerArgumentExpression("methodBase")] string targetMethodExpression = null)
    {
        TargetMethod = methodBase ?? throw new ArgumentNullException(nameof(methodBase),
            $"Target method cannot be null. |{targetMethodExpression}|");
        PatchMethod = patchMethod ?? throw new ArgumentNullException(nameof(patchMethod), "Patch method cannot be null.");
        PatchType = patchType;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as HarPatchInfo);
    }

    public bool Equals(HarPatchInfo other)
    {
        return other != null && TargetMethod == other.TargetMethod && PatchMethod == other.PatchMethod && PatchType == other.PatchType;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = (hash * 23) + TargetMethod.GetHashCode();
        hash = (hash * 23) + PatchMethod.GetHashCode();
        hash = (hash * 23) + PatchType.GetHashCode();
        return hash;
    }
}
