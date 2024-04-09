using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear;

public class TCPowerBeam : OrbitalStrike
{
    public const float Radius = 8f;

    private const int FiresStartedPerTick = 5;

    private static readonly IntRange FlameDamageAmountRange = new IntRange(25, 50);

    private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(3, 5);

    private static readonly List<Thing> tmpThings = [];

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

        var TCBeamDef = def;
        var NumFires = 5;
        for (var i = 0; i < NumFires; i++)
        {
            StartRandomFireAndDoFlameDamage(TCBeamDef);
        }
    }

    public void StartRandomFireAndDoFlameDamage(ThingDef OPBeamDef)
    {
        var EffRadius = 9f;
        var c = (from x in GenRadial.RadialCellsAround(Position, EffRadius, true)
            where x.InBounds(Map)
            select x).RandomElementByWeight(x => 1f - Mathf.Min(x.DistanceTo(Position) / EffRadius, 1f) + 0.05f);
        FireUtility.TryStartFireIn(c, Map, Rand.Range(0.1f, 0.5f), null);
        tmpThings.Clear();
        tmpThings.AddRange(c.GetThingList(Map));
        foreach (var thing1 in tmpThings)
        {
            var num = thing1 is not Corpse
                ? FlameDamageAmountRange.RandomInRange
                : CorpseFlameDamageAmountRange.RandomInRange;
            var beamFactor = 1.5f;
            num = (int)(num * beamFactor);
            if (num < 1)
            {
                num = 1;
            }

            if (num > 99)
            {
                num = 99;
            }

            var pawn = thing1 as Pawn;
            BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
            if (pawn != null)
            {
                battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn,
                    RulePackDefOf.DamageEvent_PowerBeam, instigator as Pawn);
                Find.BattleLog.Add(battleLogEntry_DamageTaken);
            }

            var flame = DamageDefOf.Flame;
            var amount = (float)num;
            var thing = instigator;
            var thingDef = weaponDef;
            thing1.TakeDamage(new DamageInfo(flame, amount, 0f, -1f, thing, null, thingDef))
                .AssociateWithLog(battleLogEntry_DamageTaken);
        }

        tmpThings.Clear();
    }
}