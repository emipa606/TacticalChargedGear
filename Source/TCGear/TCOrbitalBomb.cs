using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear
{
    // Token: 0x0200000B RID: 11
    public class TCOrbitalBomb : Gas
    {
        // Token: 0x06000028 RID: 40 RVA: 0x00002C5B File Offset: 0x00000E5B
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, true);
            destroyTick = Find.TickManager.TicksGame + def.gas.expireSeconds.RandomInRange.SecondsToTicks();
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00002C90 File Offset: 0x00000E90
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref destroyTick, "destroyTick");
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002CAC File Offset: 0x00000EAC
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

            var tcbombardment = (TCBombardment) GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed("TCBombardment"),
                TargetCell, TargetMap);
            tcbombardment.duration = 120;
            tcbombardment.instigator = this;
            tcbombardment.weaponDef = ThingDefOf.OrbitalTargeterBombardment;
            tcbombardment.StartStrike();
            if (!this.DestroyedOrNull())
            {
                Destroy();
            }
        }
    }
}