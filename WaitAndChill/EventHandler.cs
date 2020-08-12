using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using MEC;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WaitAndChill
{
    public class EventHandler
    {
        int PlayerCount;
        RoleType RoleToSet;
        Plugin Plugin;
        CoroutineHandle Handle;
        System.Random RandNumGen = new System.Random();

        public EventHandler(Plugin Plugin) => this.Plugin = Plugin;

        public void RunWhenPlayersWait()
        {
            int RoleToChoose = RandNumGen.Next(0, Plugin.Config.RolesToChoose.Count);
            PlayerCount = 0;
            Handle = Timing.RunCoroutine(BroadcastMessage());
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;
            RoleToSet = Plugin.Config.RolesToChoose[RoleToChoose];
        }

        public void RunWhenRoundStarts()
        {
            Timing.KillCoroutines(Handle);
        }

        public void RunWhenRoundRestarts()
        {
            PlayerCount = 0;
            RoleToSet = RoleType.Tutorial;
        }

        public void RunWhenPlayerJoins(JoinedEventArgs JoinEv)
        {
            if (!Round.IsStarted)
            {
                Timing.CallDelayed(0.55f, () => SetPlayer(JoinEv.Player));
                PlayerCount++;
            }
        }

        public void RunWhenPlayerLeaves(LeftEventArgs LeftEv)
        {
            if (!Round.IsStarted && PlayerCount > 0)
                PlayerCount--;
        }

        public void SetPlayer(Player Ply)
        {
            Ply.Role = RoleToSet;

            if (Plugin.Config.GiveItems)
                foreach (ItemType Item in Plugin.Config.ItemsToGive)
                    Ply.AddItem(Item);

            if (Plugin.Config.GiveAmmo)
                SetPlayerAmmo(Ply);
        }

        public void SetPlayerAmmo(Player Ply)
        {
            Ply.SetAmmo(AmmoType.Nato556, Ply.GetAmmo(AmmoType.Nato556) + Plugin.Config.AmmoToGive["Nato556Ammo"]);
            Ply.SetAmmo(AmmoType.Nato762, Ply.GetAmmo(AmmoType.Nato762) + Plugin.Config.AmmoToGive["Nato762Ammo"]);
            Ply.SetAmmo(AmmoType.Nato9, Ply.GetAmmo(AmmoType.Nato9) + Plugin.Config.AmmoToGive["Nato9Ammo"]);
        }

        public IEnumerator<float> BroadcastMessage()
        {
            if (Plugin.Config.DisplayWaitMessage)
            {
                StringBuilder MessageBuilder = NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
                while (!Round.IsStarted)
                {
                    MessageBuilder.Append(PlayerCount);
                    MessageBuilder.Append(" ");
                    if (PlayerCount != 1)
                        MessageBuilder.Append("players have connected");
                    else
                        MessageBuilder.Append("player has connected");
                    string Result = MessageBuilder.ToString();
                    NorthwoodLib.Pools.StringBuilderPool.Shared.Return(MessageBuilder);

                    MessageBuilder = NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
                    switch (GameCore.RoundStart.singleton.NetworkTimer)
                    {
                        case -2:
                            MessageBuilder.Append("The server is paused");
                            break;
                        case -1:
                            MessageBuilder.Append("The round has started");
                            break;
                        case 1:
                            MessageBuilder.Append(GameCore.RoundStart.singleton.NetworkTimer);
                            MessageBuilder.Append(" second remains");
                            break;
                        default:
                            MessageBuilder.Append(GameCore.RoundStart.singleton.NetworkTimer);
                            MessageBuilder.Append(" seconds remain");
                            break;
                    }
                    string Time = MessageBuilder.ToString();
                    NorthwoodLib.Pools.StringBuilderPool.Shared.Return(MessageBuilder);

                    MessageBuilder = NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
                    string TopMessage = TokenReplacer.ReplaceAfterToken(Plugin.Config.TopMessage, '%', new Tuple<string, object>[] { new Tuple<string, object>("players", Result), new Tuple<string, object>("seconds", Time) });
                    string BottomMessage = TokenReplacer.ReplaceAfterToken(Plugin.Config.BottomMessage, '%', new Tuple<string, object>[] { new Tuple<string, object>("players", Result), new Tuple<string, object>("seconds", Time) });

                    MessageBuilder.AppendLine(TopMessage);
                    MessageBuilder.Append(BottomMessage);
                    if (!Plugin.Config.UseBroadcastMessage)
                    {
                        MessageBuilder.AppendLine(NewLineFormatter(Plugin.Config.HintVertPos <= 32 ? Plugin.Config.HintVertPos : 32));
                        foreach (Player Ply in Player.List)
                            Ply.ReferenceHub.hints.Show(new TextHint(MessageBuilder.ToString(), new HintParameter[]
                            { new StringHintParameter("") }, HintEffectPresets.FadeInAndOut(15f, 1f, 15f)));
                    }
                    else
                        Map.Broadcast(1, MessageBuilder.ToString());
                    NorthwoodLib.Pools.StringBuilderPool.Shared.Return(MessageBuilder);
                    yield return Timing.WaitForSeconds(1f);
                }
                Map.ClearBroadcasts();
            }
        }

        // Thanks to SirMeepington for this
        public string NewLineFormatter(uint LineNumber)
        {
            StringBuilder LineBuilder = NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
            for (int i = 32; i > LineNumber; i--)
            {
                LineBuilder.Append("\n");
            }
            string Result = LineBuilder.ToString();
            NorthwoodLib.Pools.StringBuilderPool.Shared.Return(LineBuilder);
            return Result;
        }
    }
}