using BepInEx;
using drl.backend;
using HarmonyLib;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using drl;
using drl.game;



namespace DRL_DLL_Hooks
{
    [BepInPlugin("drl.hooks", "DRL Hooks", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        void Awake()
        {
            Logger.LogInfo("DRL hook loaded");
            Logger.LogInfo("API SET AS:" + File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apiurl.txt")));
            var harmony = new Harmony("drl.hooks");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(DRLService), "get_baseUri")]
    class Patch_BaseUri
    {
        static bool Prefix(ref string __result)
        {
            __result = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apiurl.txt"));
            return false;
        }
    }

    [HarmonyPatch(typeof(DRLBootController), "Awake")]
    class Patch_Replays
    {
        static void Postfix(DRLBootController __instance)
        {
            string baseUrl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apiurl.txt")).Trim();
            Debug.Log(baseUrl);
            __instance.beginnerReplayId = $"{baseUrl}/onboarding-bots-replays-v2/onboarding-bot-beginner.race";
            __instance.intermediateReplayId = $"{baseUrl}/onboarding-bots-replays-v2/onboarding-bot-intermediate.race";
            __instance.proReplayId0 = $"{baseUrl}/onboarding-bots-replays-v2/onboarding-bot-pro-1.race";
            __instance.proReplayId1 = $"{baseUrl}/onboarding-bots-replays-v2/onboarding-bot-pro-2.race";
            __instance.proReplayId2 = $"{baseUrl}/onboarding-bots-replays-v2/onboarding-bot-pro-3.race";
        }
    }
}
