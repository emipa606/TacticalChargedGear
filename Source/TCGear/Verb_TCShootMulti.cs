using RimWorld;
using Verse;

namespace TCGear;

public class Verb_TCShootMulti : Verb_LaunchProjectile
{
    protected override int ShotsPerBurst => verbProps.burstShotCount;

    public override void WarmupComplete()
    {
        base.WarmupComplete();
        if (currentTarget.Thing is not Pawn pawn || pawn.Downed || !base.CasterIsPawn ||
            base.CasterPawn.skills == null)
        {
            return;
        }

        var num = !pawn.HostileTo(caster) ? 20f : 170f;
        var num2 = verbProps.AdjustedFullCycleTime(this, base.CasterPawn);
        base.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2);
    }

    protected override bool TryCastShot()
    {
        var count = 0;
        var tryCastShot = false;
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
            tryCastShot = base.TryCastShot();
            if (!tryCastShot)
            {
                if (count <= 0)
                {
                    return false;
                }

                if (EquipmentSource != null)
                {
                    EquipmentSource.GetComp<CompChangeableProjectile>().loadedCount = 0;
                }

                burstShotsLeft = 0;

                return false;
            }

            count++;
            burstShotsLeft--;
        }

        burstShotsLeft = 0;
        if (tryCastShot && base.CasterIsPawn)
        {
            base.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
        }

        return tryCastShot;
    }
}