## Minecraft Mod Updater
This is a simple mod updater for Minecraft. (Currently only in Dutch but I may add English in the future)

It's written in C# using Microsoft Visual Studio 2017

It uses the following [NuGet](https://www.nuget.org/) packages:
* [DotNetZip v1.10.1](https://www.nuget.org/packages/DotNetZip/)
* [Fody v2.1.0](https://www.nuget.org/packages/Fody/)
* [Virtuosity.Fody v1.20.0](https://www.nuget.org/packages/Virtuosity.Fody/)
* [Costura.Fody v1.6.2](https://www.nuget.org/packages/Costura.Fody/)


## How To

I made this program to make it easy for clients to update the mods that my Minecraft server is using. But everyone can use it.

* First you have to select the Minecraft folder. There has to be a 'config' and a 'mods' folder in the folder.
* Select the update zip.
    * The update zip has to include a config.txt but it may be empty
    * The updated mods have to be in a 'mods' folder in the zip
    * The config files have to be in a 'config' folder in the zip
 
 For example:
 ```
 update.zip - /mods/
            - /config/
            - /config.txt
 ```

**config.txt**
The config.txt is used to delete the old mods that you are replacing. The files are seperated by a comma `,`
For example:
```
CoFHLib-[1.7.10]1.1.2-182.jar,ComputerCraft1.75.jar,D3Core-1.7.10-1.1.2.41.jar
```
