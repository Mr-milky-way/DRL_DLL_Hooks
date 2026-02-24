using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using drl.backend;

namespace DRL_DLL_Hooks
{
    [BepInPlugin("drl.hooks", "DRL Hooks", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        void Awake()
        {
            Logger.LogInfo("DRL hook loaded");

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
}
