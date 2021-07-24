using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear
{
    // Token: 0x0200000C RID: 12
    public class TCPowerBeam : OrbitalStrike
    {
        // Token: 0x0400000F RID: 15
        public const float Radius = 8f;

        // Token: 0x04000010 RID: 16
        private const int FiresStartedPerTick = 5;

        // Token: 0x04000011 RID: 17
        private static readonly IntRange FlameDamageAmountRange = new IntRange(25, 50);

        // Token: 0x04000012 RID: 18
        private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(3, 5);

        // Token: 0x04000013 RID: 19
        private static readonly List<Thing> tmpThings = new List<Thing>();

        // Token: 0x0600002C RID: 44 RVA: 0x00002D97 File Offset: 0x00000F97
        public override void StartStrike()
        {
            base.StartStrike();
        }

        // Token: 0x0600002D RID: 45 RVA: 0x00002DA0 File Offset: 0x00000FA0
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

        // Token: 0x0600002E RID: 46 RVA: 0x00002DD8 File Offset: 0x00000FD8
        public void StartRandomFireAndDoFlameDamage(ThingDef OPBeamDef)
        {
            var EffRadius = 9f;
            var c = (from x in GenRadial.RadialCellsAround(Position, EffRadius, true)
                where x.InBounds(Map)
                select x).RandomElementByWeight(x => 1f - Mathf.Min(x.DistanceTo(Position) / EffRadius, 1f) + 0.05f);
            FireUtility.TryStartFireIn(c, Map, Rand.Range(0.1f, 0.5f));
            tmpThings.Clear();
            tmpThings.AddRange(c.GetThingList(Map));
            foreach (var thing1 in tmpThings)
            {
                var num = !(thing1 is Corpse)
                    ? FlameDamageAmountRange.RandomInRange
                    : CorpseFlameDamageAmountRange.RandomInRange;
                var beamFactor = 1.5f;
                num = (int) (num * beamFactor);
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
                var amount = (float) num;
                var thing = instigator;
                var thingDef = weaponDef;
                thing1.TakeDamage(new DamageInfo(flame, amount, 0f, -1f, thing, null, thingDef))
                    .AssociateWithLog(battleLogEntry_DamageTaken);
            }

            tmpThings.Clear();
        }
    }
}