using HarmonyLib;
using KSP.Localization;
using KSP_Chinese_Patches.PatchesInfo;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSP_Chinese_Patches
{
    public class RealAntennasPatches : AbstractPatchBase
    {
        static Type ty = AccessTools.TypeByName("RealAntennas.ModuleRealAntenna");

        public override string PatchName => "Real Antennas";

        public override string PatchDLLName => "RealAntennas";

        private static IEnumerable<CodeInstruction> NetUIConfigurationWindow_WindowGUIPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "ConeMode: {0}"))
                 .SetOperandAndAdvance("波束模式: {0}")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Link End Mode: {0}"))
                 .SetOperandAndAdvance("链接模式: {0}")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "TargetLine: {0}"))
                 .SetOperandAndAdvance("指向链路: {0}")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "3dB Cones: {0}"))
                 .SetOperandAndAdvance("3dB波束: {0}")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "10dB Cones: {0}"))
                 .SetOperandAndAdvance("10dB波束: {0}")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "3D Drawing Distance {0:F0}, Max: {1:F1}"))
                 .SetOperandAndAdvance("3维绘制距离 {0:F0}, 最大:: {1:F1}")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Link Line Brightness: {0:F1}"))
                 .SetOperandAndAdvance("链路亮度: {0:F1}")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cone Circles"))
                 .SetOperandAndAdvance("波束圈数")
                 .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Cone Opacity"))
                 .SetOperandAndAdvance("波束透明度");

            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> OnAwakePatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.
                // base.Fields["_enabled"].guiName = Localizer.Format("");
                InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("_enabled", "天线"))
                // 	(base.Fields["_enabled"].uiControlEditor as UI_Toggle).disabledText = "禁用";
                // 	(base.Fields["_enabled"].uiControlEditor as UI_Toggle).enabledText = "启用";
                .InsertAndAdvance(StaticMethods.Field_UIToggle_Instructions("_enabled", "<color=red><b>禁用</b></color>", "<color=green>启用</color>", editorOnly: true))
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("Condition", "状态")) // Condition
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("Gain", "增益")) // Gain
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("TxPower", "传输功率 (dBm)")) // Transmit Power (dBm)
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("TechLevel", "科技等级")) // Tech Level
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("RFBand", "射频基带")) // RF Band
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("sActivePowerConsumed", "功率 (传输)")) // Power (Active)
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("sIdlePowerConsumed", "功率 (闲置)")) // Power (Idle)
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("sAntennaTarget", "天线指向")) // Antenna Target
                .InsertAndAdvance(StaticMethods.Field_GuiName_Instructions("plannerActiveTxTime", "传输时间")) // Active Transmission Time
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("AntennaTargetGUI", "天线指向")) // Antenna Targeting
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("AntennaPlanningGUI", "天线规划")) // Antenna Planning
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("DebugAntenna", "天线调试信息")) // Debug Antenna
                .InsertAndAdvance(StaticMethods.Event_GuiName_Instructions("PermanentShutdownEvent", "永久关停天线")) // Disable antenna permanently
                .InsertAndAdvance(StaticMethods.Action_GuiName_Instructions("PermanentShutdownAction", "永久关停天线")) // Disable antenna permanently
                ;

            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> RecalculateFieldsLocPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0:F2} Watts")).SetOperandAndAdvance("{0:F2} 瓦特").MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0:F2} Watts")).SetOperandAndAdvance("{0:F2} 瓦特");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PermanentShutdownEventLocPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Yes"))
                .SetOperandAndAdvance("是")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No"))
                .SetOperandAndAdvance("否")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Are you sure you want to permanently disable the antenna? Doing this will prevent it from consuming power but the operation is irreversible."))
                .SetOperandAndAdvance("确定要永久关停天线吗？该操作可以让其不再消耗电力，但操作不可逆。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Disable antenna"))
                .SetOperandAndAdvance("关停天线");

            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> ApplyTLColoringLocPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Tech Level"))
                .SetOperandAndAdvance("科技等级")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<color=orange>Tech Level</color>"))
                .SetOperandAndAdvance("<color=orange>科技等级</color>");

            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> GetInfoLocPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<color=green><b>{0}</b></color>: {1:F1} dBi, {2:F1} beamwidth\n"))
                .SetOperandAndAdvance("<color=green><b>{0}</b></color>: {1:F1} dBi, {2:F1} 波束带宽\n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<color=green>Omni-directional</color>: {0:F1} dBi"))
                .SetOperandAndAdvance("<color=green>全向</color>: {0:F1} dBi");

            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PlannerGUI_OnGUILocPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna Planning")).SetOperandAndAdvance("天线规划");

            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PlannerGUI_GUIDisplayLocPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Ground Station TechLevel: {0:F0}"))
                .SetOperandAndAdvance("地面站科技等级: {0:F0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Ground Station (Planning) TechLevel: {0}"))
                .SetOperandAndAdvance("地面站(规划)科技等级: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Apply"))
                .SetOperandAndAdvance("应用")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Set Ground Station TL to {0} and reset Home antenna to {1}"))
                .SetOperandAndAdvance("设置地面站科技等级为{0} 并重置默认天线为{1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna Selection"))
                .SetOperandAndAdvance("天线选择")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Primary"))
                .SetOperandAndAdvance("主天线")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Peer"))
                .SetOperandAndAdvance("对端天线")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Remote Body Presets"))
                .SetOperandAndAdvance("远程天体预设")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Parameters"))
                .SetOperandAndAdvance("参数")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Primary Antenna: {0} {1}"))
                .SetOperandAndAdvance("主要天线: {0} {1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Peer Antenna: {0} {1}"))
                .SetOperandAndAdvance("对端天线: {0} {1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Distance Max:"))
                .SetOperandAndAdvance("最远距离:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Distance Min:"))
                .SetOperandAndAdvance("最近距离:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Plan!"))
                .SetOperandAndAdvance("更新!")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show Details"))
                .SetOperandAndAdvance("显示细节")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close"))
                .SetOperandAndAdvance("关闭");

            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PlannerGUI_GetDisplayNamePatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.name))))
                .SetOperandAndAdvance(AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.displayName)))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LingoonaGrammarExtensions), nameof(LingoonaGrammarExtensions.LocalizeRemoveGender))));
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PlannerGUI_RenderPanelPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<color=orange>[Best Station]</color>: "))
                .SetOperandAndAdvance("<color=orange>[最佳站点]</color>: ");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PlannerGUI_FireOncePatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Tx/Rx rate at max distance: "))
                .SetOperandAndAdvance("最远距离处发/收速率: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Tx/Rx rate at min distance: "))
                .SetOperandAndAdvance("最近距离处发/收速率: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "  <color=orange><b>(No Connection)</b></color>"))
                .SetOperandAndAdvance("  <color=orange><b>(无连接)</b></color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "  <color=orange><b>(No Connection)</b></color>"))
                .SetOperandAndAdvance("  <color=orange><b>(无连接)</b></color>")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "<color=orange><b>(No Connection)</b></color>"))
                .SetOperandAndAdvance("<color=orange><b>(无连接)</b></color>");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> PlannerGUI_NotifyDisabledPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "RealAntennas: CommNet Disabled in Difficulty Settings"))
                .SetOperandAndAdvance("RealAntennas: 游戏难度已设置关闭通信链路功能")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "RealAntennas: CommNet enabled, requires scene change to take effect"))
                .SetOperandAndAdvance("RealAntennas: 通信链路功能已开启, 切换场景后生效");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> RealAntennasUI_WindowGUIPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Vessels: {0}"))
                .SetOperandAndAdvance("载具: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "GroundStations: {0}"))
                .SetOperandAndAdvance("地面站: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antennas/vessel: {0:F1}"))
                .SetOperandAndAdvance("天线/载具: {0:F1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Name"))
                .SetOperandAndAdvance("名称")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Iterations"))
                .SetOperandAndAdvance("迭代次数")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Avg Time (ms)"))
                .SetOperandAndAdvance("平均时间 (ms)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Runs/sec"))
                .SetOperandAndAdvance("运行次数/秒")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Hide Config Window"))
                .SetOperandAndAdvance("隐藏配置窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show Config Window"))
                .SetOperandAndAdvance("显示配置窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Launch Control Console"))
                .SetOperandAndAdvance("打开控制控制台")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close Control Console"))
                .SetOperandAndAdvance("关闭控制控制台");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> TechLevelInfo_ToStringPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} L:{1} MaxP:{2:N0}dBm MaxRate:{3} Eff:{4:F4}"))
                .SetOperandAndAdvance("{0} 等级:{1} 最大功率:{2:N0}dBm 最大速率:{3} 效率:{4:F4}");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> UpdateList_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Signal (Tx/Rx) : {0:F0}%/{1:F0}%  -  {2}"))
                .SetOperandAndAdvance("信号(发/收) : {0:F0}%/{1:F0}%  -  {2}");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ConnectionDebugger_OnGUI_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Connection Debugger"))
                .SetOperandAndAdvance("连接调试");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> ConnectionDebugger_GUIDisplay_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Vessel: ")) // 24
                .SetOperandAndAdvance("载具: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "None")) // 34
                .SetOperandAndAdvance("无")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna: ")) // 38
                .SetOperandAndAdvance("天线: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Band: {0}       Power: {1}dBm")) // 45
                .SetOperandAndAdvance("频带: {0}       功率: {1}dBm")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target: {0}")) // 61
                .SetOperandAndAdvance("指向: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Transmitter")) // 175
                .SetOperandAndAdvance("发射器")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "(null)")) // 225
                .SetOperandAndAdvance("(空)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna: ")) // 227
                .SetOperandAndAdvance("天线:")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Power: {0}dBm")) // 236
                .SetOperandAndAdvance("功率: {0}dBm")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target: {0}")) // 243
                .SetOperandAndAdvance("指向: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Position: {0:F0}, {1:F0}, {2:F0}")) // 250
                .SetOperandAndAdvance("位置: {0:F0}, {1:F0}, {2:F0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Beamwidth (3dB full-width): {0:F2}")) // 266
                .SetOperandAndAdvance("波束宽度 (3dB 全幅宽): {0:F2}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna AoA: {0:F1}")) // 273
                .SetOperandAndAdvance("天线到达角: {0:F1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Receiver")) // 281
                .SetOperandAndAdvance("接收器")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "(null)")) // 331
                .SetOperandAndAdvance("(空)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna: ")) // 333
                .SetOperandAndAdvance("天线: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Received Power: {0}dBm")) // 342
                .SetOperandAndAdvance("接收器功率: {0}dBm")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target: {0}")) // 349
                .SetOperandAndAdvance("指向: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Position: {0:F0}, {1:F0}, {2:F0}")) // 356
                .SetOperandAndAdvance("位置: {0:F0}, {1:F0}, {2:F0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Beamwidth (3dB full-width): {0:F2}")) // 372
                .SetOperandAndAdvance("波束宽度 (3dB 全幅宽): {0:F2}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna AoA: {0:F1}")) // 381
                .SetOperandAndAdvance("天线到达角: {0:F1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna Elevation: {0:F1}")) // 390
                .SetOperandAndAdvance("天线仰角: {0:F1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Noise")) // 406
                .SetOperandAndAdvance("噪声")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Atmosphere: {0:F0}K")) // 423
                .SetOperandAndAdvance("大气: {0:F0}K")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Body: {0:F0}K")) // 432
                .SetOperandAndAdvance("天体: {0:F0}K")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Receiver: {0:F0}K")) // 441
                .SetOperandAndAdvance("接收器: {0:F0}K")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Total Noise: {0:F2}K")) // 451
                .SetOperandAndAdvance("总噪声: {0:F2}K")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Losses")) // 469
                .SetOperandAndAdvance("衰减")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Path Loss: {0}m ({1:F1}dB)")) // 484
                .SetOperandAndAdvance("链路衰减: {0}m ({1:F1}dB)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Pointing Loss: {0:F1}dB  (Tx: {1:F1}dB + Rx:  {2:F1}dB)")) // 498
                .SetOperandAndAdvance("指向衰减: {0:F1}dB  (发: {1:F1}dB + 收:  {2:F1}dB)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Link Budget")) // 535
                .SetOperandAndAdvance("链路预算")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "RxPower = TxGain ({0:F1} dBi) + TxPower ({1:F1} dBm) - Losses ({2:F1} dB) + RxGain ({3:F1} dBi) = {4:F1} dBm")) // 539
                .SetOperandAndAdvance("接收功率 = 发射增益 ({0:F1} dBi) + 发射功率 ({1:F1} dBm) - 衰减 ({2:F1} dB) + 接收增益 ({3:F1} dBi) = {4:F1} dBm")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Min Link Channel Noise Power = N0 ({0:F1} dBm/Hz) * Bandwidth ({1}Hz ({2:F1} dB)) = {3:F1} dBm")) // 580
                .SetOperandAndAdvance("最小链路信道噪声功率 = N0 ({0:F1} dBm/Hz) * 带宽 ({1}Hz ({2:F1} dB)) = {3:F1} dBm")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Encoder: {0}")) // 611
                .SetOperandAndAdvance("编码器: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Min Link Eb/N0 = RxPower ({0:F1} dBm) - Channel Noise Power ({1:F1} dBm) - Margin ({2:F1} dB) = {3:F1}")) // 616
                .SetOperandAndAdvance("最小链路 Eb/N0 = 接收功率 ({0:F1} dBm) - 信道噪声功率 ({1:F1} dBm) - 余量 ({2:F1} dB) = {3:F1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Achieved Rate: {0:F1}")) // 652
                .SetOperandAndAdvance("理论速率: {0:F1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Valid Rates: {0:F1} - {1:F1}")) // 661
                .SetOperandAndAdvance("有效速率: {0:F1} - {1:F1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Steps: {0}")) // 673
                .SetOperandAndAdvance("速率减半次数: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close")) // 705
                .SetOperandAndAdvance("关闭");
            return matcher.InstructionEnumeration();
        }


        private static IEnumerable<CodeInstruction> AntennaTargetGUI_OnGUI_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna Targeting"))
                .SetOperandAndAdvance("天线指向");
            return matcher.InstructionEnumeration();
        }
        private static IEnumerable<CodeInstruction> AntennaTargetGUI_GUIDisplay_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Vessel: "))
                .SetOperandAndAdvance("载具: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "None"))
                .SetOperandAndAdvance("无")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna: "))
                .SetOperandAndAdvance("天线: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Band: {0}       Power: {1}dBm"))
                .SetOperandAndAdvance("频带: {0}       功率: {1}dBm")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target: {0}"))
                .SetOperandAndAdvance("指向: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target Mode: "))
                .SetOperandAndAdvance("指向模式: ")
                .MatchStartForward(new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.name))))
                .SetOperandAndAdvance(AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.displayName)))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LingoonaGrammarExtensions), nameof(LingoonaGrammarExtensions.LocalizeRemoveGender), new[] { typeof(string) })))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Lat"))
                .SetOperandAndAdvance("纬度")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Lon"))
                .SetOperandAndAdvance("经度")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Alt"))
                .SetOperandAndAdvance("高度")
                .MatchStartForward(new CodeMatch(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.name))))
                .SetOperandAndAdvance(AccessTools.PropertyGetter(typeof(CelestialBody), nameof(CelestialBody.displayName)))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LingoonaGrammarExtensions), nameof(LingoonaGrammarExtensions.LocalizeRemoveGender), new[] { typeof(string) })))
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Azimuth"))
                .SetOperandAndAdvance("方位角")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Elevation"))
                .SetOperandAndAdvance("仰角")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Apply"))
                .SetOperandAndAdvance("应用")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Deflection"))
                .SetOperandAndAdvance("偏转角")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Elevation"))
                .SetOperandAndAdvance("仰角")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Apply"))
                .SetOperandAndAdvance("应用")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close"))
                .SetOperandAndAdvance("关闭");
            return matcher.InstructionEnumeration();
        }


        private static IEnumerable<CodeInstruction> RemoteAntennaControlUI_OnGUI_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Antenna Control Center"))
                .SetOperandAndAdvance("天线控制中心");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> RemoteAntennaControlUI_GUIDisplay_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Sort Mode: {0}"))
                .SetOperandAndAdvance("排序模式: {0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Target"))
                .SetOperandAndAdvance("指向")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Debug"))
                .SetOperandAndAdvance("调试")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close"))
                .SetOperandAndAdvance("关闭")
                ;
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> Encoder_ToStringPatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[{0} Rate {1:F2} Eb/N0 {2}]"))
                .SetOperandAndAdvance("[{0} 速率 {1:F2} Eb/N0 {2}]");
            return matcher.InstructionEnumeration();
        }

        private static IEnumerable<CodeInstruction> RA_SettingTitlePatch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "RealAntennas Settings"))
                .SetOperandAndAdvance("RealAntennas 设置");
            return matcher.InstructionEnumeration();
        }

        public override bool IsModLoaded
        {
            get
            {
                if (!StaticMethods.IsAssemblyLoaded(PatchDLLName, new Version(2, 6, 0)))
                {
                    Debug.Log($"[KSPCNPatches] [{PatchName}] 的版本不是 2.6.0 已跳过\n");
                    return false;
                }
                return true;
            }
        }

        protected override void LoadAllPatchInfo()
        {
            Patches = new HashSet<HarPatchInfo>
            {
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "OnAwake"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.OnAwakePatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "RecalculateFields"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RecalculateFieldsLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "PermanentShutdownEvent"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PermanentShutdownEventLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "ApplyTLColoring"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ApplyTLColoringLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.ModuleRealAntenna"), "GetInfo"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.GetInfoLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "OnGUI"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_OnGUILocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "GUIDisplay", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_GUIDisplayLocPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI+<>c"), "<GUIDisplay>b__44_0", new[] { typeof(CelestialBody) }),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_GetDisplayNamePatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "RenderPanel"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_RenderPanelPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.PlannerGUI"), "FireOnce"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_FireOncePatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.RACommNetScenario+<NotifyDisabled>d__35"), "MoveNext"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.PlannerGUI_NotifyDisabledPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.RealAntennasUI"), "WindowGUI", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RealAntennasUI_WindowGUIPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.TechLevelInfo"), "ToString"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.TechLevelInfo_ToStringPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.MapUI.NetUIConfigurationWindow"), "WindowGUI", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.NetUIConfigurationWindow_WindowGUIPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.MapUI.SignalToolTipController"), "UpdateList"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.UpdateList_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.Network.ConnectionDebugger"), "OnGUI"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ConnectionDebugger_OnGUI_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.Network.ConnectionDebugger"), "GUIDisplay", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.ConnectionDebugger_GUIDisplay_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.AntennaTargetGUI"), "OnGUI"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.AntennaTargetGUI_OnGUI_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.AntennaTargetGUI"), "GUIDisplay", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.AntennaTargetGUI_GUIDisplay_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.RemoteAntennaControlUI"), "OnGUI"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RemoteAntennaControlUI_OnGUI_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.Targeting.RemoteAntennaControlUI"), "GUIDisplay", new[] { typeof(int) }),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RemoteAntennaControlUI_GUIDisplay_Patch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.Method(AccessTools.TypeByName("RealAntennas.Antenna.Encoder"), "ToString"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.Encoder_ToStringPatch)),
                    PatchType.Transpiler
                ),
                new HarPatchInfo
                (
                    AccessTools.PropertyGetter(AccessTools.TypeByName("RealAntennas.RAParameters"), "Title"),
                    new HarmonyMethod(typeof(RealAntennasPatches), nameof(RealAntennasPatches.RA_SettingTitlePatch)),
                    PatchType.Transpiler
                ),
            };
        }
    }
}
