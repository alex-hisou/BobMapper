# BobMapper
A tool to create maps for Robbery Bob: Man of Steal. Created by Alexan's Soul with additional help by Naves.

### Contact info for bugs

alexans.soul@gmail.com

## Requirements
Minimum OS version - Windows 7, 64-bit

[.NET 8 Desktop Runtime](https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.29/windowsdesktop-runtime-8.0.29-win-x64.exe)

*For the Android tools* [Java Runtime Enviroment 8](https://javadl.oracle.com/webapps/download/AutoDL?BundleId=253458_ba687cb3cbb24342adc8fdf890b993dc)

## Downloading

 1. [Download the zip from the latest
    release](https://github.com/alex-hisou/BobMapper/releases/latest)
 2. Extract it in any directory that doesn't require admin privliges (Documents is recommended)
 3. Download the needed resources in the next paragraph
 
 ### (Advanced) Build Instructions
 To build the program yourself, clone the repo:
 
	 git clone https://github.com/alex-hisou/BobMapper.git
   
To build:

	dotnet build BobMapper.sln

Note: The repo doesn't include the tools, dependencies or runtimes. It only includes the BobMapper program

## Needed resources
**For the Steam version** - [Decrypted resources.dat](https://drive.google.com/file/d/1pevtwpo5c4094R50vWa1QcOm9JqMuBGL/view?usp=sharing)

**For the Android version** - [Decompiled files from apk](https://drive.google.com/file/d/1FitSAD96k9nVp6HoTE7XwP0A5u42kiBN/view?usp=sharing)

## Modding Instructions

### Steam
1. Replace the resources.dat file in the Robbery Bob directory with the decrypted version (Default location is C:\Program Files(x86)\Steam\steamapps\common\Robbery Bob\)
2. Open the newly-replaced file with a zip tool.
3. Navigate to common\Levels within the archive
4. Replace the .lev files in the archive (Use Levels.xml in the same directory as a reference for what files to replace)

### Android

1. Back up your save data to Google Play and uninstall Robbery Bob
2. Open a Powershell window
3. Type in the following command:


  java -jar (drag apktool file from BobMapper\Tools) b (drag modded files)


4. Once its done, type in the next command:


  java -jar (drag uber apk signer) --apks (drag apk from apktool)


5. Connect your phone through USB.
6. Enable USB debugging from the developer options ([Tutorial for how to enable developer options](https://developer.android.com/studio/debug/dev-options))
7. Open the platform tools folder in Powershell and type in the following command:


  .\adb install -r -d (drag in the aligned debug signed apk that uber apk signer made)

## Important info for using the editor

* Overlapping walls break NPC pathfinding. Avoid them
* Make sure every room is sealed.
* If a horizontal wall intersects a vertical wall, you need to split the vertical wall in two at the intersection point
* Some textures will not display ingame. For any texture that does that, contact the developers.
* You can right click with the Change Floor tool selected to 
* Currently missing features include: Elevators, room shadow vfx, apartment levels, night levels, big buttons, cables, changing map size after level creation, direct file injection.
