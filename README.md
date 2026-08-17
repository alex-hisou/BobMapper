# BobMapper

A tool to create maps for Robbery Bob: Man of Steal. Created by Alexan's Soul with additional help by Naves.

### Contact info for bugs

alexans.soul@gmail.com

## Requirements

Minimum OS version - Windows 7, 64-bit

[.NET 8 Desktop Runtime](https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.29/windowsdesktop-runtime-8.0.29-win-x64.exe)

[Java Runtime Enviroment 8](https://javadl.oracle.com/webapps/download/AutoDL?BundleId=253458_ba687cb3cbb24342adc8fdf890b993dc)

[Python 3 Runtime](https://python.org/downloads/release/pymanager-263)

## Downloading

 1. [Download the zip from the latest
    release](https://github.com/alex-hisou/BobMapper/releases/latest)
 2. Extract it in any directory that doesn't require admin privliges (Documents is recommended)
 
 ### (Advanced) Build Instructions
 To build the program yourself, clone the repo:
 
	 git clone https://github.com/alex-hisou/BobMapper.git
   
To build:

	dotnet build BobMapper.sln

Note: The repo doesn't include dependencies or runtimes. It only includes the BobMapper program and certain third-party tools.

## Included third-party tools

The BobMapper program includes the following programs within its installation:
* BK Crack
* Uber APK Signer
* APKtool
* Android Platform Tools

## Modding Instructions

### Steam

1. Set the steam resources.dat directory in the prefrences. (Most commonly in C:/Program Files(x86)/Steam/steamapps/common/Robbery Bob/)
2. Use the inject and play (Steam) option.

### Android

1. Download Moddable Robbery Bob 1.zip from the inject tab.
2. Use the inject tool.
1. To make an apk, use the inject and make apk option.

## Important info for using the editor

* Overlapping walls break NPC pathfinding. Avoid them
* Make sure every room is sealed.
* Some textures will not display ingame. For any texture that does that, contact the developers.
* You can right click with the Change Floor tool to rotate a floor tile.
* Currently missing features include:  Dialogue editor, room shadow vfx, big buttons, cables
