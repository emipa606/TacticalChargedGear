using RimWorld;
using Verse;

namespace TCGear
{
    // Token: 0x02000002 RID: 2
    public class CompTCGRepair : ThingComp
    {
        // Token: 0x04000001 RID: 1
        public const int RepairTicks = 2500;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public CompProperties_TCGRepair Props => (CompProperties_TCGRepair) props;

        // Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
        public override void CompTick()
        {
            base.CompTick();
            if (parent.Spawned && Controller.Settings.RepPct > 0f && parent.IsHashIntervalTick(2500))
            {
                DoRepair(parent);
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020AF File Offset: 0x000002AF
        public override void CompTickRare()
        {
            base.CompTickRare();
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000020B8 File Offset: 0x000002B8
        public void DoRepair(Thing rep)
        {
            if (rep.Spawned && IsActive(rep))
            {
                var things = rep.Position.GetThingList(rep.Map);
                if (things.Count > 0)
                {
                    foreach (var thing in things)
                    {
                        if (thing.def.useHitPoints && thing != rep && validThingForRep(thing))
                        {
                            var max = thing.MaxHitPoints;
                            var repair = (int) (max * (Controller.Settings.RepPct / 100f));
                            if (repair > 0 && thing.HitPoints < max)
                            {
                                thing.HitPoints += repair;
                                if (thing.HitPoints >= max)
                                {
                                    thing.HitPoints = max;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x0000219C File Offset: 0x0000039C
        public bool validThingForRep(Thing thing)
        {
            var result = false;
            return thing.def.IsWeapon || thing.def.IsApparel || thing.def.IsShell || result;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021DC File Offset: 0x000003DC
        public bool IsActive(Thing rep)
        {
            var result = true;
            var CPT = rep.TryGetComp<CompPowerTrader>();
            return (CPT == null || CPT.PowerOn) && !rep.IsBrokenDown() && result;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x0000220C File Offset: 0x0000040C
        public override string CompInspectStringExtra()
        {
            return TranslatorFormattedStringExtensions.Translate("TCGear.FBack", Controller.Settings.RepPct.ToString(),
                1f.ToString("F2"));
        }
    }
}