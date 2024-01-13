#region

using HarmonyLib;
using NebulaWorld;

#endregion

namespace NebulaPatcher.Patches.Dynamic;

[HarmonyPatch(typeof(AstroKillStat))]
internal class AstroKillStat_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(AstroKillStat.GameTick))]
    public static bool GameTick_Prefix(AstroKillStat __instance)
    {
        //Do not run in single player for host
        if (!Multiplayer.IsActive || Multiplayer.Session.LocalPlayer.IsHost)
        {
            return true;
        }

        //Multiplayer clients should not include their own calculated statistics
        if (Multiplayer.Session.Statistics.IsIncomingRequest)
        {
            return true;
        }
        __instance.ClearRegister();
        return false;

    }
}
