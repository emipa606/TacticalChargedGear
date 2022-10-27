using System;
using System.Collections.Generic;
using Verse;

namespace TCGear;

[StaticConstructorOnStartup]
internal static class TCGOptions_Initializer
{
    static TCGOptions_Initializer()
    {
        LongEventHandler.QueueLongEvent(Setup, "LibraryStartup", false, null);
    }

    public static void Setup()
    {
        var projDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
        if (projDefs.Count > 0)
        {
            var TCGList = TCGResearchList();
            foreach (var ResDef in projDefs)
            {
                if (!TCGList.Contains(ResDef.defName))
                {
                    continue;
                }

                var Resbase = ResDef.baseCost;
                Resbase = checked((int)Math.Round(Resbase * (Controller.Settings.ResPct / 100f)));
                ResDef.baseCost = Resbase;
            }
        }

        if (Controller.Settings.BarrelReplace)
        {
            return;
        }

        var thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
        if (thingDefs.Count <= 0)
        {
            return;
        }

        foreach (var thingDef in thingDefs)
        {
            if (!thingDef.defName.StartsWith("Gun_CGear"))
            {
                continue;
            }

            var turret = false;
            if (thingDef.weaponTags != null)
            {
                var tags = thingDef.weaponTags;
                if (tags.Count > 0)
                {
                    using var enumerator3 = tags.GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        if (enumerator3.Current == "TurretGun")
                        {
                            turret = true;
                        }
                    }
                }
            }

            if (!turret || thingDef.Verbs == null)
            {
                continue;
            }

            var list = thingDef.Verbs;
            if (list.Count <= 0)
            {
                continue;
            }

            foreach (var VP in list)
            {
                if (VP == null)
                {
                    continue;
                }

                var unused = VP.consumeFuelPerShot;
                VP.consumeFuelPerShot = 0f;
            }
        }
    }

    public static List<string> TCGResearchList()
    {
        var list = new List<string>();
        list.AddDistinct("CGearTech");
        list.AddDistinct("CGearTech_Armour");
        list.AddDistinct("CGearTech_Turrets");
        list.AddDistinct("CGearTechEnhance");
        list.AddDistinct("CGearTechSpecialist");
        list.AddDistinct("CGearTech_AdvGrd");
        list.AddDistinct("CGearTech_AdvShells");
        list.AddDistinct("CGearTechTPC");
        list.AddDistinct("CGearTech_ArmJP");
        return list;
    }
}