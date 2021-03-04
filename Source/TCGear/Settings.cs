using UnityEngine;
using Verse;

namespace TCGear
{
    // Token: 0x02000007 RID: 7
    public class Settings : ModSettings
    {
        // Token: 0x04000006 RID: 6
        public bool BarrelReplace = true;

        // Token: 0x04000005 RID: 5
        public float RepPct = 5f;

        // Token: 0x04000004 RID: 4
        public float ResPct = 100f;

        // Token: 0x06000014 RID: 20 RVA: 0x000024CC File Offset: 0x000006CC
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
                listing_Standard.Label("TCGear.RepPct".Translate() + "  " + (int) RepPct);
                RepPct = (int) listing_Standard.Slider((int) RepPct, 0f, 10f);
                listing_Standard.CheckboxLabeled("TCGear.BarrelReplace".Translate(), ref BarrelReplace);
                listing_Standard.Gap(gap);
                listing_Standard.Label("TCGear.ResPct".Translate() + "  " + (int) ResPct);
                ResPct = (int) listing_Standard.Slider((int) ResPct, 10f, 200f);
                listing_Standard.Gap(gap);
                Text.Font = 0;
                listing_Standard.Label("          " + "TCGear.ResWarn".Translate());
                listing_Standard.Gap(gap);
                listing_Standard.Label("          " + "TCGear.ResTip".Translate());
                Text.Font = GameFont.Small;
                listing_Standard.Gap(gap);
                listing_Standard.End();
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x0000263C File Offset: 0x0000083C
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ResPct, "ResPct", 100f);
            Scribe_Values.Look(ref RepPct, "RepPct", 5f);
            Scribe_Values.Look(ref BarrelReplace, "BarrelReplace", true);
        }
    }
}