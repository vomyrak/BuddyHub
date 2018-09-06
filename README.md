# BuddyHub
### The Universal Controller Platform for Assistive Technology
**BuddyHub is a free and open-source platform that leverages the Internet of Things (IoT) to allow people with different disabilities to control various output devices in their environment via accesible inputs through a simple-to-navigate and user-friendly interface.**

## About
BuddyHub was born out of a joint collaboration between Imperial College London and Wooden Spoon: The Children's Charity of Rugby, the latter of whom invests heavily in assistive technologies and aims to promote their development and make them more accessible to end users.

BuddyHub aims to be an affordable, end-to-end solution for people with disabilities to utilize smart devices in their surroundings through a variety of accessible input devices that caters to their individual needs in a plug-and-play fashion. This is all done through the user interface of BuddyHub, which was designed with a special needs audience in mind. For instance, a user with limited motion capabilities could control smart appliances, robotic arms and other forms of hardware they might need, as long as they are integrated into BuddyHub's system.

This project also aims to promote the development of assistive technology by providing a platform for developers and companies to build on and integrate their products with. The ultimate goal is to make such technologies more affordable as well as integrate them with the Internet of Things for the benefit of end users. As more devices become interconnected, this universal controller platform can act as a central hub of control for these devices, giving users with various disabilities the ability to utilize a wide range of devices that cater to their individual needs.

The team that developed the platform focused on establishing the groundwork and software architecture to achieve the above goals. The first iteration of the product demonstrates the viability and usefulness of such a universal and accessible software platform that can be used for controlling technologies in a familiar and easy manner by people with disabilities. Due to time constraints, not all features we wanted to implement / issues we wanted to fix were resolved; the team hopes that a community of developers / other students could collaborate and improve on what has been done so far.

## Installation and Setup
BuddyHub currently has web and desktop (Windows) versions, both of which are functional and augment the intent of having a user-friendly and accessible UI for end users.

To access the web version of BuddyHub, please visit [BuddyHub (Web)](http://wsurop18-universal-controller.herokuapp.com/).

To install the Windows version, follow the steps below:
* [Download zip file from here.](http://wsurop18-universal-controller.herokuapp.com/buddyhub-2018-08-31.zip) Extract the zip file to desired location. Among all other files, there should be two separate executables that include the server and the desktop UI.
* To run BuddyHub, run server.exe followed by UCUI.exe to launch the UI.

## Guide to BuddyHub's UI
Detailed below are the accessibility settings that we have implemented on the web and desktop versions of BuddyHub's UI. This should assist future developers should they wish to edit current / add new accessibility features.

To configure the accessibility settings of BuddyHub, check out this [guide](https://github.com/vomyrak/BuddyHub/blob/master/accessibility.md).

### BuddyHub (Web)
![Web Screenshot](https://user-images.githubusercontent.com/33024429/44993438-7fdbd700-af92-11e8-89af-382574926858.png)

### BuddyHub (Desktop)
![Desktop Screenshot](https://user-images.githubusercontent.com/33024429/44993218-b7964f00-af91-11e8-8178-2ab4fd17444d.png)

## List of Compatible Devices (Updated: 30/8/18)
To demonstrate the universal plug-and-play capabilities of the prototype that the team developed, we cross-tested the universal controller platform with the following input and output devices:

#### Input Devices:
(_Any input devices that are natively mapped to a PC's mouse and keyboard inputs can be used to interact with BuddyHub's UI._)
* Tobii Eye Tracker 4C
* Most accesible joysticks (we used _Slimline Joystick_)
* Buddy buttons

#### Output Devices:
* Web-based Voice Synthesizer for Alexa (allows a user with limited vocal capabilities to interact with Alexa through BuddyHub)
* Philips Hue Go ([API Documentation](https://www.developers.meethue.com/documentation/getting-started))
* Lynxmotion AL5D PLTW robotic arm (we wrote a C# API to interact with the robotic arm's native firmware)

## Add a Device (Developer Documentation)
Wish to integrate a device with BuddyHub?

#### Requirements:
* RESTful Web API that accepts HTTP requests (for devices with a TCP/UDP connection)
* C# wrapper that implements our IDevice interface (for USB devices)
* Easy-to-understand device names and descriptions for hardware functionality
* Supports API authentication via OAuth 2.0 and hardware authentication (_to be implemented_)

#### Guide:
* Visit [BuddyHub@Developers](https://developers-buddyhub.herokuapp.com/) to add a new device
* Choose to add a device with either a RESTful API or a C# interface
    * C# libraries have to implement an C# IDevice interface
    * Link your project to the IDevice interface is under UCUI/UCUI/UCUI/CSharpServer/IDevice.cs (_to be relocated_)
    * Include ```using System.ComponentModel.Composition```
    * Immediately above your main object class, add a line ```[Export(typeof(CSharpServer.IDevice))]```
* Fill up the form with user-friendly names and descriptions (these will be what users see in the main BuddyHub UI)
* Specify the HTTP methods and resources that BuddyHub will use to interact with your device
* Select if a text input interface is required for your device
* Submit your device for approval by the admin team and we'll get back to you as soon as possible

#### Device suggestions:
Users can also suggest devices to be added to BuddyHub by clicking on the ["Suggest Device"](https://wsurop18-universal-controller.herokuapp.com/contact) button on BuddyHub's UI. Simply just fill in the form and the details will be sent for a review. The admin team will look into adding the suggested devices on a regular basis.

## Future Improvements
Wish to contribute to BuddyHub? Fork the repo and follow the installation guide as shown [here](https://github.com/vomyrak/BuddyHub/blob/master/web/README.md).

List of features we wish to implement but have not:
#### Accesibility
* One and two-switch modes for navigation
* Tremor filtering for push buttons
* Adjustable wait time for click-on-hover option
* Accesibility options for the visually impaired such as screen reader support
* Implementing clear and intuitive button icons for the web interface
* Ability to save user settings and preferences
* Ability to add futher language options
* Integration with other softwares and open APIs to contain day-to-day applications within BuddyHub's interface (e.g. instant-messaging apps, Fitbit etc.)
* Integration of an AAC (Augmentative and Alternative Communication) board alongside the current text-to-speech capability
* Setting up of macros
* Android and iOS mobile app
* More audio-visual feedback options

#### Security
* API authentication via OAuth 2.0
* Hardware authentication to register device's IP Address
* GDPR compliance to ensure user's data security

#### Development
* An interface for admins to approve and process device suggestions and submissions
* Improve on the workflow for developers to integrate new devices to BuddyHub
* CLI (Command Line Interface) tool for the integration of new devices
* Adding more devices to BuddyHub and possible integration with other smart hubs such as Apple HomeKit and Samsung SmartThings

List of issues can be found [here](https://github.com/vomyrak/BuddyHub/issues).

## Contributors and Acknowledgements
This project was initiated and funded by Wooden Spoon: The Children's Charity of Rugby, and was developed by a team of 10 undergraduate students from Imperial College London under the supervision and guidance of _Dr. Ian Radcliffe_ from Imperial College London, Department of Bioengineering.

The team comprised of _Maksim Lavrov, May McCool, Terence Seow, Rachel Tan, Yong Shen Tan, Sung Kyun Yi, Fiona Boyce, Joan Tso, Balint Hodossy and Husheng Deng_.

The team would like to thank the following people for their contributions and advice to the team throughout the course of the project:
  
  * _Mahendran Subramanian_, _Balasundaram Kadirvelu_ and _Pavel Orlov_ from Dr. Faisal's Research Group, Imperial College London
  * _Paschal Egan_ and _Niraj Kanabar_ from Imperial College London, Department of Bioengineering
  * _Dr. Thomas Clarke_ from Imperial College London
  * _Ben Crundwell_ from Cambridge Design Partnership
  * _Barrie Ellis_ from Special Effects
  * _Charlie Danger_, _Simon Bull_, _Christian Dryden_, _Rachel Moore_, _Hélio Lourenço_ and _Diane Arthurs_ from ACE Center

## Contact
Should you have any queries about the above project or wish to find out more about how you could contribute, please kindly reach out to us at wsurop2018@gmail.com.
