#region

using HarmonyLib;
using NebulaWorld;

#endregion

namespace NebulaPatcher.Patches.Dynamic;

[HarmonyPatch(typeof(KillStatistics))]
internal class KillStatistics_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(KillStatistics.PrepareTick))]
    public static bool PrepareTick_Prefix(KillStatistics __instance)
    {
        if (!Multiplayer.IsActive || Multiplayer.Session.LocalPlayer.IsHost)
        {
            return true;
        }
        for (var i = 0; i < __instance.gameData.factoryCount; i++)
        {
            __instance.factoryKillStatPool[i]?.PrepareTick();
        }
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(KillStatistics.AfterTick))]
    public static bool AfterTick_Prefix(KillStatistics __instance)
    {
        if (!Multiplayer.IsActive || Multiplayer.Session.LocalPlayer.IsHost)
        {
            return true;
        }
        for (var i = 0; i < __instance.gameData.factoryCount; i++)
        {
            __instance.factoryKillStatPool[i]?.AfterTick();
        }
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(KillStatistics.GameTick))]
    public static bool GameTick_Prefix(KillStatistics __instance)
    {
        if (!Multiplayer.IsActive || Multiplayer.Session.LocalPlayer.IsHost)
        {
            return true;
        }
        //Do not run on client if you do not have all data
        for (var i = 0; i < __instance.gameData.factoryCount; i++)
        {
            if (__instance.factoryKillStatPool[i] == null)
            {
                return false;
            }
        }
        return true;
    }
}
