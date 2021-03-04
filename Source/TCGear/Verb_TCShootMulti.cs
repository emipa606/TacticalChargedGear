using RimWorld;
using Verse;

namespace TCGear
{
    // Token: 0x0200000D RID: 13
    public class Verb_TCShootMulti : Verb_LaunchProjectile
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000031 RID: 49 RVA: 0x00002F98 File Offset: 0x00001198
        protected override int ShotsPerBurst => verbProps.burstShotCount;

        // Token: 0x06000032 RID: 50 RVA: 0x00002FA8 File Offset: 0x000011A8
        public override void WarmupComplete()
        {
            base.WarmupComplete();
            var pawn = currentTarget.Thing as Pawn;
            if (pawn != null && !pawn.Downed && base.CasterIsPawn && base.CasterPawn.skills != null)
            {
                var num = !pawn.HostileTo(caster) ? 20f : 170f;
                var num2 = verbProps.AdjustedFullCycleTime(this, base.CasterPawn);
                base.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2);
            }
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00003034 File Offset: 0x00001234
        protected override bool TryCastShot()
        {
            var count = 0;
            var flag = false;
            if (EquipmentSource != null)
            {
                var loaded = EquipmentSource.GetComp<CompChangeableProjectile>().loadedCount;
                if (loaded > ShotsPerBurst)
                {
                    loaded = ShotsPerBurst;
                }

                EquipmentSource.GetComp<CompChangeableProjectile>().loadedCount = loaded * ShotsPerBurst;
            }

            while (count < ShotsPerBurst)
            {
                flag = base.TryCastShot();
                if (!flag)
                {
                    if (count > 0)
                    {
                        EquipmentSource.GetComp<CompChangeableProjectile>().loadedCount = 0;
                        burstShotsLeft = 0;
                    }

                    return flag;
                }

                count++;
                burstShotsLeft--;
            }

            burstShotsLeft = 0;
            if (flag && base.CasterIsPawn)
            {
                base.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
            }

            return flag;
        }
    }
}