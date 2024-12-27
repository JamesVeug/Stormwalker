# 0.8.2
## Fixes
- Fixed non-stormwalker mod hotkeys being reset

# 0.8.1
## Fixes
- Works with ATS 1.5.5R
- Fixed shift on Lakes not selecting Small Fishing Hut
- Change hotkeys to avoid errors when using the same key for multiple actions
  - WoodCutters Balance Hostility is now `]`
  - WoodCutters Unassign All is now `,`
  - WoodCutters AssignOne is now `]`
  - Zoom Overview is still `Backspace`

# 0.8.0
## Fixes
- Works with ATS 1.4.15R
- Can now change hotkeys in the options menu
  - Changes made to the hotkeys are saved to `AppData\LocalLow\Eremite Games\Against the Storm\API\Stormwalker.custombindings`

## Changed
- New Icon (Thanks to Thin Creator!)
- Pressing shift on Ores now starts a Mine
- Pressing shift on Lakes now starts a Fishing Hut

# 0.7.0
## Fixes
- Works with ATS 1.3.4R

## Changed
- Moved hotkeys and settings to configs, so they can be easily changed!
- Changed save data directory to be in `%localappdata%low/Eremite Games/Against the Storm/Stormwalker.save`


# 0.6.0
## Changed
- Works with ATS Full Release (1.0.1)
- Now requires x64 version of BepinEx. README adjusted accordingly.

## Removed
- Middle Click trader match offer function is now in the base game


# 0.5.0
## Changed
- The Shift+X and Ctrl+X Hotkeys now directly act as hotkeys for left and right-clicking the Unassign Woodcutters button
- Had to move the Unassign button because it eclipsed the 5x speed button

## Fixed
- Alt+X No longer tries to unassign woodcutters to reach negative hostility levels.

## Removed
- Placing paths with P was removed, as building shortcuts provide a more general solution.


# 0.4.0
## Added:
- The copy building key now also works on trees, ore and geysers

## Fixes:
- Warning icon for villagers scheduled to leave a building will now show correctly
- Fixed off-by-1 logic when using Alt+X to unassign woodcutters
- Slightly better saving logic

## Removed:
- Working area for mines; With the ore placement change in the base game this has become redundant.
- Consumption Control on I: This now exists in the base game.


# 0.3.0
## Additions
- You can now put a limit on amount of residents allowed in an individual house. This should help with getting exact amounts of people into your hearth upgrades
- Use Ctrl+RightClick to schedule a worker to leave their workplace after their next production finishes.

## Bugfixes
- Fixed rounding errors when trying to match a trader's offer in the trade panel.
- You can no longer use Shift in the fog to discover hidden resources.


# 0.2.0
## Added
- See your City Score in the trade menu
- Use Middle Click on trader menu goods to automatically match the offer.
- Unassign woodcutters with Shift+X (unassign all) and Alt+X (unassign to decrease hostility by 1)
- You can Ctrl+Click items in the ingredient wheel to open the recipe panel
- Consumption Control is now tied to the I hotkey

## Changed
- Creating a gathering camp by hovering over a resource node has been changed to obey the new "Copy Building" keybind

## Removed
The following are now part of the base game and have thus been removed:
Moving the trading post
- Ctrl-click and Shift-click to increase production limits.


# 0.1.0
First Release of Stormwalker