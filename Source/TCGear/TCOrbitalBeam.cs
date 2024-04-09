using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear;

public class TCOrbitalBeam : Gas
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

        var tcpowerBeam = (TCPowerBeam)GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed("TCPowerBeam"),
            TargetCell, TargetMap);
        tcpowerBeam.duration = 120;
        tcpowerBeam.instigator = this;
        tcpowerBeam.weaponDef = ThingDef.Named("OrbitalTargeterPowerBeam");
        tcpowerBeam.StartStrike();
        if (!this.DestroyedOrNull())
        {
            Destroy();
        }
    }
}