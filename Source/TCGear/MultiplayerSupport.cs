using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace TCGear;

[StaticConstructorOnStartup]
internal static class MultiplayerSupport
{
    private static readonly Harmony harmony = new Harmony("rimworld.pelador.tcgear.multiplayersupport");

    static MultiplayerSupport()
    {
        if (!MP.enabled)
        {
            return;
        }

        var array = new[]
        {
            AccessTools.Method(typeof(DamageWorker_TCOrb), nameof(DamageWorker_TCOrb.Apply)),
            AccessTools.Method(typeof(TCPowerBeam), nameof(TCPowerBeam.StartRandomFireAndDoFlameDamage)),
            AccessTools.Method(typeof(TCBombardment), nameof(TCBombardment.CreateRandomExplosion)),
            AccessTools.Method(typeof(TCBombardment), nameof(TCBombardment.StartRandomFire))
        };
        foreach (var methodInfo in array)
        {
            FixRNG(methodInfo);
        }
    }

    private static void FixRNG(MethodInfo method)
    {
        harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre"),
            new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos"));
    }

    private static void FixRNGPre()
    {
        Rand.PushState(Find.TickManager.TicksAbs);
    }

    private static void FixRNGPos()
    {
        Rand.PopState();
    }
}