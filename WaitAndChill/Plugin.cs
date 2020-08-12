using Exiled.API.Features;
using PlayerEvents = Exiled.Events.Handlers.Player;
using RoundEvents = Exiled.Events.Handlers.Server;


namespace WaitAndChill
{
    public class Plugin : Plugin<Config>
    {
        public EventHandler Handler;
        public override string Author => "KoukoCocoa";
        public override string Name => "WaitAndChill";

        public override void OnEnabled()
        {
            Handler = new EventHandler(this);
            RoundEvents.RestartingRound += Handler.RunWhenRoundRestarts;
            RoundEvents.RoundStarted += Handler.RunWhenRoundStarts;
            RoundEvents.WaitingForPlayers += Handler.RunWhenPlayersWait;
            PlayerEvents.Joined += Handler.RunWhenPlayerJoins;
            PlayerEvents.Left += Handler.RunWhenPlayerLeaves;
            PlayerEvents.Shooting += Handler.RunWhenPlayerShoots;
        }

        public override void OnDisabled()
        {
            PlayerEvents.Shooting -= Handler.RunWhenPlayerShoots;
            PlayerEvents.Left -= Handler.RunWhenPlayerLeaves;
            PlayerEvents.Joined -= Handler.RunWhenPlayerJoins;
            RoundEvents.WaitingForPlayers -= Handler.RunWhenPlayersWait;
            RoundEvents.RoundStarted -= Handler.RunWhenRoundStarts;
            RoundEvents.RestartingRound -= Handler.RunWhenRoundRestarts;
            Handler = null;
        }

        public override void OnReloaded() { }
    }
}
