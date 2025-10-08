# FF1 Pixel Remaster Archipelago Mod

This is a mod to add Archipelago support to Final Fantasy 1 Pixel Remaster (FF1 Pixel Remaster).

## Installation
- Must use a compatible PC version of FF1 Pixel Remaster on the latest update.

- Download the appropriate IL2CPP release of [BepInEx 6](https://github.com/BepInEx/BepInEx/releases/download/v6.0.0-pre.2/BepInEx-Unity.IL2CPP-win-x64-6.0.0-pre.2.zip).

- Extract the BepInEx zip folder you downloaded from the previous step into your game's install directory (For example: C:\Program Files (x86)\Steam\steamapps\common\FINAL FANTASY PR)
  - Launch the game and close it. This will finalize the BepInEx installation.
- [Download and extract the `FF1PRAP.zip` file from the latest release.](https://github.com/wildham0/FF1PRAP/releases/latest)
  - Copy the `FF1PRAP` folder from the release zip into `BepInEx/plugins` under your game's install directory.
- Launch the game again and you should see the Archipelago connection windon on the title screen.
- To uninstall the mod, either remove/delete the `FF1PRAP` folder from the plugins folder or rename the winhttp.dll file located in the game's root directory (this will disable all installed mods from running).


## Generating a Multiworld with Archipelago
- Head to the [Archipelago Setup Guide](https://archipelago.gg/tutorial/Archipelago/setup/en) and follow the 'installing the archipelago software' section.
- Launch the Archipelago Launcher and click `Browse Files`.
- Copy the `ff1pr.apworld` from the [latest randomizer release](https://github.com/wildham0/FF1PRAP/releases/latest) into the `lib/worlds` or `custom_worlds` folder of Archipelago.
  - Note: If you install the apworld by double clicking it, it will cause errors later on. Make sure there is only one `ff1pr.apworld` in either the `lib/worlds` folder or `custom_worlds` folder.
- In the Archipelago Launcher, click `Generate Template Options` to create a template yaml with the new options from the apworld file.
- Edit the yaml and place it (along with any others) in the Archipelago `Players` folder. 
- Press `Generate` in the Archipelago Launcher, and find the resulting zip file in the `output` folder.
- Once you have the generated zip file, upload it (to the Archipelago website)[https://archipelago.gg/uploads].


### Hosting the game on the website
- Click `Create New Room`, and you will see something like `/connect archipelago.gg:38281`. Archipelago.gg is the host and the last 5 numbers here are the port number, which you will need for the next step.
- Launch the game and select `Archipelago` on the Title Screen, then click `Edit AP Config` and fill in your connection details. Player name must match what you entered in the first step, and hostname/port should match the info from the previous step.
- All that's left is to press Connect, and if it says `Connected`, simply start a New Game and have fun!


## Main Features
- Items Shuffle
- Entrances Shuffle
- Monster Parties Randomization
- McGuffin Hunt Feature


## Helpful Tips
- The King is not a location.
- The Bridge and the Canal are not Items. The Bridge will be built or not depending on your options, and Nerrick always build the Canal.
- The Giant is moved by a few tiles so that his Chests cannot be accessed without the Star Ruby.
- The Caravan check is always at the Caravan.
- When shuffling Entrances, the Caravan is an entrance that can be shuffled, don't forget about it.


## Credits
- silent-destroyer and Scipio, for the Tunic Randomizer which was used extensively as a basis for this mod.
- Silvris, for Pixel Remasters modding support
- Alchav, for his contribution
- Playtesters: Prime, Abyss, Bartz24, Wyatt, Kuchiri