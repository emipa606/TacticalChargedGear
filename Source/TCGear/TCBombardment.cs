using System.Linq;
using RimWorld;
using Verse;

namespace TCGear;

public class TCBombardment : OrbitalStrike
{
    private const int ImpactAreaRadius = 12;

    private const int ExplosionRadiusMin = 5;

    private const int ExplosionRadiusMax = 7;

    public const int EffectiveRadius = 13;

    public const int RandomFireRadius = 15;

    private const int BombIntervalTicks = 28;

    private const int StartRandomFireEveryTicks = 30;

    private static readonly SimpleCurve DistanceChanceFactor;

    // Note: this type is marked as 'beforefieldinit'.
    static TCBombardment()
    {
        var simpleCurve = new SimpleCurve
        {
            new CurvePoint(0f, 1f), new CurvePoint(30f, 0.1f)
        };
        DistanceChanceFactor = simpleCurve;
    }

    public override void StartStrike()
    {
        base.StartStrike();
    }

    public override void Tick()
    {
        base.Tick();
        if (Destroyed)
        {
            return;
        }

        if (Find.TickManager.TicksGame % 28 == 0)
        {
            CreateRandomExplosion();
        }

        if (Find.TickManager.TicksGame % 30 == 0)
        {
            StartRandomFire();
        }
    }

    public void CreateRandomExplosion()
    {
        var thingDef = def;
        var TCBombDmg = 40;
        var TCBombRad = 12;
        var TCBombMinBlast = 5;
        var TCBombMaxBlast = 7;
        var intVec = (from x in GenRadial.RadialCellsAround(Position, TCBombRad, true)
            where x.InBounds(Map)
            select x).RandomElementByWeight(x => DistanceChanceFactor.Evaluate(x.DistanceTo(Position)));
        var num = (float)Rand.Range(TCBombMinBlast, TCBombMaxBlast);
        var map = Map;
        var bomb = DamageDefOf.Bomb;
        var thing = instigator;
        var weapon = weaponDef;
        GenExplosion.DoExplosion(intVec, map, num, bomb, thing, TCBombDmg, -1f, null, weapon, thingDef);
    }

    public void StartRandomFire()
    {
        var unused = def;
        var TCFireRad = 15;
        FireUtility.TryStartFireIn((from x in GenRadial.RadialCellsAround(Position, TCFireRad, true)
                where x.InBounds(Map)
                select x).RandomElementByWeight(x => DistanceChanceFactor.Evaluate(x.DistanceTo(Position))), Map,
            Rand.Range(0.1f, 0.925f));
    }
}