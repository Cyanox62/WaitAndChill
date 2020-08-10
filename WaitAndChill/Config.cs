using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace WaitAndChill
{
    public class Config : IConfig
    {
        [Description("Determines whether this plugin will be enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("Determines if any kind of message at all will be displayed")]
        public bool DisplayWaitMessage { get; set; } = true;

        [Description("Determines if Broadcasts will be used for the message instead of Hints (WARNING: It can mess with any other broadcasts that are being done by other plugins)")]
        public bool UseBroadcastMessage { get; set; } = false;

        [Description("Determines if items will be given to users when they spawn in")]
        public bool GiveItems { get; set; } = true;

        [Description("Determines if ammo will be given to users when they spawn in")]
        public bool GiveAmmo { get; set; } = true;

        [Description("Determines the position of the Hint on the users screen (0 = Top, 32 = Close to Middle, Default 2)")]
        public uint HintVertPos { get; set; } = 2;

        [Description("The top message that is displayed to users (Works with Unity Rich Text tags)")]
        public string TopMessage { get; set; } = "<size=60><color=yellow><b>The game will be starting soon</b></color></size>";

        [Description("The bottom message that is displayed to users (Works with Unity Rich Text tags)")]
        public string BottomMessage { get; set; } = "<size=40><i>%players</i></size>";

        [Description("The list of items that will be given to users when they spawn (Case insensitive, use RoleType names)")]
        public List<string> ItemsToGive { get; private set; } = new List<string>() { "GunUSP", "GunE11SR", "GunLogicer" };

        [Description("The amount of ammo for each AmmoType that will be given to users when they spawn (Default 100)")]
        public Dictionary<string, uint> AmmoToGive { get; private set; } = new Dictionary<string, uint>()
        {
            { "Nato556Ammo", 100 },
            { "Nato762Ammo", 100 },
            { "Nato9Ammo", 100 },
        };
    }
}
