using HarmonyLib;
using System;
using System.Reflection;

namespace KSP_Chinese_Patches
{
    public enum PatchType
    {
        Prefix,
        Postfix,
        Transpiler,
        Finalizer
    }
    public class HarPatchInfo
    {
        public MethodBase TargetMethod { get; }
        public HarmonyMethod PatchMethod { get; }

        public PatchType PatchType { get; }
        public HarPatchInfo(MethodBase methodBase, HarmonyMethod patchMethod, PatchType patchType)
        {
            TargetMethod = methodBase ?? throw new ArgumentNullException(nameof(methodBase), "Target method cannot be null.");
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
}
