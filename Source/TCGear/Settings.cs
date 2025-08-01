using UnityEngine;
using Verse;

namespace TCGear;

public class Settings : ModSettings
{
    public bool BarrelReplace = true;

    public float RepPct = 5f;

    public float ResPct = 100f;

    public void DoWindowContents(Rect canvas)
    {
        var gap = 12f;
        var listingStandard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listingStandard.Begin(canvas);
        listingStandard.Gap(gap);
        checked
        {
            listingStandard.Label("TCGear.RepPct".Translate() + "  " + (int)RepPct);
            RepPct = (int)listingStandard.Slider((int)RepPct, 0f, 10f);
            listingStandard.CheckboxLabeled("TCGear.BarrelReplace".Translate(), ref BarrelReplace);
            listingStandard.Gap(gap);
            listingStandard.Label("TCGear.ResPct".Translate() + "  " + (int)ResPct);
            ResPct = (int)listingStandard.Slider((int)ResPct, 10f, 200f);
            listingStandard.Gap(gap);
            Text.Font = 0;
            listingStandard.Label("          " + "TCGear.ResWarn".Translate());
            listingStandard.Gap(gap);
            listingStandard.Label("          " + "TCGear.ResTip".Translate());
            Text.Font = GameFont.Small;
            listingStandard.Gap(gap);
            if (Controller.currentVersion != null)
            {
                listingStandard.Gap();
                GUI.contentColor = Color.gray;
                listingStandard.Label("TCGear.ModVersion".Translate(Controller.currentVersion));
                GUI.contentColor = Color.white;
            }

            listingStandard.End();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ResPct, "ResPct", 100f);
        Scribe_Values.Look(ref RepPct, "RepPct", 5f);
        Scribe_Values.Look(ref BarrelReplace, "BarrelReplace", true);
    }
}