# WaitAndChill
A Remake of a ServerMod plugin where before the game starts, users can be tutorials and talk and do whatever they wish

### Features
- Two message lines which are customizable (You can make it a Hint displayed on each Player or a Broadcast, it also works with Unity's Rich Text tags, you can also disable the message and just let users do what they want)
- You can adjust the vertical position of the message lines when they are displayed! (Hints only)
- Choice of randomly setting a role for users to be when they spawn

### Note
- %player will return one of two options for messages ((0 or x players have connected) or (1 player has connected))
- %seconds will return one of four options for messages (The server is paused, The round has started, 1 second remains, x seconds remain)

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
  use_broadcast_message: false
  # Determines the position of the Hint on the users screen (0 = Top, 32 = Close to Middle, Default 2)
  hint_vert_pos: 2
  # The top message that is displayed to users (Works with Unity Rich Text tags)
  top_message: <size=50><color=yellow><b>The game will be starting soon, %seconds</b></color></size>
  # The bottom message that is displayed to users (Works with Unity Rich Text tags)
  bottom_message: <size=40><i>%players</i></size>
  # The list of roles that will be chosen to spawn as by random chance (Use RoleType names)
  roles_to_choose:
  - Tutorial
  # Customization for the player and timer text, works with Unity Rich Text tags
  custom_text_values:
    Timer:
      XSecondsRemain: seconds remain
      1SecondRemains: second remains
      ServerIsPaused: The server is paused
      RoundStarting: The round has started
    Player:
      XPlayersConnected: players have connected
      1PlayerConnected: player has connected
```
