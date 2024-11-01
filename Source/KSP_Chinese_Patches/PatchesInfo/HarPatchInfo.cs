using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public enum PatchType
    {
        Prefix,
        Postfix,
        Transpiler
    }
    public class HarPatchInfo
    {
        public MethodBase TargetMethod { get; }
        public HarmonyMethod PatchMethod { get; }

        public PatchType PatchType { get; }
        public HarPatchInfo(MethodBase methodBase, HarmonyMethod patchMethod, PatchType patchType)
        {
            TargetMethod = methodBase;
            PatchMethod = patchMethod;
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
