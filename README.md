# WaitAndChill
A Remake of a ServerMod plugin where before the game starts, users can be tutorials and talk and do whatever they wish

### Features
- Two message lines which are customizable (You can make it a Hint displayed on each Player or a Broadcast, it also works with Unity's Rich Text tags, you can also disable the message and just let users do what they want)
- You can adjust the vertical position of the message lines when they are displayed! (Hints only)
- Choice of spawning items and/or ammo for users when they spawn in (Items can be entered in case insensitive!)

### Note
%player will return one of two options for messages ((0 or x players have connected) or (1 player has connected))

### Credits
- [F4Fridey](https://github.com/F4Fridey) (Original plugin idea from ServerMod)
- [SirMeepington](https://github.com/sirmeepington) (Code for positioning TextHints vertically)
- [RogerFK](https://github.com/RogerFK) (Code for replacing tokens (%player) with other values)

### Config
```yaml
wait_and_chill:
  # Determines whether this plugin will be enabled or not
  is_enabled: true
  # Determines if any kind of message at all will be displayed
  display_wait_message: true
  # Determines if Broadcasts will be used for the message instead of Hints (WARNING: It can mess with any other broadcasts that are being done by other plugins)
  use_broadcast_message: true
  # Determines if items will be given to users when they spawn in
  give_items: true
  # Determines if ammo will be given to users when they spawn in
  give_ammo: true
  # Determines the position of the Hint on the users screen (0 = Top, 32 = Close to Middle, Default 2)
  hint_vert_pos: 2
  # The top message that is displayed to users (Works with Unity Rich Text tags)
  top_message: <color=yellow><b>The game will be starting soon</b></color>
  # The bottom message that is displayed to users (Works with Unity Rich Text tags)
  bottom_message: <i>%players</i>
  # The list of items that will be given to users when they spawn (Case insensitive, use RoleType names)
  items_to_give:
  - GunUSP
  - GunE11SR
  - GunLogicer
  # The amount of ammo for each AmmoType that will be given to users when they spawn (Default 100)
  ammo_to_give:
    Nato556Ammo: 100
    Nato762Ammo: 100
    Nato9Ammo: 100
```
