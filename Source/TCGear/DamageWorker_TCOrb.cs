using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear
{
    // Token: 0x02000005 RID: 5
    public class DamageWorker_TCOrb : DamageWorker
    {
        // Token: 0x0600000D RID: 13 RVA: 0x000022A8 File Offset: 0x000004A8
        public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
        {
            if (def.explosionHeatEnergyPerCell > 1.401298E-45f)
            {
                GenTemperature.PushHeat(explosion.Position, explosion.Map,
                    def.explosionHeatEnergyPerCell * cellsToAffect.Count);
            }

            MoteMaker.MakeStaticMote(explosion.Position, explosion.Map, ThingDefOf.Mote_ExplosionFlash,
                explosion.radius * 6f);
            MoteMaker.MakeStaticMote(explosion.Position, explosion.Map, ThingDefOf.Mote_ExplosionFlash,
                explosion.radius * 6f);
            ExplosionVisualEffectCenter(explosion);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002338 File Offset: 0x00000538
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            var damageResult = new DamageResult();
            if (victim.def.category == ThingCategory.Pawn && victim.def.useHitPoints && dinfo.Def.harmsHealth)
            {
                var num = dinfo.Amount;
                damageResult.totalDamageDealt = Mathf.Min(victim.HitPoints, GenMath.RoundRandom(num));
                victim.HitPoints -= Mathf.RoundToInt(damageResult.totalDamageDealt);
                if (victim.HitPoints <= 0)
                {
                    victim.HitPoints = 0;
                    victim.Kill(dinfo);
                }
            }

            return damageResult;
        }
    }
}