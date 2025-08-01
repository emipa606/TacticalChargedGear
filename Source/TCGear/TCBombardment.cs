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

    protected override void Tick()
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
        var intVec = (from x in GenRadial.RadialCellsAround(Position, 12, true)
            where x.InBounds(Map)
            select x).RandomElementByWeight(x => DistanceChanceFactor.Evaluate(x.DistanceTo(Position)));
        var num = (float)Rand.Range(5, 7);
        GenExplosion.DoExplosion(intVec, Map, num, DamageDefOf.Bomb, instigator, 40, -1f, null, weaponDef, def);
    }

    public void StartRandomFire()
    {
        _ = def;
        var TCFireRad = 15;
        FireUtility.TryStartFireIn((from x in GenRadial.RadialCellsAround(Position, TCFireRad, true)
                where x.InBounds(Map)
                select x).RandomElementByWeight(x => DistanceChanceFactor.Evaluate(x.DistanceTo(Position))), Map,
            Rand.Range(0.1f, 0.925f), null);
    }
}