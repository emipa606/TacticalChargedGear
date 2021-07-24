using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace TCGear
{
    // Token: 0x02000006 RID: 6
    [StaticConstructorOnStartup]
    internal static class MultiplayerSupport
    {
        // Token: 0x04000003 RID: 3
        private static readonly Harmony harmony = new Harmony("rimworld.pelador.tcgear.multiplayersupport");

        // Token: 0x06000010 RID: 16 RVA: 0x000023D4 File Offset: 0x000005D4
        static MultiplayerSupport()
        {
            if (!MP.enabled)
            {
                return;
            }

            var array = new[]
            {
                AccessTools.Method(typeof(DamageWorker_TCOrb), "Apply"),
                AccessTools.Method(typeof(TCPowerBeam), "StartRandomFireAndDoFlameDamage"),
                AccessTools.Method(typeof(TCBombardment), "CreateRandomExplosion"),
                AccessTools.Method(typeof(TCBombardment), "StartRandomFire")
            };
            foreach (var methodInfo in array)
            {
                FixRNG(methodInfo);
            }
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002479 File Offset: 0x00000679
        private static void FixRNG(MethodInfo method)
        {
            harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre"),
                new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos"));
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000024B3 File Offset: 0x000006B3
        private static void FixRNGPre()
        {
            Rand.PushState(Find.TickManager.TicksAbs);
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000024C4 File Offset: 0x000006C4
        private static void FixRNGPos()
        {
            Rand.PopState();
        }
    }
}