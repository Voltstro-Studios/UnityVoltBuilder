# UnityVoltBuilderTool

[![License](https://img.shields.io/github/license/voltstro/UnityVoltBuilderTool.svg)](/LICENSE)
[![Discord](https://img.shields.io/badge/Discord-Voltstro-7289da.svg?logo=discord)](https://discord.voltstro.dev) 
[![YouTube](https://img.shields.io/badge/Youtube-Voltstro-red.svg?logo=youtube)](https://www.youtube.com/Voltstro)


An in editor, modular tool to make Unity game builds easier and more convenient.

# Features

The main feature of this tool is that its modular! It is broken down into 3 sections, scene settings, build settings and game builder.
You can modify or provide your own modules for any of these sections using the interfaces.

The default modules provide these features:
* Scene import from build settings, re-arrange in tool
* Change build target, if it’s a dev build, copy PDB files and/or if it’s a server build
* Copy files to build folder on successful build
* Zip build folder on successful build
* Build bundles
* Build game as full or scripts only
* Do a new build in a new folder with the current date

# Installation

Please read all of the install instructions before installing!

## Dependencies

This tool does require Newtonsoft.Json to be in your project, and by default comes with the library included in the package, with it set not to be included with the build.
If you already have Newtonsoft.Json included in your project, make sure to de-selected it in the package import window!

## Install the package

Download the latest package release from [releases](https://github.com/Voltstro/UnityVoltBuilderTool/releases), and import the package like a normal Unity package.

Everything is put into the `Assets/Editor/VoltBuild` folder.
The default gamebuilder module includes zip functionality, using the default C# `System.IO.Compression`. By default, Unity doesn’t include this, however you can make it include it by adding/modifying the `csc.rsp` file. The package does come with this file included, however if you already have a `csc.rsp` file in your project, you will need to modify it and add:

```
-r:System.IO.Compression.dll
-r:System.IO.Compression.FileSystem.dll
```

# Using the tool

To use the tool, go to Tools **->** Volt Build **->** Build Tool. It will open up a screen looking like this: (default)

![Preview](preview.jpg)

Its recommended to dock the window somewhere for convince.

If you are using the default build config and default builder, you can change what files are copied to the build folder by going to Tools **->** Volt Build **->** Copy Files On Build Editor.

## Supported Versions

This tool was built and tested for Unity 2019.3 on Windows. 

Other OSes should work; however, no testing has been done on them.

Other editor version may not work either, but as long as Newtonsoft.Json works and those IO.Compression dlls are included, this tool *should* work.

## Modifying the tool for your project

This tool is designed to be modfied, you can still use the default one if you want!

Like I said before, the build tool is broken down into 3 sections using interfaces. You can modify what class the tool will use in `BuildTool.cs` under OnEnabled().

## Using a custom config

You may want to use a custom build config to save settings that you need in your build settings. For example, you might want to save if you want to copy the build or not.

To use a custom build config, go to `ConfigManager.cs` and in NewConfig() change the BuildOptions to use your own class that implements `IBuildConfig`.

After that you can use `ConfigManager.GetBuildConfig(out T config)` to get a custom build config. E.G:

```csharp
if(ConfigManager.GetBuildConfig(out MyExampleConfig config))
{
	config.CopyBuildToShare = true;
}
```

# Authors
Voltstro – *Initial Work* – [Voltstro](https://github.com/Voltstro)

# License
This project is licensed under the Apache-2.0 license – see the [LICENSE](/LICENSE) file for details.
