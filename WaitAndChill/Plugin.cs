using Exiled.API.Features;
using HarmonyLib;
using PlayerEvents = Exiled.Events.Handlers.Player;
using RoundEvents = Exiled.Events.Handlers.Server;
using Scp914Events = Exiled.Events.Handlers.Scp914;


namespace WaitAndChill
{
	public class Plugin : Plugin<Config>
	{
		public EventHandler Handler;
		public override string Author => "KoukoCocoa";
		public override string Name => "WaitAndChill";

		public override void OnEnabled()
		{
			base.OnEnabled();

			if (!Config.IsEnabled) return;

			Handler = new EventHandler(this);
			Handler.Init();
			RoundEvents.RestartingRound += Handler.RunWhenRoundRestarts;
			RoundEvents.RoundStarted += Handler.RunWhenRoundStarts;
			RoundEvents.WaitingForPlayers += Handler.RunWhenPlayersWait;
			PlayerEvents.Joined += Handler.RunWhenPlayerJoins;
			PlayerEvents.Left += Handler.RunWhenPlayerLeaves;
			PlayerEvents.Spawning += Handler.RunWhenPlayerSpawns;
			PlayerEvents.InteractingLocker += Handler.RunWhenLockerOpens;
			PlayerEvents.PickingUpItem += Handler.RunWhenPickingUpItem;
			Scp914Events.Activating += Handler.RunWhenActivating914;
			Scp914Events.ChangingKnobSetting += Handler.RunWhenChanging914KnobState;
		}

		public override void OnDisabled()
		{
			base.OnEnabled();

			PlayerEvents.Left -= Handler.RunWhenPlayerLeaves;
			PlayerEvents.Joined -= Handler.RunWhenPlayerJoins;
			RoundEvents.WaitingForPlayers -= Handler.RunWhenPlayersWait;
			RoundEvents.RoundStarted -= Handler.RunWhenRoundStarts;
			RoundEvents.RestartingRound -= Handler.RunWhenRoundRestarts;
			PlayerEvents.Spawning -= Handler.RunWhenPlayerSpawns;
			PlayerEvents.InteractingLocker -= Handler.RunWhenLockerOpens;
			PlayerEvents.PickingUpItem -= Handler.RunWhenPickingUpItem;
			Scp914Events.Activating -= Handler.RunWhenActivating914;
			Scp914Events.ChangingKnobSetting -= Handler.RunWhenChanging914KnobState;
			Handler = null;
		}

		public override void OnReloaded() { }
	}
}
