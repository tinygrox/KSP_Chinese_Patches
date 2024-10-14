using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Chinese_Patches
{
    public class xSciencePatches
    {
        public static IEnumerable<CodeInstruction> Body_FigureOutType_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Gas Giant"))
                .SetOperandAndAdvance("气态巨行星")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Star"))
                .SetOperandAndAdvance("恒星")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Planet"))
                .SetOperandAndAdvance("行星")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Moon"))
                .SetOperandAndAdvance("卫星")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Unknown"))
                .SetOperandAndAdvance("未知");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> HelpWindow_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science! Help"))
                .SetOperandAndAdvance("[x] 科学! 帮助");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> HelpWindow_DrawWindowContents_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science! by Z-Key Aerospace and Bodrick."))
                .SetOperandAndAdvance("[x] 科学! 开发者：Z-Key Aerospace 和 Bodrick.")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "About"))
                .SetOperandAndAdvance("关于")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science! creates a list of all possible science.  Use the list to find what is possible, to see what is left to accomplish, to decide where your Kerbals are going next."))
                .SetOperandAndAdvance("[x] 科学! 列出了所有可能的科学实验。利用列表对实验进行查漏补缺，决定你的坎巴拉人接下来要去往何处。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "The four filter buttons at the bottom of the window are"))
                .SetOperandAndAdvance("窗口底部的四个筛选按钮是")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Show experiments available right now – based on you current ship and its situation"))
                .SetOperandAndAdvance("* 显示当前可用的实验 - 基于你当前的载具携带实验和当前所处情况")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Show experiments available on this vessel – based on your ship but including all known biomes"))
                .SetOperandAndAdvance("* 显示载具上可用的实验 - 基于你的载具携带的实验和所有已知的生态群落")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Show all unlocked experiments – based on instruments you have unlocked and celestial bodies you have visited."))
                .SetOperandAndAdvance("* 显示所有未锁定的实验 - 基于已解锁的仪器和你到过的天体")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Show all experiments – shows everything.  You can hide this button"))
                .SetOperandAndAdvance("* 显示所有实验 - 显示所有的实验。你可以将此按钮隐藏")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "The text filter"))
                .SetOperandAndAdvance("文本筛选框")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "To narrow your search, you may enter text into the filter eg \"kerbin’s shores\""))
                .SetOperandAndAdvance("要缩小搜索范围，你可以在搜索框中输入文本，例如\"kerbin 海岸\"")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use – to mean NOT eg \"mun space -near\""))
                .SetOperandAndAdvance("输入 - 意为 [排除] 例 \"mun 太空 -近地\"")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Use | to mean OR eg \"mun|minmus space\""))
                .SetOperandAndAdvance("输入 | 意为 [或] 比如 \"mun|minmus 太空\"")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Hover the mouse over the \"123/456 completed\" text.  A pop-up will show more infromation."))
                .SetOperandAndAdvance("鼠标移到已完成 \"123/456 \" 文本上. 能显示更多信息")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Press the X button to clear your text filter."))
                .SetOperandAndAdvance("点击 X 按钮能够清空筛选条件。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "The settings are"))
                .SetOperandAndAdvance("相关设置是")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Hide complete experiments – Any science with a full green bar is hidden.  It just makes it easier to see what is left to do."))
                .SetOperandAndAdvance("* 隐藏已完成实验 - 任何绿色条充满的科学实验都会隐藏。仅方便我们查看还有什么剩下实验没做。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Complete without recovery – Consider science in your spaceships as if it has been recovered.  You still need to recover to get the points.  It just makes it easier to see what is left to do."))
                .SetOperandAndAdvance("* 已完成未回收 - 载具中存在但尚未回收的科学点数。你仍然需要回收才能获得科学点数。仅是为了方便我们查看还有什么剩下实验没做。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Check debris – Science that survived a crash will be visible.  You may still be able to recover it."))
                .SetOperandAndAdvance("* 检查残骸 - 在坠毁事件中幸存下来的科学点数。你可能仍然能够回收的。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Allow all filter – The \"All\" filter button shows science on planets you have never visited using instruments you have not invented yet.  Some people may consider it overpowered.  If you feel like a cheat, turn it off."))
                .SetOperandAndAdvance("* 显示所有 - \"全部\"筛选按钮会显示你从未到达过的星球和尚未解锁的实验。有些人可能会认为这个功能太强大了。如果你觉得这有作弊的嫌疑，那就关掉它。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Filter difficult science – Hide science that is practically impossible.  Flying at stars, that kinda thing."))
                .SetOperandAndAdvance("* 过滤困难的科学实验 - 隐藏实际上无法进行实验。诸如在恒星上飞行，着陆之类的。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Use blizzy78's toolbar – If you have blizzy78’s toolbar installed then place the [x] Science! button on that instead of the stock \"Launcher\" toolbar."))
                .SetOperandAndAdvance("* 使用 blizzy78的工具栏 – 如果你安装了blizzy78的toolbar，那[x]Science!按钮将会出现在那里，而不是原版的侧边栏。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Right click [x] icon – Choose to open the Here and Now window by right clicking.  Hides the second window.  Otherwise mute music."))
                .SetOperandAndAdvance("* 右键 [x] 图标 - 右键点击行为可选择为打开\"此时此地\"窗口、隐藏窗口、还是禁用音乐。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Music starts muted – Music is muted on load."))
                .SetOperandAndAdvance("* 初始音乐静音 - 在加载时禁播音乐。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Adjust UI Size – Change the scaling of the UI."))
                .SetOperandAndAdvance("* 调整界面大小 - 更改界面的缩放比例。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Here and Now Window"))
                .SetOperandAndAdvance("此时此地窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "The Here and Now Window will stop time-warp, display an alert message and play a noise when you enter a new situation.  To prevent this, close the window."))
                .SetOperandAndAdvance("\"此时此地\"窗口会在你进入新情况时自动停止时间加速、显示提醒消息并播放声音。如不需要，可关闭窗口。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "The Here and Now Window will show all outstanding experiments for the current situation that are possible with the current ship."))
                .SetOperandAndAdvance("\"此时此地\"窗口将显示当前载具所有的可进行的未完成的科学实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "To run an experiment, click the button.  If the button is greyed-out then you may need to reset the experiment or recover or transmit the science."))
                .SetOperandAndAdvance("要运行某个实验，请单击按钮。如果按钮不可点，那表示可能需要重置实验、回收或传输。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "To perform an EVA report or surface sample, first EVA your Kerbal.  The window will react, allowing those buttons to be clicked."))
                .SetOperandAndAdvance("要进行舱外报告或采集地表样本，先让你的坎巴拉人出舱。窗口会自动识别，按钮会变得可用。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Did you know? (includes spoilers)"))
                .SetOperandAndAdvance("你知道吗？(包含剧透)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* In the VAB editor you can use the filter \"Show experiments available on this vessel\" to see what your vessel could collect before you launch it."))
                .SetOperandAndAdvance("*\u0020在VAB编辑器中，你可以点击\"显示载具上可用的实验\"来提前查看你的载具可以收集到什么实验。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Does the filter \"mun space high\" show mun’s highlands?  – use \"mun space –near\" instead."))
                .SetOperandAndAdvance("* 使用\"mun 远地太空\"筛选条件时是否会显示 mun 高地 - 改用\"mun 太空 -附近\"")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Need more science?  Go to Minmus.  It’s a little harder to get to but your fuel will last longer.  A single mission can collect thousands of science points before you have to come back."))
                .SetOperandAndAdvance("* 还需要更多的科学点数？去 Minmus 吧。它有点难去，但你的燃料能用的时间会更长。单次任务就能收集数千个科学点数。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Generally moons are easier - it is more efficient to collect science from the surface of Ike or Gilly than from Duna or Eve.  That said - beware Tylo, it's big and you can't aerobrake."))
                .SetOperandAndAdvance("* 一般来说，天然卫星会更容易 - 从 Ike 或 Gilly 的表面收集科学点数要比从 Duna 或 Eve 收集更容易。但话又说回来，去 Tylo 要小心，它很大，还不能大气制动.")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "* Most of Kerbin’s biomes include both splashed and landed situations.  Landed at Kerbin’s water?  First build an aircraft carrier."))
                .SetOperandAndAdvance("* Kerbin的大部分生态群落包含了溅落和着陆的情况。要着陆在 Kerbin 的水上？首先建造一个能停飞行器的载具吧，比如航空母舰。")
                ;
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ScienceInstance_Description_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} while {1}")) //  {实验名称} while {生态群落}
                .SetOperandAndAdvance("{1} {0}"); // {生态群落} {实验}
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ScienceWindow_Draw_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science!"))
                .SetOperandAndAdvance("[x] 科学!");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ScienceWindow_DrawControls_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0}/{1} complete."))
                .SetOperandAndAdvance("完成度 {0}/{1}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} remaining\n{1:0.#} mits"))
                .SetOperandAndAdvance("剩余{0}\n{1:0.#}米特")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Clear search"))
                .SetOperandAndAdvance("清空搜索")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show experiments available right now"))
                .SetOperandAndAdvance("显示当前可用实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show experiments available on this vessel"))
                .SetOperandAndAdvance("显示当前载具可用实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show unlocked experiments unavailable on this vessel"))
                .SetOperandAndAdvance("显示当前载具已解锁不可用实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show all unlocked experiments"))
                .SetOperandAndAdvance("显示所有已解锁实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show all experiments"))
                .SetOperandAndAdvance("显示所有实验");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ScienceWindow_DrawTitleBarButtons_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close window"))
                .SetOperandAndAdvance("关闭窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Close window"))
                .SetOperandAndAdvance("关闭窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Open help window"))
                .SetOperandAndAdvance("打开帮助窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Open help window"))
                .SetOperandAndAdvance("打开帮助窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Open settings window"))
                .SetOperandAndAdvance("打开设置窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "S"))
                .SetOperandAndAdvance("设置")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Open settings window"))
                .SetOperandAndAdvance("打开设置窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Maximise window"))
                .SetOperandAndAdvance("放大窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Compact window"))
                .SetOperandAndAdvance("精简模式")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "S"))
                .SetOperandAndAdvance("设置")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Compact window"))
                .SetOperandAndAdvance("精简模式");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> SettingsWindow_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science! Settings"))
                .SetOperandAndAdvance("[x] 科学! 设置");
            return matcher.InstructionEnumeration();
        }


        public static IEnumerable<CodeInstruction> SettingsWindow_DrawWindowContents_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Simple mode"))
                .SetOperandAndAdvance("精简模式")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Hides the bottom number on the experiment buttons for a cleaner look."))
                .SetOperandAndAdvance("隐藏科学实验按钮底部的数字，更直观。")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Hide complete experiments"))
                .SetOperandAndAdvance("隐藏已完成实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Experiments considered complete will not be shown."))
                .SetOperandAndAdvance("将不会显示已完成的实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Complete without recovery"))
                .SetOperandAndAdvance("已完成未回收")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show experiments as completed even if they have not been recovered yet.\nYou still need to recover the science to get the points!\nJust easier to see what is left."))
                .SetOperandAndAdvance("显示已完成尚未回收的实验。\n你仍然需要回收相应部件才能获得科学点数！\n仅为了方便查看还有什么剩下实验没做")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Check unloaded vessels"))
                .SetOperandAndAdvance("检查未载入的载具")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Unloaded vessels will be checked for recoverable science."))
                .SetOperandAndAdvance("会检查未载入的载具是否存在可回收的科学点数")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Check debris"))
                .SetOperandAndAdvance("检查残骸")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Vessels marked as debris will be checked for recoverable science."))
                .SetOperandAndAdvance("被标记为残骸的载具也会检查是否存在可回收的科学点数")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Hide Min Science Slider"))
                .SetOperandAndAdvance("隐藏最小科学点数滑动条")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Hide the minimum science slider in the Here & Now window"))
                .SetOperandAndAdvance("隐藏在\"此时此地\"窗口中的最小科学点数滑动条")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Low Min Science"))
                .SetOperandAndAdvance("极低最小科学点数")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Minimum science slider in the Here & Now window will go fom 0.0001 to 0.1"))
                .SetOperandAndAdvance("\"此时此地\"窗口最小科学点数滑动条范围变成从0.0001调至0.1")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Allow all filter"))
                .SetOperandAndAdvance("全部实验按钮")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Adds a filter button showing all experiments, even on unexplored bodies using unavailable instruments.\nMight be considered cheating."))
                .SetOperandAndAdvance("添加一个筛选条件按钮显示所有的实验，包括未到达天体实验，使用未研发的科学仪器.\n可能会被认为是作弊")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Filter difficult science"))
                .SetOperandAndAdvance("筛选困难科学实验")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Hide a few experiments such as flying at stars and gas giants that are almost impossible.\n Also most EVA reports before upgrading Astronaut Complex."))
                .SetOperandAndAdvance("隐藏少数的如在恒星或气态巨行星上飞行这类几乎不可能完成的实验.\n以及升级宇航员中心前的舱外报告")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Selected Object Window"))
                .SetOperandAndAdvance("物体窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show the Selected Object Window in the Tracking Station."))
                .SetOperandAndAdvance("在追踪站显示物体窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Right click [x] icon"))
                .SetOperandAndAdvance("右键[x]图标行为")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Mute music"))
                .SetOperandAndAdvance("静音")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Here & Now window gets its own icon"))
                .SetOperandAndAdvance("\"此时此地\"窗口有独立按钮图标")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Opens Here & Now window"))
                .SetOperandAndAdvance("打开\"此时此地\"窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Here & Now icon is hidden"))
                .SetOperandAndAdvance("隐藏\"此时此地\"图标")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Music starts muted"))
                .SetOperandAndAdvance("载入时静音音乐")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Title music is silenced upon load.  No need to right click"))
                .SetOperandAndAdvance("载入主菜单时静音音乐.无需右键")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Adjust UI size"))
                .SetOperandAndAdvance("调整界面尺寸")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Adjusts the the UI scaling."))
                .SetOperandAndAdvance("调整界面的缩放尺寸")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science! V"))
                .SetOperandAndAdvance("[x] 科学! V")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ShipStateWindow_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science! Selected Object"))
                .SetOperandAndAdvance("[x] 科学! 物体");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ShipStateWindow_DrawBody_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Body: "))
                .SetOperandAndAdvance("天体: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " - Home!"))
                .SetOperandAndAdvance(" - 家园!")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Space high: "))
                .SetOperandAndAdvance("远地太空: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nAtmos depth: "))
                .SetOperandAndAdvance("\n大气层高度: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nFlying high: "))
                .SetOperandAndAdvance("\n高空飞行: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nHas oxygen - jets work"))
                .SetOperandAndAdvance("\n有氧环境 - 喷气引擎可工作")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nNo kind of atmosphere"))
                .SetOperandAndAdvance("\n无大气")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nHas oceans"))
                .SetOperandAndAdvance("\n存在海洋")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\nNo surface"))
                .SetOperandAndAdvance("\n无地表");
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> ShipStateWindow_DrawVessel_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Unowned object"))
                .SetOperandAndAdvance("未知物体")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Crew: {0}, Parts: {1}, Mass: {2:f2}t"))
                .SetOperandAndAdvance("乘员: {0}, 部件: {1}, 质量: {2:f2}t")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No command pod"))
                .SetOperandAndAdvance("无指令舱")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Has command seat"))
                .SetOperandAndAdvance("存在指令舱座位");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> Situation_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();
            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "{0} {1}{2}")) // {Situation.ToString()} {BodyDisplayName} {EXPERIMENT} -> {溅落} {Kerbin} #1生态群落#2实验名称
                .SetOperandAndAdvance("{1} {0} {2}") // eg. {Kerbin} {高空飞行} {海岸 舱外报告} -> ScienceInstance_Description_Patch
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "'s {0}"))
                .SetOperandAndAdvance("{0}")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "'s {0} ({1})"))
                .SetOperandAndAdvance("{0} ({1})");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> Situation_ToString_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "flying high over"))
                .SetOperandAndAdvance("高空飞行")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "flying low over"))
                .SetOperandAndAdvance("低空飞行")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "in space high over"))
                .SetOperandAndAdvance("远地太空")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "in space near"))
                .SetOperandAndAdvance("近地太空")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "landed at"))
                .SetOperandAndAdvance("着陆")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "splashed down at"))
                .SetOperandAndAdvance("溅落")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> StatusWindow_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "[x] Science! Here and Now"))
                .SetOperandAndAdvance("[x] 科学! 此时此地");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> StatusWindow_DrawExperiment_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Experiment description (next run value)"))
                .SetOperandAndAdvance("实验描述 (下次运行的值)")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "\n\nRecovered+OnBoard value"))
                .SetOperandAndAdvance("\n\n回收+载具上的值")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> StatusWindow_MakeSituationToolTip_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Body: "))
                .SetOperandAndAdvance("天体: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " - Home!"))
                .SetOperandAndAdvance(" - 家!")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Space high: "))
                .SetOperandAndAdvance("远地太空: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Atmos depth: "))
                .SetOperandAndAdvance("大气高度: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Flying high: "))
                .SetOperandAndAdvance("高空飞行: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Has oxygen - jets work\n"))
                .SetOperandAndAdvance("有氧环境 - 喷气引擎可工作\n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No kind of atmosphere\n"))
                .SetOperandAndAdvance("无大气\n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Has oceans\n"))
                .SetOperandAndAdvance("存在海洋\n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No surface\n"))
                .SetOperandAndAdvance("无地表\n")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Current vessel: "))
                .SetOperandAndAdvance("当前载具: ")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, " stored experiments"))
                .SetOperandAndAdvance(" 已存储实验数据")
                ;
            return matcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> StatusWindow_DrawWindowContents_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Min Science"))
                .SetOperandAndAdvance("最小科学")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Time warp will be stopped"))
                .SetOperandAndAdvance("停止时间加速")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Time warp will not be stopped"))
                .SetOperandAndAdvance("不会停止时间加速")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Audio alert will sound"))
                .SetOperandAndAdvance("声音提示")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "No audio alert"))
                .SetOperandAndAdvance("无声音提示")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Show results window"))
                .SetOperandAndAdvance("显示结果窗口")
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "Suppress results window"))
                .SetOperandAndAdvance("不显示结果窗口")
                ;
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> StatusWindow_UpdateSituation_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, "New Situation: "))
                .SetOperandAndAdvance("新的情况: ");
            return matcher.InstructionEnumeration();
        }
        public static IEnumerable<CodeInstruction> ResourcesName_Patch(IEnumerable<CodeInstruction> codeInstructions)
        {
            CodeMatcher matcher = new CodeMatcher(codeInstructions).Start();

            matcher
                .MatchStartForward(
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldarg_1),
                new CodeMatch(OpCodes.Stfld, AccessTools.Field(AccessTools.TypeByName("ScienceChecklist.xResourceData"), "name"))
                )
                .RemoveInstructions(3)
                .MatchEndForward(
                new CodeMatch(OpCodes.Ldarg_1),
                new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(PartResourceLibrary), nameof(PartResourceLibrary.GetDefinition), new[] { typeof(string) })),
                new CodeMatch(OpCodes.Stfld, AccessTools.Field(AccessTools.TypeByName("ScienceChecklist.xResourceData"), "def"))
                ).Advance(1)
                .InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(AccessTools.TypeByName("ScienceChecklist.xResourceData"), "def")),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(PartResourceDefinition), nameof(PartResourceDefinition.displayName))),
                new CodeInstruction(OpCodes.Stfld, AccessTools.Field(AccessTools.TypeByName("ScienceChecklist.xResourceData"), "name"))
                )
                ;
            return matcher.InstructionEnumeration();
        }
    }
}
