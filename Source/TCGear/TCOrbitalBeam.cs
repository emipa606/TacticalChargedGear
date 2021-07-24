using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear
{
    // Token: 0x0200000A RID: 10
    public class TCOrbitalBeam : Gas
    {
        // Token: 0x06000024 RID: 36 RVA: 0x00002B21 File Offset: 0x00000D21
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, true);
            destroyTick = Find.TickManager.TicksGame + def.gas.expireSeconds.RandomInRange.SecondsToTicks();
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002B56 File Offset: 0x00000D56
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref destroyTick, "destroyTick");
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00002B70 File Offset: 0x00000D70
        public override void Tick()
        {
            if (destroyTick <= Find.TickManager.TicksGame)
            {
                Destroy();
            }

            graphicRotation += graphicRotationSpeed;
            if (this.DestroyedOrNull())
            {
                return;
            }

            var TargetMap = Map;
            var TargetCell = Position;
            if (Find.TickManager.TicksGame % 10 == 0)
            {
                FleckMaker.ThrowSmoke(this.TrueCenter() + new Vector3(0f, 0f, 0.1f), TargetMap, 1f);
            }

            if (Find.TickManager.TicksGame % 300 != 0)
            {
                return;
            }

            var tcpowerBeam = (TCPowerBeam) GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed("TCPowerBeam"),
                TargetCell, TargetMap);
            tcpowerBeam.duration = 120;
            tcpowerBeam.instigator = this;
            tcpowerBeam.weaponDef = ThingDefOf.OrbitalTargeterPowerBeam;
            tcpowerBeam.StartStrike();
            if (!this.DestroyedOrNull())
            {
                Destroy();
            }
        }
    }
}