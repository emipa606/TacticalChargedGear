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

    private static void Setup()
    {
        var projDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
        if (projDefs.Count > 0)
        {
            var tcgList = tcgResearchList();
            foreach (var resDef in projDefs)
            {
                if (!tcgList.Contains(resDef.defName))
                {
                    continue;
                }

                var Resbase = resDef.baseCost;
                Resbase = checked((int)Math.Round(Resbase * (Controller.Settings.ResPct / 100f)));
                resDef.baseCost = Resbase;
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

            foreach (var vp in list)
            {
                if (vp == null)
                {
                    continue;
                }

                _ = vp.consumeFuelPerShot;
                vp.consumeFuelPerShot = 0f;
            }
        }
    }

    private static List<string> tcgResearchList()
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