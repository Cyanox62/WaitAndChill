using Exiled.API.Features;
using HarmonyLib;

namespace WaitAndChill.Patches
{
	[HarmonyPatch(typeof(WorkStation), nameof(WorkStation.CanTake))]
	class CanTake
	{
		public static void Postfix(ref bool __result) => __result = !(!Round.IsStarted && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2));
	}
}
