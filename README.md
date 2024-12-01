# Stormwalker
*An Against the Storm Tweakpack*

A variety pack of convenience features popularly requested for [Against the Storm](https://www.gog.com/game/against_the_storm). No real theme or direction, just things I found useful and easy to mod mashed together to hopefully be useful to others.

**This mod is currently InDev**. The code is a mess, there might be bugs, and the keybinds haven't been integrated into the options menu and as a result they aren't configurable. It is functional, but I'm hoping to go back and clean the mod up soon.

## Features

- Hotkeys for woodcutter assignment button: Immediately unassign all woodcutters using `Shift+X` and assign one using `Ctrl+X`. 
- Additionally, use `Alt+X` to automatically unassign just enough woodcutters to lower Hostility level by one.
- Trader menu now also shows your City Score (an internal counter that determines which traders can spawn).
- `Ctrl+Click` on items in the ingredient wheel will bring you to the recipe panel, similar to left-clicking on other goods icons.
- Overview zoom Mode. Press `Backspace` in game to immediately gain a better perspective on your map. Press `Backspace` again to zoom back in.
- Hovering over a resource deposits, geysers, ores or trees and pressing the *Copy Building* key (default `Shift`) will request the construction of the associated building.
-  `5x` speed mode. Available as additional UI button and the `5` keybind. Gotta go fast!
- `Ctrl+Right Click` workers in a building to schedule them to leave their position as soon as their next production cycle finishes
- You can now set a limit on the amount of villagers allowed in an individual house

## Configs

Hotkeys and settings of Stormwalker can be configured to your liking.
Use Thunderstore or R2modman to modify these or manually edit the file in `BepInEx/config/Stormwalker.cfg`.


## Installation

Using Thunderstore (recommended):
1. Download and setup the Thunderstore app: https://thunderstore.io/
2. Using the thunderstore app install the Stormwalker mod

Manual installation:
1. Download BepInEx 5: https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x64_5.4.22.0.zip
2. Extract into your Against the Storm game directory
3. Grab the latest release of this mod from https://thunderstore.io/c/against-the-storm/p/StormwalkerDevs/Stormwalker/ using the Manual Install button
4. Extract into the same Against the Storm game directory so the .dll is inside `BepInEx/plugins`
5. Run the game

Note that I bundle a BepInEx config with my release, which changes the loader's entry point. That's all it does. If you know what you're doing you can make the required changes yourself and just use the .dll




## Disclaimer

Not associated with Eremite games. Eremite Games can't offer assistance with modded games and newer game versions may or may not break your mods. You install mods at your own risk. You may want to backup your save files from `%userprofile%\appdata\locallow\Eremite Games` prior to installing this mod.

This mod is a fork from [github](https://github.com/ats-mods/Stormwalker) and made to work with the latest ATS version due to high demand.