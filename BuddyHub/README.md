# BuddyHub Core Controller & Desktop User Interface

## File Structure
This folder contains a single Visual Studio 2019 solution that comprises the a few subprojects in their respective folders. To explore and demonstrate the portability of the project in various platforms, projects are created with different versions of C# frameworks provided by Microsoft.

* UCProtocol
    * This project implements internal protocols shared by the universal controller, desktop UI, and are intended for use by plug-in projects as well. It implements internal data representations, IDevice plug-in interface, server core, and internal server definitions. This project is targeted at .Net Standard 2.0 and is cross-platform.

* UCUtility
    * This project implements **WINDOWS-SPECIFIC** components of the universal controller. Currently these include audio playback engine (using NAudio .Net library that uses DirectSound), and a network manager class that executes Windows netsh commands. For the universal controller to work on other platforms, these components have to be re-written for the desired platform. This projectis targeted at .Net Framework 4.6.1.

* Server
    * This project implements the core server of the universal controller. The server interacts with remote database, controls USB and wireless smart devices, receives requests from related local applications. This project is targed at .Net Core 2.1 and is cross-platform.

* USBManager
    * This project implements a **WINDOWS-SPECIFIC** manager application that inspects USB devices and tracks USB events. It is not directly linked to the server in anyway. It communicates with the server via internal http requests and uses message definitions implemented within UCProtocol. This project is targeted at .Net Framework 4.6.1, and it has to be re-written for different platforms. 

* DesktopUI
    * This project implements the **WINDOWS-SPECIFIC** desktop user interface of the universal controller using Windows Presentation Foundation (WPF) framework. It is not directly linked to the server in anyway. It communicates with the server via internal http requests and uses message definitions implemented within UCProtocol. This project is targeted at .Net Framework 4.6.1, and it has to be re-written for different platforms. 

In addition, an example implementation of hardware device control library is included in ./USBDevices/Lynxmotion. This project is adapted from [Lynxmotion SSC32/AL5x Robotic Arm Library](https://github.com/remyzerems/Lynxmotion) by implementing IDevice interface. 

## How to use the library
* To add new USB device
    * Link your project with UCProtocol and UCUtility projects under ./BuddyHub/
    * In your main device object source file, link your project to System.ComponentModel ```using System.ComponentModel.Composition```
    * Implement in your main object class using IDevice interface.
    * Export your main object class using ```[Export(typeof(CSharpServer.IDevice))]```

* To develop the universal controller
    * No addition preparations required except for recover the required NuGet packages using Visual Studio.

## How to test the library
* To test the server
    * Create a Mongo database, and put the details in *auth.json* under .bin/Server in the following format:
    ```
    {
      "user": "<username of your database>",
      "pass": "<password of your database>",
      "dns": "<IP address of your databse>"
    }
    ```
    * Build debug configuration with Visual Studio and run it in debug mode
    * You can also place a copy of it under .bin/ServerExe. If you publish the server project as a exe, that will be the default working directory. Note that exe file should be executed with administrative previledge.

* To test the desktop UI
    * Compile the DesktopUI project and build it for debug
    * Run the server in any mode
    * Run the desktop UI
