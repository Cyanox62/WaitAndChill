using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using Hints;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Vector3 RoomPosition;
        Room Room;
        Lift GateALift;
        Lift GateBLift;

        RoomType[] PossibleRooms = { RoomType.EzGateA, RoomType.EzGateB, RoomType.Hcz939, RoomType.Surface, RoomType.Hcz106, RoomType.Lcz173, RoomType.LczGlassBox, RoomType.Lcz012 };

        public EventHandler(Plugin Plugin) => this.Plugin = Plugin;

        public void RunWhenPlayersWait()
        {
            int RoleToChoose = RandNumGen.Next(0, Plugin.Config.RolesToChoose.Count);
            PlayerCount = 0;
            Handle = Timing.RunCoroutine(BroadcastMessage());
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;
            RoleToSet = Plugin.Config.RolesToChoose[RoleToChoose];

            RoomType type = PossibleRooms[RandNumGen.Next(PossibleRooms.Length)];

            switch (type)
			{
                case RoomType.EzGateA:
                case RoomType.EzGateB:
                case RoomType.Hcz939:
                case RoomType.LczGlassBox:
                case RoomType.Lcz012:
                    Room = Map.Rooms.First(r => r.Type == type);
                    RoomPosition = Room.Position;
                    if (type == RoomType.Lcz012)
					{
                        RoomPosition.y -= 6;
					}
                    break;
                case RoomType.Surface:
                    if (RandNumGen.Next(2) == 0)
                    {
                        RoomPosition = new Vector3(53, 1019, -44);
                    }
                    else
                    {
                        RoomPosition = new Vector3(-22, 1019, -44);
                    }
                    break;
                case RoomType.Hcz106:
                    Room = Map.Rooms.First(r => r.Type == type);
                    RoomPosition = RoleType.Scp106.GetRandomSpawnPoint();
                    break;
                case RoomType.Lcz173:
                    Room = Map.Rooms.First(r => r.Type == type);
                    RoomPosition = RoleType.Scp173.GetRandomSpawnPoint();
                    break;
                default:
                    Room = Map.Rooms.First(r => r.Type == RoomType.EzGateA);
                    RoomPosition = Room.Position;
                    break;
            }

            if (Room != null && type != RoomType.LczGlassBox)
            {
                foreach (Door door in Room.Doors) door.Networklocked = true;
            }
            else if(type == RoomType.LczGlassBox)
			{
                Room.Doors.First(d => d.doorType == Door.DoorTypes.HeavyGate).Networklocked = true;
			}

            RoomPosition.y += 2;

            foreach (Lift lift in Map.Lifts)
            {
                if (lift.elevatorName == "GateA")
                {
                    GateALift = lift;
                }
                else if (lift.elevatorName == "GateB")
				{
                    GateBLift = lift;
				}
            }

            GateALift.Network_locked = true;
            GateALift.NetworkstatusID = (byte)Lift.Status.Down;
            GateBLift.Network_locked = true;
            GateBLift.NetworkstatusID = (byte)Lift.Status.Down;
        }

        public void RunWhenRoundStarts()
        {
            Timing.KillCoroutines(new CoroutineHandle[] { Handle });

            GateALift.Network_locked = false;
            GateALift.NetworkstatusID = (byte)Lift.Status.Up;
            GateBLift.Network_locked = false;
            GateBLift.NetworkstatusID = (byte)Lift.Status.Up;

            if (Room != null)
            {
                foreach (Door door in Room.Doors) door.Networklocked = false;
            }
        }

        public void RunWhenRoundRestarts()
        {
            PlayerCount = 0;
            RoleToSet = RoleType.Tutorial;
        }

        public void RunWhenPlayerJoins(JoinedEventArgs JoinEv)
        {
            if (!Round.IsStarted && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2))
            {
                Timing.CallDelayed(1f, () => JoinEv.Player.Role = RoleToSet);
                PlayerCount++;
            }
        }

        public void RunWhenPlayerLeaves(LeftEventArgs LeftEv)
        {
            if (!Round.IsStarted && PlayerCount > 0)
                PlayerCount--;
        }

        public void RunWhenPlayerSpawns(SpawningEventArgs SpawnEv)
        {
            if (!Round.IsStarted && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2) && SpawnEv.RoleType == RoleType.Tutorial)
            {
                SpawnEv.Position = RoomPosition;
            }
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
                        MessageBuilder.Append(Plugin.Config.CustomTextValues.TryGetValue("Player", out Dictionary<string, string> Diction1) ? (Diction1.TryGetValue("XPlayersConnected", out string value) ? value : "") : "");
                    else
                        MessageBuilder.Append(Plugin.Config.CustomTextValues.TryGetValue("Player", out Dictionary<string, string> Diction1) ? (Diction1.TryGetValue("1PlayerConnected", out string value) ? value : "") : "");
                    string Result = MessageBuilder.ToString();
                    NorthwoodLib.Pools.StringBuilderPool.Shared.Return(MessageBuilder);

                    MessageBuilder = NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
                    string msg = "<size=50><color=yellow><b>";
                    switch (GameCore.RoundStart.singleton.NetworkTimer)
                    {
                        case -2:
                            //MessageBuilder.Append(Plugin.Config.CustomTextValues.TryGetValue("Timer", out Dictionary<string, string> Diction1) ? (Diction1.TryGetValue("ServerIsPaused", out string value1) ? value1 : "") : "");
                            if (GameCore.RoundStart.LobbyLock)
                            {
                                msg += "the round is paused";
                            }
                            else
                            {
                                msg += "waiting for players";
                            }
                            break;
                        case -1:
                        case 0:
                            //MessageBuilder.Append(Plugin.Config.CustomTextValues.TryGetValue("Timer", out Dictionary<string, string> Diction2) ? (Diction2.TryGetValue("RoundStarting", out string value2) ? value2 : "") : "");
                            msg += "the round is starting";
                            break;
                        case 1:
                            //MessageBuilder.Append(GameCore.RoundStart.singleton.NetworkTimer);
                            //MessageBuilder.Append(" ");
                            // MessageBuilder.Append(Plugin.Config.CustomTextValues.TryGetValue("Timer", out Dictionary<string, string> Diction3) ? (Diction3.TryGetValue("1SecondRemains", out string value3) ? value3 : "") : "");
                            msg += "the round will start in 1 second";
                            break;
                        default:
                            //MessageBuilder.Append(GameCore.RoundStart.singleton.NetworkTimer);
                            //MessageBuilder.Append(" ");
                            //MessageBuilder.Append(Plugin.Config.CustomTextValues.TryGetValue("Timer", out Dictionary<string, string> Diction4) ? (Diction4.TryGetValue("XSecondsRemain", out string value4) ? value4 : "") : "");
                            msg += $"the round will start in {GameCore.RoundStart.singleton.NetworkTimer} seconds";
                            if (GameCore.RoundStart.singleton.NetworkTimer == 0)
                                CharacterClassManager.ForceRoundStart();
                            break;
                    }
                    msg += "</b></color></size>";
                    string Time = MessageBuilder.ToString();
                    NorthwoodLib.Pools.StringBuilderPool.Shared.Return(MessageBuilder);

                    MessageBuilder = NorthwoodLib.Pools.StringBuilderPool.Shared.Rent();
                    string TopMessage = msg; //TokenReplacer.ReplaceAfterToken(Plugin.Config.TopMessage, '%', new Tuple<string, object>[] { new Tuple<string, object>("players", Result), new Tuple<string, object>("seconds", Time) });
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