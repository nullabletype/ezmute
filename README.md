# ezmute 
*(easy mute)*

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/nullabletype/ezmute/ezmute%20Console%20Build?logo=github)

A configurable and easy to use way to mute your microphone/input devices on Windows Vista, 8, and 10

## What can it do?
- Provides a super quick way to mute and unmute your input devices including microphones at the operating system level
- Provides a configurable button to mute/unmute your input devices
- Provides configurable hot keys to toggle mute
- Shows current status by text and colour in the main window
- Shows current status in the "system tray"/notification area
- Shows current status in the start bar
- Supports always on top
- Mute status shown in Microsoft Teams
- Can configure the size of the app, starting position, font size and colours

## Installation
Just download from the releases tab, edit the config.json and run the exe. There are four versions available:

| Name | Description |
| -- | -- |
| *-win-x64 | 64 bit version. Requires [net core 3.1 runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1) or later. |
| *-win-x86 | 32 bit version. Requires [net core 3.1 runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1) or later. |
| *-standalone-win-x64 | 64 bit version. Doesn't require any further installs but is considerably larger. |
| *-standalone-win-xx86 | 32 bit version. Doesn't require any further installs but is considerably larger. |

## Configuration
Once downloaded open config.json in your favourite text editor. Each setting is described in there.

## Credits
- ezmute uses [NAudio](https://github.com/naudio/NAudio) to do the heavy lifting of talking to devices.
- The tray icon is provided via [NotifyIcon](https://github.com/hardcodet/wpf-notifyicon).
- Keyboard shortcuts are provided via [NHotkey](https://github.com/thomaslevesque/NHotkey).

♥
