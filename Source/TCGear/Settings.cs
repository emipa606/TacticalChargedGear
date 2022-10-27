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
        var listing_Standard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listing_Standard.Begin(canvas);
        listing_Standard.Gap(gap);
        checked
        {
            listing_Standard.Label("TCGear.RepPct".Translate() + "  " + (int)RepPct);
            RepPct = (int)listing_Standard.Slider((int)RepPct, 0f, 10f);
            listing_Standard.CheckboxLabeled("TCGear.BarrelReplace".Translate(), ref BarrelReplace);
            listing_Standard.Gap(gap);
            listing_Standard.Label("TCGear.ResPct".Translate() + "  " + (int)ResPct);
            ResPct = (int)listing_Standard.Slider((int)ResPct, 10f, 200f);
            listing_Standard.Gap(gap);
            Text.Font = 0;
            listing_Standard.Label("          " + "TCGear.ResWarn".Translate());
            listing_Standard.Gap(gap);
            listing_Standard.Label("          " + "TCGear.ResTip".Translate());
            Text.Font = GameFont.Small;
            listing_Standard.Gap(gap);
            if (Controller.currentVersion != null)
            {
                listing_Standard.Gap();
                GUI.contentColor = Color.gray;
                listing_Standard.Label("TCGear.ModVersion".Translate(Controller.currentVersion));
                GUI.contentColor = Color.white;
            }

            listing_Standard.End();
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