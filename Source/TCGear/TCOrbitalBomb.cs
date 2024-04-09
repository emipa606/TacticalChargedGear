using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear;

public class TCOrbitalBomb : Gas
{
    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, true);
        destroyTick = Find.TickManager.TicksGame + def.gas.expireSeconds.RandomInRange.SecondsToTicks();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref destroyTick, "destroyTick");
    }

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

        var tcbombardment = (TCBombardment)GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed("TCBombardment"),
            TargetCell, TargetMap);
        tcbombardment.duration = 120;
        tcbombardment.instigator = this;
        tcbombardment.weaponDef = ThingDef.Named("OrbitalTargeterBombardment");
        tcbombardment.StartStrike();
        if (!this.DestroyedOrNull())
        {
            Destroy();
        }
    }
}