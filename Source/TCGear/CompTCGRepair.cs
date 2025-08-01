using RimWorld;
using Verse;

namespace TCGear;

public class CompTCGRepair : ThingComp
{
    public const int RepairTicks = 2500;

    public CompProperties_TCGRepair Props => (CompProperties_TCGRepair)props;

    public override void CompTick()
    {
        base.CompTick();
        if (parent.Spawned && Controller.Settings.RepPct > 0f && parent.IsHashIntervalTick(2500))
        {
            DoRepair(parent);
        }
    }


    private void DoRepair(Thing rep)
    {
        if (!rep.Spawned || !isActive(rep))
        {
            return;
        }

        var things = rep.Position.GetThingList(rep.Map);
        if (things.Count <= 0)
        {
            return;
        }

        foreach (var thing in things)
        {
            if (!thing.def.useHitPoints || thing == rep || !validThingForRep(thing))
            {
                continue;
            }

            var max = thing.MaxHitPoints;
            var repair = (int)(max * (Controller.Settings.RepPct / 100f));
            if (repair <= 0 || thing.HitPoints >= max)
            {
                continue;
            }

            thing.HitPoints += repair;
            if (thing.HitPoints >= max)
            {
                thing.HitPoints = max;
            }
        }
    }

    private static bool validThingForRep(Thing thing)
    {
        return thing.def.IsWeapon || thing.def.IsApparel || thing.def.IsShell;
    }

    private static bool isActive(Thing rep)
    {
        var cpt = rep.TryGetComp<CompPowerTrader>();
        return (cpt == null || cpt.PowerOn) && !rep.IsBrokenDown();
    }

    public override string CompInspectStringExtra()
    {
        return "TCGear.FBack".Translate(Controller.Settings.RepPct.ToString(),
            1f.ToString("F2"));
    }
}