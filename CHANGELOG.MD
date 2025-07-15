# 更新日志 | Changelog

## [1.7.3]
## 修复 & 更新

- 由 @BirghtUniverse 提供的修复和更新 #67

## [1.7.2]

### 修复

- 修复因 **[x] Science! Continued** 更新（更新了本地化）导致报错的问题。=> 能用。

## [1.7.1]

### 修复

- 修复了一处与 **ASET Consolidated Props Pack** IVA 面板相关的错译

## [1.7.0]

### 新增

- **Coatl-Aerospace** - 此 Mod 尚未登录 CKAN，详情请见 https://github.com/raveloda/Coatl-Aerospace/，翻译由 **[@BirghtUniverse](https://github.com/BirghtUniverse)** 提供
- **DMagic Orbital Science** - DMagic 系列的科学部件，由 **[@BirghtUniverse](https://github.com/BirghtUniverse)** 提供
- **QuizTech Aero Pack** - 由 **[@ZIYUXUAN201011](https://github.com/ZIYUXUAN201011)** 提供。
- **Refurbished Armory** - 由 **[@ZIYUXUAN201011](https://github.com/ZIYUXUAN201011)** 提供。

## [1.6.0]

### 新增

- **B9 Aerospace Procedural Wings - Fork** - 大名鼎鼎的 B9 自定义机翼。补充了中文翻译。
- **Kerbal GPS** - 增加代入感的 GPS 卫星 Mod，至少需要 4 颗可见 GPS 卫星才能生效。
- **ASET Consolidated Props Pack** - 继续补充翻译了一些舱内视角下的 prop 文本。

### 修正

- **Squad** - 修正了当移动处理实验室无法分析数据时，其弹出的报错信息文本的错译：原文：`无法以<<1>>分析数据` - `Cannot analyze data as <<1>>` 现修正为 `无法分析数据因为<<1>>`
- **Kerbalism** - 翻译了生涯模式中，启用辐射功能的情况下，在宇航员中心处小绿人信息下方的 `Carrer 辐射: xx%` 文本，现显示为 `职业生涯辐射: xx%`

### 问题修复

- 修复了因强行添加 `Extend`、`Retract` 等本地化键，从而导致的 [#53](https://github.com/tinygrox/KSP_Chinese_Patches/issues/53) 问题。

## [1.5.0]

### 新增

- **Kerballoons** - 为游戏添加 气球 部件的 Mod。部件和硬编码翻译，部分信息写死在.csv文件中，但又不想弄，就先这样将就着吧。
- **IndicatorLights** - 补全了之前缺少的 DLL 硬编码翻译
- **Kerbalism** - 补充了部分 DLL 硬编码翻译，比如监视面板资源的显示

### 修正

- **Squad** - 将研发中心的解锁的部件下方的 `所有` 改成更直白的`已解锁`(原文`owned`)

### 其他

去除了 RealAntenna 翻译的版本限制，出问题再说吧。

上传了一系列个人自用的 MM Patch，用于解决部分 Mod 之间的兼容性问题，同时为玩家提供 Patch 范本和思路。具体可查看 https://github.com/tinygrox/KSP_Chinese_Patches/tree/main/SomeMMPatches 

## [1.4.1]

### 修正

- **VAB Organizer** - 修正 1 处因翻译导致的错误
- **Kiwi Tech Tree Overhaul** - 添加关于电池、指令舱、部分燃料箱的升级选项文本的翻译，相关文本较多，错漏难免

## [1.4.0]

### 新增

- **VAB Organizer** - 很好的部件分类 Mod，添加了主 Mod 翻译以及同作者 如 NF 系列 Mod 的与其相关的翻译
- **Cryo Tanks** - 对自带翻译的修正，特别是“氢气”的修正，以及若干错误修正。
- **System Heat** - 主Mod的翻译补全
- **Shuttle Orbiter Construction Kit** - 仿 NASA 航天飞机的 Mod，部件翻译，涉及 B9PartSwitch 的文本尚未翻译
- **Kiwi Tech Tree Overhaul** - 科技树 Mod 的第一版翻译

### 修改

- **Squad** - 几处原版翻译的修正


## [1.3.1]

该版本是一次紧急修复

### 更新内容

- 修复了一处会导致 PlanetsideExplorationTechnologies 风电工作不正常的 Bug
- 修复了一处 B9PartSwith 文本缺失括号的问题

- 部分 Patch 的代码重构

## [1.3.0]

本次更新内容：

### 新增

- **Extraplanetary Launchpads(需要Harmony)** - 可以在其他星球搭建发射台建造火箭的Mod，Mod 很不错，但是我选择【SimpleConstruction!】添加了内置硬编码文本的翻译
- **OrbitPOInts(需要Harmony)** - 一个能在地图界面可视化星球的引力范围、大气高度等信息的Mod
- **Planetside Exploration Technologies(需要Harmony)** - 硬编码内容的翻译，现在翻译应该完整了。
- **Resource Overview(需要Harmony)** - 轻量型资源显示器。
- **Near Future Electrical** - 补充新增的部件翻译
- **Community Resource Pack** - 为每个资源定义添加了 abbreviation 字段，适配 KSPCF 的改动 https://github.com/KSPModdingLibs/KSPCommunityFixes/pull/235，同时将`联氨(肼)`改为单字翻译`肼`、 `鉛者` 改为单字翻译 `铅` 。这是 1.2 的更新内容，忘记写了，这里补上。

### 修正

- **James Webb For Kerbal** - 将原来的翻译“詹姆斯·**韦伯**”修改为“詹姆斯·**韦布**”，遵循全国科学技术名词审定委员会发布的译名
- **Squad** - 将原版中文翻译的 `在<<1>>表面建造一辆探测车` 更正为`在<<1>>表面完成探测车的组装` （英原文 `Finish construction of a Rover on the surface of <<1>>`）
- **Benjee10 sharedAssets** - 修复了一处导致**C-100 异体同构对接装置**的动作按钮仍是英文的错误
- **Far Future Technologies** - 补充新增的部件翻译

### 其他

- 代码整体进行重构，输出日志现在包含每个 Mod 的翻译 Patch 的执行时间，更加直观的知道是翻译哪个Mod拖慢了速度

## [1.2.0]

本次更新内容：

### 新增

- **Avionics Systems** - 翻译了此Mod包含的舱内显示器的部分页面，注意未全部翻译，只翻译了涉及了 **DE_IVAExtension** 的内容，字体使用**微软雅黑**，请确保系统有此字体，一般来说 Win10 都是自带的。 
- **ASET Consolidated Props Pack** - 翻译了部分 Prop (仪器的按钮文本等)的内容，字体同样设置为**微软雅黑**，请确保系统有安装该字体
- **HullcamVDS Continued** - 对新的摄像机部件进行翻译，以及 DLL 硬编码的 guiName 的翻译(需要Harmony)
- **Through The Eyes of a Kerbal** - 并未翻译，而是一个修复Patch，修复了一个因为这个 Mod 而导致的报错
- **RasterPropMonitor Core(需要Harmony)** - 部分联动摄像机 Mod 的 guiName 相关的翻译

## [1.1.0]

本次更新内容如下：(DLL 翻译暂不开放翻译接口)

### 新增

- **[x] Science! Continued(需要Harmony)** - 方便科学点数查漏补缺
- **BetterBurnTime(需要Harmony)** - 一个速度指示扩展，感觉有点鸡肋
- **Background Resources** - TAC LS 和 DeepFreeze Continued 的前置，翻译了设置文本
- **Better SRBs** - 固推药柱的几种装法
- **Docking Port Alignment Indicator(需要Harmony)** - 手动对接辅助，鉴定为不如 MechJeb2 一根
- **Physics Range Extender(需要Harmony)** - 物理距离扩展
- **B9 Part Switch(需要Harmony)** - 燃料箱上的硬编码 PartInfo 等文本

## [1.0.0]

几年测试期间无收到任何恶性问题反馈，测试阶段可以结束，进入正式版本。欢迎各位继续尽情提 PR！

### 项目更新

- 项目更新日志现在转存到 CHANGELOG.MD 文件，文件名全大写
- 为项目添加了一个能够自动发布的 GitHub Action
- 上线部分 DLL Mod 翻译的支持 - 需要安装 Harmony (*CKAN 搜 Harmony 2*)，否则不生效。

### 新增 Mod 翻译支持

- **FASA** - 一个部件包，一些仿 NASA 历代任务的航天部件，比如水星计划、探索者系列、双子座、阿波罗系列等。
- **Where Can I Go(需要Harmony)** - 一个很简单的小 Mod，会读当前载具的dv，然后告诉你当前载具可以去到哪些地方，能不能入轨、飞掠等等。
- **SmartStage(需要Harmony)** - 这个 Mod 可以自动帮你整理分级。示例场景：原版场景下，你创建了运载火箭 A 和载荷 B，分别设置好了分级，但是当你想要合并两者时，发现两者的分级顺序也自动合并了，但是合并后的分级并非以增加的方法进行，这导致了合并后分级顺序紊乱，当分级较多时，排列分级顺序将会成为一个折磨人的过程，而 SmartStage 就是为了处理了这一点，虽然不算很完美。
- **RealAntenna(需要Harmony)** - 部分玩家心心念的真实天线 Mod，现在可以翻译为中文了。翻译尚不支持 RO 环境。

## [0.12.0]
本次更新内容均由 @zhangyuesai 无私分享

### 新增

- **Astrogator** - 一个轨道窗口计算器
- **SpaceY 系列** - SpaceYCorp、SpaceYLifters 和 SpaceYExpanded 的支持
- **Speed Unit Annex** - 一个增强了速度指示的 Mod

### 修正

- **JNSQ** 的中几个生态群系的名称
- **Bases and Stations Reborn** 中的一处翻译。

## [0.11.0]

本次更新主要新增了合同包翻译：

- Bases and Stations Reborn - 空间基地扩展合同 | 同时修改了合同包的最大活跃合同数为 3，即同一时间可出现的合同数量为 3

- Clever Sats - 卫星合同

- CommNet Relays - 尚未使用过，请玩家自行体验

- Exploration Plus - 探索+，原版探索合同增强

- Field Research - 增加了一系列科研合同

- Rover Missions - 尚未使用过，请玩家自行体验

- Tourism Plus - 原版旅客合同增强

  (上述合同包由 @zhangyuesai 热心分享，感谢 @zhangyuesai ！)

- Kerbal Academy - 完成合同 = 训练你的小绿人 = 小绿人等级升级

修改：

一系列小错误修正 + 原版翻译修正。

详情请看 https://github.com/tinygrox/KSP_Chinese_Patches/pull/23

## [0.10.0]

本次的更新内容全部由玩家 [@zhangyuesai](https://github.com/zhangyuesai) 提供

## [0.9.0]

新增：

- **JNSQ** - 一个原版扩展星球包的支持，感谢玩家 [@zhangyuesai](https://github.com/zhangyuesai) 的分享
- **JNSQ Real Date** - JNSQ + Kronometer 搭配使用的真实日期显示 Mod，主要更改了左上角的日期现实格式，默认显示格式是 日 月 年，如果想要使用年月日，可以去本Mod目录下的 JNSQRealDate/ZH_JSNQRealDate.cfg 文件内查看方法。
- **Kronometer** - 用来修改日期显示格式的 Mod。
- **Tarsier Space Technology** - 一个太空望远镜 Mod，是可以用的那种望远镜部件。来自 [@zhangyuesai](https://github.com/zhangyuesai) 的分享
- **Quick 系列** - 包括QuickBrake、QuickContracts、QuickExit、QuickGoTo、QuickHide、QuickIVA、QuickMute、QuickRevert、QuickSAS、QuickSearch、QuickStart  同样来自玩家 [@zhangyuesai](https://github.com/zhangyuesai) 的分享

## [0.8.0]

不算什么大更。

新增：

- **ResearchBodies** 支持。
- **Squad** - 原版一处错译修正。原版翻译 `#autoLOC_8003261 = 动作绑定` 英原文为 `Axis Groups` ，修正为 `轴动作组`

## [0.7.1]

一次小更。

新增：

- **MOAR Station Science** 支持，感谢来自玩家 [@1328411791](https://github.com/1328411791) 分享的翻译
- **Squad** - 原版一处错译修正。原版翻译 `#autoLOC_283371 = 让<<1>>摆脱重力` 英原文为 `Have <<1>> pass out from gee forces` ，修正为 `让<<1>>过载昏迷`

## [0.7.0]

新增：

- **Better Early Tree** 的支持
- **Kerbal Foundries2** 的支持。车轮部件的英文来源于依赖  **KSPWheel**
- **Kronal Vessel Viewer Continued** 的支持。一个蓝图生成Mod，功能有限。
- **RasterPropMonitor Core** 的支持。2个镜头部件。

- **Mk3 Stockalike Expansion** 的支持，我知道作者诈尸更新了一波，但是我先更新我的，以后再说
- **Rational Resources Nuclear Family(未完全)** 和 **Rational HydroDynamics** 的极小部分支持。太多了，我其实后悔了。
- **SCANSat** - 来自玩家 [@yuanze31](https://github.com/yuanze31) 提供的补全翻译。
- **Far Future Technologies** - 突然发现有些小地方没翻译。
- **MechJeb2** 的支持，发现这个最初的汉化已经被很多人调教过了，很好，补全了一些目标轨道数据信息相关的翻译。

修改：

- **AmpYear** - 修正了一下在使用 RemoteTech 失去控制时 AmpYear 的中文翻译导致界面错位问题。
- **Mk2 Stockalike Expansion** 一点小更，但是忘记更了啥。
- **RemoteTech** 研发科技节点【Probe Core Upgrade】翻译。

## [0.6.1]

小型更新：

新增：

- RemoteTech的翻译，感谢 [@yuanze31](https://github.com/yuanze31) 提供的改进翻译。

修改：

- 因为最新 Kerbalism 包含了对HabTech2的修复，特此删除了本项目对 Kerbalism 的 HabTech2 修复 Patch（其实就是改个文件名）

## [0.6.0]

新增：

- 新增对 **Kerbalism** 的翻译补充
- 新增对 **Ferram Aerospace Research Continued(FAR)** 的部分翻译补充

## [0.5.0]

新增：

- 新增对 **DeepFreeze Continued** 的支持。该文本是本人于2022年9月份提交给作者的内容 https://github.com/JPLRepo/DeepFreeze/pull/111 不过很明显的，原作者已经不活跃了，所以汉化也就跟着遥遥无期了。这其实也是促成本项目创立的原因之一，之所以现在才加入是因为之前忘记加了。

修改：

- 感谢 [@yuanze31](https://github.com/yuanze31) 对 **Universal Storage II Finalized** 的翻译做出的改进。

## [0.4.0]

此版本将变更目录名称，原目录名 **GameData\Tinygrox_CNPatches** 将变为 **GameData\0000Tinygrox_CNPatches**，为了保证 KSP 在载入本地化文件时，此项目的读取顺序能早于其他 Mod 的本地化文件。

新增：

- 新增了对 **AmpYear** 的支持

## [0.3.0]

新增：

- 新增了对 **Advanced Jet Engine** 的翻译补全
- 新增了对 **Mk2 Stockalike Expansion** 的支持 
- 新增了对 **Rational Resources** 主 Mod 的支持
- 新增了对 **Universal Storage II Finalized** 的**不完全**支持（意思是未完全汉化完毕），感谢由百度贴吧用户 @CQC_S ([贴吧原贴](https://tieba.baidu.com/p/8466511187)) 提供的翻译文本。该 Mod 目前尚未在本人的 Mod 使用列表中，发现任何错误、漏翻，建议请直接提交 PR 以改进。

修改：

- 将原本 **Solar Array** 的翻译 -  **太阳能阵列** 更改为与原版一致的 **太阳能电池板** - 涉及到更改的 Mod 为 **HabTech2** 与 **Planetside Exploration Technologies**
- **Hab Tech 2** 中对 **SG-G1 Pointing Gimbal** 的原翻译 **SG-G1 朝向万向节** 修改为 **SG-G1 方向调节云台**

## [0.2.0]

- 新增了对 **RealExoplanets** 行星包的支持
- 新增了 **Realfuels** 支持，主要补全了原来本地化没有照顾到的内容

## [0.1.0]

- Mod 的第一次上传
