using Exiled.API.Features;
using HarmonyLib;
using PlayerEvents = Exiled.Events.Handlers.Player;
using RoundEvents = Exiled.Events.Handlers.Server;


namespace WaitAndChill
{
    public class Plugin : Plugin<Config>
    {
        public EventHandler Handler;
        public override string Author => "KoukoCocoa";
        public override string Name => "WaitAndChill";

        private Harmony hInstance;

        public override void OnEnabled()
        {
            base.OnEnabled();

            if (!Config.IsEnabled) return;

            hInstance = new Harmony("koukococoa.waitandchill");
            hInstance.PatchAll();

            Handler = new EventHandler(this);
            RoundEvents.RestartingRound += Handler.RunWhenRoundRestarts;
            RoundEvents.RoundStarted += Handler.RunWhenRoundStarts;
            RoundEvents.WaitingForPlayers += Handler.RunWhenPlayersWait;
            PlayerEvents.Joined += Handler.RunWhenPlayerJoins;
            PlayerEvents.Left += Handler.RunWhenPlayerLeaves;
            PlayerEvents.Spawning += Handler.RunWhenPlayerSpawns;
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
            Handler = null;

            hInstance.UnpatchAll();
            hInstance = null;
        }

        public override void OnReloaded() { }
    }
}
