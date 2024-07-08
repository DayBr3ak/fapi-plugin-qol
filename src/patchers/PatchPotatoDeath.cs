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
using System.Reflection.Emit;
using System.Collections;

[HarmonyPatch(typeof(EnemyStats))]
class PatchPotatoDeath
{

    [HarmonyTranspiler]
    [HarmonyPatch(nameof(EnemyStats.TakeDamage))]
    static IEnumerable<CodeInstruction> TranspileTakeDamage(IEnumerable<CodeInstruction> instructions) {

        int endIndex = -1;
        var codes = new List<CodeInstruction>(instructions);
        // for (var i = 1; i < codes.Count; i++)
        // {
        //     var code = codes[i];
        //     Plugin.StaticLogger.LogInfo("il: " + code);
        // }

        // Remove the Play dead anim and call our own method
        for (var i = 1; i < codes.Count; i++)
        {
            var code = codes[i];
            // Plugin.StaticLogger.LogInfo("il: " + code);
            if (code.opcode == OpCodes.Callvirt && codes[i - 1].opcode == OpCodes.Ldstr && (codes[i - 1].operand as string) == "EnemyPotatoDeath1") {
                endIndex = i;
                break;
            }
        }

        var startIndex = -1;
        if (endIndex >= 0) {
            startIndex = endIndex - 3;
            codes.RemoveRange(startIndex, 4);
            var method = SymbolExtensions.GetMethodInfo(() => PatchPotatoDeath.DoSomething(new EnemyStats()));
            codes.Insert(startIndex, new CodeInstruction(OpCodes.Ldarg_0));
            codes.Insert(startIndex + 1, new CodeInstruction(OpCodes.Callvirt, method));
        }

        if (startIndex < 0) {
            return codes.AsEnumerable();
        }


        // Remove the Destroy after the play dead anim
        for (var i = startIndex; i < codes.Count; i++)
        {
            var code = codes[i];
            // Plugin.StaticLogger.LogInfo("il: " + code);
            if (code.opcode == OpCodes.Call && codes[i - 1].opcode == OpCodes.Ldc_R4) {
                var op = codes[i -1].operand;
                if (op != null && (float)op == 1.5f) {
                    codes.RemoveRange(i - 3, 4);
                    break;
                }
            }
        }

        // Remove the Destroy at the end of the method
        // it needs to be Nop, since a label is referencing the instruction
        codes[codes.Count - 3].opcode = OpCodes.Nop;
        codes[codes.Count - 2].opcode = OpCodes.Nop;

        return codes.AsEnumerable();
    }

    public static void DoSomething(EnemyStats self) {
        // Plugin.StaticLogger.LogInfo("OnDeath, dosomething " + self.gameObject.name);
        self.StartCoroutine(FlyOffAnim(self));
    }

    static IEnumerator FlyOffAnim(EnemyStats x) {

        if (GameManager.i.PD.AreaSelected < 73 || GameManager.i.PD.AreaSelected > 79) {
            x.enemy.Play("EnemyPotatoDeath1");
            yield return null;
            GameObject.Destroy(x.gameObject, 1.5f);
            GameObject.Destroy(x);
            yield break;
        }

        x.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45f));
        float travelTime = 10f;
        float currentTime = 0f;

        Vector3 moveVelocity = new Vector3(50.0f, 50.0f, 0);
        moveVelocity = Quaternion.Euler(0f,0f, Random.Range(-40f, 10f)) * moveVelocity;

        // Rigidbody2D rb = (Rigidbody2D)typeof(EnemyStats).GetField("rb",BindingFlags.NonPublic | BindingFlags.Instance).GetValue(x);
        // GameObject.Destroy(rb);
        GameObject.Destroy(x.enemy); // stop the animation all together

        while (currentTime <= travelTime) {
            currentTime += Time.deltaTime;
            x.transform.localPosition = new Vector3(
                x.transform.localPosition.x + moveVelocity.x,
                x.transform.localPosition.y + moveVelocity.y,
                0
            );
            yield return null;
        }

        GameObject.Destroy(x.gameObject);
        GameObject.Destroy(x);

        // Plugin.StaticLogger.LogInfo("exit");
        yield return null;
    }

}