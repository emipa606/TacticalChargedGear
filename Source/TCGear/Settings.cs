using System;
using UnityEngine;
using Verse;

namespace TCGear
{
	// Token: 0x02000007 RID: 7
	public class Settings : ModSettings
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000024CC File Offset: 0x000006CC
		public void DoWindowContents(Rect canvas)
		{
			float gap = 12f;
            Listing_Standard listing_Standard = new Listing_Standard
            {
                ColumnWidth = canvas.width
            };
            listing_Standard.Begin(canvas);
			listing_Standard.Gap(gap);
			checked
			{
				listing_Standard.Label(Translator.Translate("TCGear.RepPct") + "  " + (int)this.RepPct, -1f, null);
				this.RepPct = (float)((int)listing_Standard.Slider((float)((int)this.RepPct), 0f, 10f));
				listing_Standard.CheckboxLabeled(Translator.Translate("TCGear.BarrelReplace"), ref this.BarrelReplace, null);
				listing_Standard.Gap(gap);
				listing_Standard.Label(Translator.Translate("TCGear.ResPct") + "  " + (int)this.ResPct, -1f, null);
				this.ResPct = (float)((int)listing_Standard.Slider((float)((int)this.ResPct), 10f, 200f));
				listing_Standard.Gap(gap);
				Text.Font = 0;
				listing_Standard.Label("          " + Translator.Translate("TCGear.ResWarn"), -1f, null);
				listing_Standard.Gap(gap);
				listing_Standard.Label("          " + Translator.Translate("TCGear.ResTip"), -1f, null);
				Text.Font = GameFont.Small;
				listing_Standard.Gap(gap);
				listing_Standard.End();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000263C File Offset: 0x0000083C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.ResPct, "ResPct", 100f, false);
			Scribe_Values.Look<float>(ref this.RepPct, "RepPct", 5f, false);
			Scribe_Values.Look<bool>(ref this.BarrelReplace, "BarrelReplace", true, false);
		}

		// Token: 0x04000004 RID: 4
		public float ResPct = 100f;

		// Token: 0x04000005 RID: 5
		public float RepPct = 5f;

		// Token: 0x04000006 RID: 6
		public bool BarrelReplace = true;
	}
}
