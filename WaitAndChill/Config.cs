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

		[Description("Determines the position of the Hint on the users screen (0 = Top, 32 = Close to Middle, Default 2)")]
		public uint HintVertPos { get; set; } = 2;

		[Description("The top message that is displayed to users (Works with Unity Rich Text tags)")]
		public string TopMessage { get; set; } = "<size=50><color=yellow><b>The game will be starting soon, %seconds</b></color></size>";

		[Description("The bottom message that is displayed to users (Works with Unity Rich Text tags)")]
		public string BottomMessage { get; set; } = "<size=40><i>%players</i></size>";

		[Description("The list of roles that will be chosen to spawn as by random chance (Use RoleType names)")]
		public List<RoleType> RolesToChoose { get; private set; } = new List<RoleType>() { RoleType.Tutorial };

		[Description("Customization for the player and timer text, works with Unity Rich Text tags")]
		public Dictionary<string, Dictionary<string, string>> CustomTextValues { get; private set; } = new Dictionary<string, Dictionary<string, string>>()
		{
			{
				"Timer", new Dictionary<string, string>()
				{
					{ "XSecondsRemain", "seconds remain" },
					{ "1SecondRemains", "second remains" },
					{ "ServerIsPaused", "The server is paused" },
					{ "RoundStarting", "The round has started" }
				}
			},
			{
				"Player", new Dictionary<string, string>()
				{
					{ "XPlayersConnected", "players have connected" },
					{ "1PlayerConnected", "player has connected" }
				}
			},
		};
	}
}
