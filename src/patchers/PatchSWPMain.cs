using BepInEx.Logging;
using FapiQolPlugin;
using HarmonyLib;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;

using static Utils;
using System.Collections;

[HarmonyPatch(typeof(EnemyStats))]
class PatchSWPMainx
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(EnemyStats.TakeDamage))]
    static void Postfix()
    {
        Plugin.StaticLogger.LogInfo("Postfix PatchSWPMainx");
        // var LoaderObject = new GameObject("PluginLoaderSWP");
        // LoaderObject.AddComponent<SweetPotatoesStatTracker>();

        // Unpatch this method, on this specific harmony instance
        var harmony = new Harmony("com.example.fapiqolplugin");
        var original = typeof(EnemyStats).GetMethod(nameof(EnemyStats.TakeDamage));
        harmony.Unpatch(original, HarmonyPatchType.Postfix);
    }
}