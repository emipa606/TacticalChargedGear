using System;
using UnityEngine;
using Verse;

namespace TCGear
{
	// Token: 0x02000004 RID: 4
	public class Controller : Mod
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002273 File Offset: 0x00000473
		public override string SettingsCategory()
		{
			return Translator.Translate("TCGear.Name");
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002284 File Offset: 0x00000484
		public override void DoSettingsWindowContents(Rect canvas)
		{
			Controller.Settings.DoWindowContents(canvas);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002291 File Offset: 0x00000491
		public Controller(ModContentPack content) : base(content)
		{
			Controller.Settings = base.GetSettings<Settings>();
		}

		// Token: 0x04000002 RID: 2
		public static Settings Settings;
	}
}
