# 贡献指南 | 如何参与本项目

> [!NOTE]
>
> 本文档由 GLM-5.2 辅助生成。
> 所有内容均经过人工审核、修改和最终确认。

首先，感谢你有兴趣为本项目出一份力！无论你是想反馈一个翻译错误、修正一个标点，还是为项目添加新的汉化，都非常欢迎。

本项目是一个专为 **KSP（坎巴拉太空计划）社区无中文 Mod 的翻译补丁合集**，主要通过 [Module Manager](https://ksp.sarbian.com/jenkins/job/ModuleManager/) 的 Patch 机制替换文本，少部分涉及硬编码文本使用 Harmony + C#。采用 **CC BY 4.0** 许可

> 本指南面向**完全不了解 GitHub 的玩家**，会从零开始讲清楚每一步。已经熟悉 Git 工作流的同好可以直接跳到 [贡献方式一览](#贡献方式一览) 对应章节。

---

## 目录

- [贡献方式一览](#贡献方式一览)
- [准备工作](#准备工作)
- [GitHub 工作流：从零开始](#github-工作流从零开始)
- [贡献方式一：问题反馈（Issue）](#贡献方式一问题反馈issue)
- [贡献方式二：翻译改进与校对](#贡献方式二翻译改进与校对)
- [贡献方式三：新增 .cfg 翻译补丁](#贡献方式三新增-cfg-翻译补丁)
- [贡献方式四：Harmony / C# 硬编码翻译](#贡献方式四harmony--c-硬编码翻译)
- [提交前的检查清单](#提交前的检查清单)
- [分支模型与发布流程](#分支模型与发布流程)
- [翻译风格与术语建议](#翻译风格与术语建议)
- [许可与署名](#许可与署名)

---

## 贡献方式一览

本项目欢迎以下四种贡献形式，难度从低到高：

| 方式 | 适合谁 | 需要会什么 | 改动内容 |
|------|--------|-----------|---------|
| [问题反馈（Issue）](#贡献方式一问题反馈issue) | 所有玩家 | 会用 GitHub 提 issue | 不改代码 |
| [翻译改进/校对](#贡献方式二翻译改进与校对) | 任何人 | 基本的 Git/GitHub 操作 | 改 `.cfg` 文本 |
| [新增 .cfg 翻译补丁](#贡献方式三新增-cfg-翻译补丁) | 略懂 Mod 文件结构 | Git/GitHub + MM patch 基础 | 新增 `.cfg` |
| [Harmony / C# 硬编码翻译](#贡献方式四harmony--c-硬编码翻译) | 有 C# 基础的开发者 | Git/GitHub + C# + Harmony | 新增 `.cs` + `.cfg` |

如果只是想反馈"这里翻译错了/漏翻了"，提一个 Issue 就足够了，不需要动手改代码。

---

## 准备工作

无论哪种贡献方式，你都需要：

1. **一个 GitHub 账号**（免费注册：<https://github.com/signup>）
2. **Git 客户端** —— 推荐 [GitHub Desktop](https://desktop.github.com/)（图形界面，新手友好）或命令行 [Git](https://git-scm.com/)
3. **一个文本编辑器** —— 推荐 [VS Code](https://code.visualstudio.com/)，配合 [KSPCFG 插件](https://marketplace.visualstudio.com/items?itemName=al2me6.ksp-cfg-support) 可以高亮 `.cfg` 语法并辅助排错
4. **KSP 游戏本体** + 已安装的 **Module Manager**（如果没有你不可能用得上这个 Mod）
5. （仅硬编码翻译）**Visual Studio 2026** 或同等 C# IDE（如 Rider），确认安装了 **.NET Framework 4.7.2**

---

## GitHub 工作流：从零开始

你不能直接往别人的仓库里写东西，正确流程是 **Fork → git clone 到自己电脑上 → 新建 branch → 修改 → pull → 发起 Pull Request**。

下面按步骤说明。可参考 [GitHub 官方文档 - Pull requests](https://docs.github.com/en/pull-requests)，这里只讲本项目相关的关键点。

### 第 1 步：Fork 项目

1. 用你的 GitHub 账号登录后，回到本项目主页
2. 点右上角的 **Fork** 按钮，取消勾选 **Copy the _ branch only**
3. 点击绿色的 Create fork 按钮，GitHub 就会把整个仓库复制一份到你自己的账号下（例如 `https://github.com/你的用户名/KSP_Chinese_Patches`）

### 第 2 步：克隆到本地

在你 fork 出来的仓库页面点 **Code → HTTPS**，复制地址，然后：

- **GitHub Desktop**：点 `File → Clone repository → URL`，粘贴地址，选一个本地目录。这一步也可以直接在网页端点击 Code 之后选择 Open with GitHub Desktop
- **命令行**(如果安装了 Git)：
  
  ```bash
  git clone https://github.com/你的用户名/KSP_Chinese_Patches.git
  cd KSP_Chinese_Patches
  ```

### 第 3 步：关联上游仓库（可选，为了以后同步更新）

只需做一次：

```bash
git remote add upstream https://github.com/tinygrox/KSP_Chinese_Patches.git
```

以后当原仓库有更新时，可以用 `git fetch upstream` 拉取，再合并到你的分支。

关于同步，这个也可以在网页端进行操作，每当存在更新时，进入你的 fork 仓库页面，找到并点击 **Sync fork**，然后再点 **Update branch**。

### 第 4 步：基于 dev 分支新建你自己的分支

**重要：请始终基于 `dev` 分支开工，不要用 `main`。** 详见 [分支模型与发布流程](#分支模型与发布流程)。

```bash
git switch dev
git pull upstream dev      # 拉取最新的 dev
git switch -c add-xxx翻译  # 新建并切换到你的分支，分支名随意，建议用英文且有描述性，最好用某个要汉化的mod名称
```

最好是针对一个 mod 的汉化就对应一条分支。

### 第 5 步：做出修改

用编辑器修改或新增文件。具体改什么、怎么改，见下方对应章节。

### 第 6 步：提交（commit）

```bash
git add .                  # 暂存所有改动
git commit -m "新增 XXX Mod 的中文翻译"
```

提交信息（commit message）建议清晰描述你做了什么，例如：
- `新增 AtomicAge Mod 的部件翻译`
- `修正 Kerbalism 中"氢气"的翻译错误`
- `补充 NearFutureSolar 漏翻的两条文本`

### 第 7 步：推送到你的 fork

```bash
git push origin add-xxx翻译
```

### 第 8 步：发起 Pull Request

推送完成后，打开 GitHub 网页端，你的仓库会提示 **Compare & pull request** 点击按钮后：（或者点进 `Pull request` 点击 new pull request）

1. **确认 base 分支是 `dev`**（不是 `main`）
2. 填写标题和说明，说明你新增/修改了哪些 Mod 的翻译
3. 点 **Create pull request**

维护者会进行 review，可能提出修改建议。你按建议改完后再 `git push` 一次即可，PR 会自动更新。

---

## 贡献方式一：问题反馈（Issue）

不需要改代码，适合所有人。详细的反馈要求见 [REPORT.md](./REPORT.md)，这里是要点：

### 反馈前请确认

- 问题可以稳定复现（别反馈重启就好那种）
- 问题确实由本汉化补丁引起，而非 Mod 本身或游戏本身
- Issue 里是不是有人提了

> [!NOTE]
>
> 如果你的游戏安装了大量 Mod，请不要直接上传完整 Mod 列表并要求排查。没人（反正我没有，除非你加钱）有那个时间在数百个 Mod 环境中帮你定位问题来源。
>
> 请先自行进行最基本的排查工作，使用二分法逐步缩小范围，尽量将问题定位到**某一个**、**某几个**，或**某一堆**
>
> 在最小复现环境下再次验证问题是否存在

### 必须提供的信息

一个合格的 issue **至少**要包含：

1. **游戏版本**（如 1.12.5）
2. **已安装的 Mod 列表**（可用 CKAN 导出）
3. **`ModuleManager.ConfigCache` 文件** —— 位于 `GameData/` 下
4. **问题复现步骤** —— 清晰描述"装了 A → 正常，再装 B → 异常"这类过程
5. （加分项）**问题出现时**的截图、和**问题出现时**位于游戏根目录的 `KSP.log`

GitHub 无法直接传 `.log`/`.ConfigCache` 文件，请先一起压缩成 zip 再上传

不推荐：

> 进入 VAB 出现异常，然后甩出安装了 200 多个 Mod 的列表

推荐：

> 经过排查后确认，仅安装 A、B、C 三个 Mod 时即可稳定复现问题；移除 B 后问题消失。



### 如何提交

在本项目主页点 **Issues → New issue**，按上面的要点填写即可。

---

## 贡献方式二：翻译改进与校对

适合"看到某条翻译不对，想直接改对"的情况。这是最轻量的代码贡献。

### 流程

1. 按 [GitHub 工作流](#github-工作流从零开始) fork、clone、建分支
2. 用编辑器打开对应 Mod 文件夹下的 `.cfg`，找到出问题的那行文本改掉
3. 本地放进游戏测一下（见 [本地测试](#本地测试)）
4. 提交、推送、发 PR

### 改动示例

例如 `GameData/0000Tinygrox_CNPatches/MechJeb2/ZH_MJ2.cfg` 里有这样一行：

```
#MechJeb_TargetTrueLongitude = 目标true longitude // Target true longitude - 是啥来着，我以前翻过的，现在忘记了
```

你觉得可以翻成"目标真经度"，那就改成：

```
#MechJeb_TargetTrueLongitude = 目标真经度 // Target true longitude
```

> 行尾 `//` 后面的是英文原文注释，**请保留**，方便后人对照。

### 注意

- 只改翻译值，**不要动本地化键名**（`#` 开头那部分）
- 不要删掉英文原文注释
- 保持文件编码 UTF-8、缩进 4 空格、行尾 LF（详见 [格式要求](#格式要求)）

---

## 贡献方式三：新增 .cfg 翻译补丁

这是本项目最主要的贡献方式。绝大多数部件类 Mod 的文本都可以通过 MM patch 翻译。

### 目录结构

所有翻译文件都在 `GameData/0000Tinygrox_CNPatches/` 下，**每个 Mod 一个文件夹**，文件夹名采用 **CKAN 中的显示名称**：

```
GameData/0000Tinygrox_CNPatches/
├── AtomicAge/
│   └── Localization/
│       └── zh-cn.cfg          ← 本地化替换式
├── NearFutureSolar/
│   └── NFS.cfg                ← 直接 MM patch 式
├── MechJeb2/
│   └── ZH_MJ2.cfg
├── _HarmonyCNPatches/          ← DLL 翻译专用，见"贡献方式四"
│   ├── KSP_Chinese_Patches.dll
│   ├── WhereCanIGo.cfg
│   └── ...
├── ModCNPatches.version        ← 版本元数据
└── ModCNPatches.ckan           ← CKAN 元数据
```

> 文件夹名以 CKAN 显示名为准。如果不确定名字，可在 CKAN 客户端里查。

### 新增一个 Mod 翻译的步骤

1. 在 `GameData/0000Tinygrox_CNPatches/` 下新建一个文件夹，命名为该 Mod 的 CKAN 名称
2. 在文件夹内创建 `.cfg` 文件
3. 编写翻译内容（写法见下文）
4. 本地测试（见 [本地测试](#本地测试)）
5. 更新 `README.md` 的"目前支持的 Mod"列表
6. 提交 PR

### 两种写法

本项目存在两种主要的 `.cfg` 写法，根据目标 Mod 的本地化机制选择：

#### 写法 A：本地化替换式（推荐，大多数 Mod 适用）

如果目标 Mod 本身使用了 KSP 的 `Localization` 机制（即源码里用 `#LOC.xxx` 这类键，并在 `Localization/` 目录下提供多语言 `.cfg`），你也用同样的格式覆盖即可：

```cfg
Localization
{
    zh-cn
    {
        // ********** 部件：nuclearEngineKANDL
        #LOC.aa_nuclearEngineKANDL_title = LV-RTG "蜡烛" SKALOU.v2 放射性同位素火箭
        #LOC.aa_nuclearEngineKANDL_description = 将燃料泵入炽热的RTG放射性堆芯并导入喷管……
    }
}
```

文件路径约定：`<Mod名>/Localization/zh-cn.cfg`。

> 注释用 `//`，可标注部件名方便维护。英文原文可以放在行尾 `//` 后作为对照。

#### 写法 B：MM patch

如果目标 Mod 没有自己实现本地化，以部件为例，文本直接写死在 `title`/`description` 等字段里，则用 MM 的 `@PART` 操作符直接替换：

```cfg
// 替换单个部件的标题和描述
@PART[partName]:NEED[targetModName]:AFTER[targetModName]
{
    @title = 中文标题
    @description = 中文描述
    @tags |= 中文 标签 // tags 一般不改，要改也是增加
}
```

> 需要知道目标部件的 `name`。可以在目标 Mod 的 `.cfg` 或 `ModuleManager.ConfigCache` 里查到。

### 格式要求

项目根目录的 `.editorconfig` 规定了统一格式：

| 要求 | 值 |
|------|-----|
| 编码 | UTF-8（无 BOM） |
| 缩进 | 4 个空格（不要用 Tab） |
| 行尾 | LF（Unix 风格） |
| 文件末尾 | 保留一个空行 |

在 VS Code 安装 `EditorConfig` 插件会自动按这些规则格式化。

### CI 检查：选择器不能有空格

提交 PR 后，`Check CFG Format` 这个 workflow 会扫描所有 `.cfg`，**检测部分 MM Patch 语法是否正确**。例如下面这种写法会报错：

```cfg
// 错：选择器 [Some Part Name] 里有空格
@PART[Some Part Name] { ... }
```

正确做法是用 `?` 代替空格 `[Some?Part?Name]`，`?` 是单字符匹配，属于正则表达式的内容。

### 本地测试

1. 把你改好的 `0000Tinygrox_CNPatches` 文件夹放进 KSP 的 `GameData/` 下（覆盖或新增）
2. 确保 `GameData/` 下已安装 Module Manager（及 Harmony，如果涉及 DLL 翻译）
3. 启动游戏，进入相关场景查看翻译是否生效、是否有报错
4. 检查 `KSP.log` 中 `[ModuleManager]` 和 `[KSPCNPatches]` 相关日志
5. （可选）在 `GameData/` 下查看 `ModuleManager.ConfigCache`，确认你的 patch 被正确应用

> 修改 `.cfg` 后需要重启游戏才能生效（MM 在加载时应用 patch）。

---

## 贡献方式四：Harmony / C# 硬编码翻译

有些 Mod 的文本是写死在 DLL 代码里的（GUI 文本、硬编码字符串等），MM patch 翻译不了，需要用 [Harmony](https://github.com/KSPModdingLibs/HarmonyKSP) 在运行时拦截并替换这些字符串。本项目已经搭好了这套框架。

> 入门概念可参考仓库内的 [Harmony简单入门.md](./Harmony简单入门.md)。

### 环境要求

- **Visual Studio 2022**（或支持 .NET Framework 4.7.2 的 IDE，如 Rider）也可以使用 VS Code + C# Dev Kit
- **.NET Framework 4.7.2**（VS installer 里勾选"）

### 工程结构

C# 源码位于 `Source/KSP_Chinese_Patches/`：

```
Source/KSP_Chinese_Patches/
├── HarmonyInitialise.cs          ← 入口：扫描所有 patch 类并应用
├── StaticMethods.cs              ← 工具方法（如检测 DLL 是否加载）
├── ModPatches/                   ← 每个 Mod 一个 XxxPatches.cs
│   ├── WhereCanIGoPatches.cs
│   ├── RealAntennasPatches.cs
│   ├── RO/
│   └── ...
├── PatchesInfo/
│   ├── AbstractPatchBase.cs      ← 所有 patch 类的基类
│   └── HarPatchInfo.cs           ← 单个 Harmony patch 的信息载体
└── KSP_Chinese_Patches.csproj    ← 工程文件
```

编译产物输出到 `GameData/0000Tinygrox_CNPatches/_HarmonyCNPatches/KSP_Chinese_Patches.dll`。

### 重要：`.csproj` 不在仓库中

因为工程文件里包含本地的 KSP 安装路径，每个人都不一样，**clone 下来后如果不修改会无法直接编译**。

你需要根据自己的实际情况做出修改。在 `Source/KSP_Chinese_Patches/` 下修改 `KSP_Chinese_Patches.csproj`，参考下面内容，**把 `KSPTestDir` 和 `KSPSteamDir` 改成你自己的 KSP 安装路径**：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Library</OutputType>
    <TargetFramework>net472</TargetFramework>
    <LangVersion>default</LangVersion>
    <RootNamespace>KSP_Chinese_Patches</RootNamespace>
    <AssemblyName>KSP_Chinese_Patches</AssemblyName>
    <Deterministic>false</Deterministic>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <AutoGenerateBindingRedirects>false</AutoGenerateBindingRedirects>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
  </PropertyGroup>

  <!-- KSP Path -->
  <PropertyGroup>
    <KSPTestDir>path\to\KSP</KSPTestDir>
    <KSPSteamDir>path\to\KSP</KSPSteamDir>
  </PropertyGroup>

  <PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>

  <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <DebugType>none</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>..\..\GameData\0000Tinygrox_CNPatches\_HarmonyCNPatches\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <WarningLevel>5</WarningLevel>
  </PropertyGroup>

  <ItemGroup>
    <!-- Harmony 2.2.x Path -->
    <Reference Include="0Harmony">
      <HintPath>$(KSPTestDir)\GameData\000_Harmony\0Harmony.dll</HintPath>
      <Private>False</Private>
    </Reference>

    <Reference Include="Assembly-CSharp">
      <HintPath>$(KSPTestDir)\KSP_x64_Data\Managed\Assembly-CSharp.dll</HintPath>
      <Private>False</Private>
    </Reference>

    <Reference Include="$(KSPTestDir)\KSP_x64_Data\Managed\Unity*" Private="False"/>
  </ItemGroup>

  <Target Name="CopyToTestInstance" AfterTargets="Build">
    <Copy SourceFiles="$(TargetPath)" DestinationFolder="$(KSPTestDir)\GameData\0000Tinygrox_CNPatches\_HarmonyCNPatches" SkipUnchangedFiles="true"/>
  </Target>

  <Target Name="CopyToSteamInstance" AfterTargets="Build" Condition="'$(Configuration)' == 'Release'">
    <Copy SourceFiles="$(TargetPath)" DestinationFolder="$(KSPSteamDir)\GameData\0000Tinygrox_CNPatches\_HarmonyCNPatches" SkipUnchangedFiles="true"/>
  </Target>

</Project>

```

如果你没有像我一样专门复制了一份 KSP 作为测试实例，那么两个安装路径可以完全一致，或者全局替换掉，只保留一个路径

再用以下命令创建解决方案文件（可选，方便 VS 打开）：

```bash
dotnet new sln -n KSP_Chinese_Patches
dotnet sln add KSP_Chinese_Patches.csproj
```

> 这两个文件是本地构建用的，**不要提交到仓库**

### 翻译框架的工作原理

1. `HarmonyInitialise.cs` 在游戏启动时运行，自动扫描所有继承自 `AbstractPatchBase` 的类
2. 对每个类，先通过 `PatchDLLName` 检测对应 Mod 的 DLL 是否已加载，没装就跳过
3. 已加载的则调用 `LoadAllPatchInfo()`，应用其中定义的所有 Harmony patch
4. 翻译文本通过 `Localizer.Format("#CNPatches_xxx")` 从对应的 `.cfg` 本地化文件读取

### 新增一个 Mod 的 Harmony 翻译

#### 1) 新建 patch 类

在 `Source/KSP_Chinese_Patches/ModPatches/` 下新建 `XxxPatches.cs`，继承 `AbstractPatchBase`：

```csharp
using HarmonyLib;
using KSP.Localization;
using KSP_Chinese_Patches.PatchesInfo;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace KSP_Chinese_Patches.ModPatches;

public class XxxPatches : AbstractPatchBase
{
    public override string PatchName => "Xxx Mod 在 CKAN 中的名字";
    public override string PatchDLLName => "XxxDllName";  // Mod 实际不带 .dll 的程序集名
    
    // 根据情况添加 Prefix、Postfix 或 Transpiler Patch
   
    // Transpiler：在 IL 层把字符串常量替换成本地化结果
    private static IEnumerable<CodeInstruction> ReplaceStrings(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions).Start();
        while (matcher.Pos < matcher.Length)
        {
            matcher.MatchStartForward(new CodeMatch(OpCodes.Ldstr));
            if (!matcher.IsValid) break;
            string original = matcher.Operand as string;
            if (original != null && original == "xxxxx")
                matcher.SetOperandAndAdvance("yyyyy");
            else
                matcher.Advance(1);
        }
        return matcher.InstructionEnumeration();
    }

    protected override void LoadAllPatchInfo()
    {
        Patches = new HashSet<HarPatchInfo>()
        {
            new HarPatchInfo
            (
                AccessTools.Method(AccessTools.TypeByName("XxxNamespace.XxxClass"), "XxxMethod"),
                new HarmonyMethod(typeof(XxxPatches), nameof(XxxPatches.ReplaceStrings)),
                HarmonyPatchType.Transpiler
            ),
        };
    }
}
```

说明：
- `PatchName` 用 CKAN 显示名；`PatchDLLName` 是程序集名（不带 `.dll`），框架靠它判断 Mod 是否安装
- `LoadAllPatchInfo()` 里把目标方法和你的 patch 方法配对，类型用 `HarmonyPatchType.Transpiler` / `Prefix` / `Postfix` / `Finalizer`
- 查找目标方法需要先用 [dnSpy](https://github.com/dnSpy/dnSpy) 反编译目标 Mod 的 DLL，找到类名、方法名和里面的硬编码字符串

#### 2) 新建配套的本地化文件

在 `GameData/0000Tinygrox_CNPatches/_HarmonyCNPatches/` 下新建 `Xxx.cfg`，存放上一步引用的所有 `#CNPatches_xxx` 键：

```cfg
Localization
{
    zh-cn
    {
        #CNPatches_Xxx_SomeText = 某段中文翻译 // Some string
        #CNPatches_Xxx_Another = 另一段中文 // Another string
    }
}
```

命名规范：翻译键统一以 `#CNPatches_<Mod缩写>_<标识>` 命名，英文原文用 `//` 注释保留。

#### 3) 编译与测试

1. 用你配好的 `.csproj` 编译（Debug 或 Release）
2. Release 配置会直接输出到 `GameData/0000Tinygrox_CNPatches/_HarmonyCNPatches` 同时会自动复制到游戏安装目录，前提是在 `.csproj` 里指定好了路径
3. 确保游戏已安装 Harmony（`GameData/000_Harmony/0Harmony.dll`）
4. 启动游戏，查看 `KSP.log` 中 `[KSPCNPatches]` 开头的日志，确认你的 patch 被加载、应用
5. 进入对应 Mod 的界面验证翻译生效

### C# 代码风格

`.editorconfig` 已定义了完整规则，关键点：

- 缩进 4 空格，UTF-8，LF
- 大括号换行，采用微软标准
- 尽量遵循 `private` 字段 `_camelCase`，`private static` 字段用 `s_camelCase`，`const` 变量用 `PascalCase`
- 优先用具体类型而非 `var`（类型明确时才用 `var`）

---

## 提交前的检查清单

发 PR 前请逐项确认：

- [ ] 基于 `dev` 分支新建的工作分支（不是 `main`）
- [ ] MM patch 语法正确
- [ ] `.cfg` 本地化文件里保留了英文原文注释（`//` 后）
- [ ] 已在 KSP 中实际加载测试，翻译生效、无报错
- [ ] PR 的 base 分支选的是 `dev`
- [ ] PR 标题和描述清楚说明了改了哪些 Mod 的什么内容

---

## 分支模型与发布流程

本项目采用 `dev` + `main` 双分支模型：

```
你的 fork ──PR──> dev ──PR(维护者)──> main ──CI 自动发布──> Release
```

- **`dev`**：开发分支，PR 都要往这里提
- **`main`**：稳定发布分支，只有我从 `dev` 往 `main` 发 PR

### 自动发布机制

当我把 `dev` 到 `main` 的 PR 合并时，`.github/workflows/main.yml` 会自动：

1. 从 `CHANGELOG.MD` 读取最新版本号和更新日志
2. 打包 `GameData/` 为 zip
3. 创建 GitHub Release 并发布

所以**发行完全由我控制**，你无需关心发布流程，只需要：

- PR 往 `dev` 提
- 如果你的改动值得记录，在 `CHANGELOG.MD` 顶部加一条（格式见下）

### CHANGELOG 格式

```markdown
## [1.8.4]

### 新增

- 新增 XXX Mod 的中文翻译 - [@你的用户名](https://github.com/你的用户名)

### 修复

- 修正 YYY 中"氢气"的翻译错误
```

- 版本号遵循 `主.次.修订`（如 `1.8.4`）
- 分类用 `### 新增` / `### 修复` / `### 变更` 等
- 大的改动建议带上贡献者署名和 PR 链接
- 不确定怎么升版本号就留给我，在 PR 说明里写清改动即可

---

## 翻译风格与术语建议

为保证整个项目风格统一，翻译时建议遵循以下原则：

1. **术语统一**：航天专业术语尽量沿用社区惯用译法（如 apoapsis=远拱点、periapsis=近拱点、thrust=推力、specific impulse=比冲）
2. **保留专有名词**：部件型号、厂商名（如" PorkWorks"）、Mod 内造词可保留英文或在首次出现时中英并列
3. **描述要"说人话"**：尽量符合中文表达习惯；长句适当断句
4. **保留原文对照**：尽量在 `.cfg` 行尾用 `//` 注释保留英文原文，方便后续校对
5. **不翻译的元素**：`tags` 字段里的搜索关键词、`#autoLOC_` 等已有的键名本身不要动
6. **遇到拿不准的造词**：可以在 PR 描述里说明并征求意见，或者干脆不译

---

## 许可与署名

- 本项目采用 **CC BY 4.0** 协议，详见 [README.md - 许可协议](./README.md#许可协议--license)
- 你提交的翻译内容将被视为在同一协议（CC BY 4.0）下贡献给本项目
- 重大新增翻译会在 `CHANGELOG.MD` 中署名（GitHub 用户名 + 链接），请在 PR 描述里注明希望如何署名

再次感谢你的贡献！有问题欢迎直接提 Issue 或在 PR 里讨论。
