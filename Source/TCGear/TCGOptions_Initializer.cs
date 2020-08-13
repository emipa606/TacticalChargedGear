using System;
using System.Collections.Generic;
using Verse;

namespace TCGear
{
	// Token: 0x02000009 RID: 9
	[StaticConstructorOnStartup]
	internal static class TCGOptions_Initializer
	{
		// Token: 0x06000021 RID: 33 RVA: 0x0000289A File Offset: 0x00000A9A
		static TCGOptions_Initializer()
		{
			LongEventHandler.QueueLongEvent(new Action(TCGOptions_Initializer.Setup), "LibraryStartup", false, null, true);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000028B8 File Offset: 0x00000AB8
		public static void Setup()
		{
			List<ResearchProjectDef> projDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			if (projDefs.Count > 0)
			{
				List<string> TCGList = TCGOptions_Initializer.TCGResearchList();
				foreach (ResearchProjectDef ResDef in projDefs)
				{
					if (TCGList.Contains(ResDef.defName))
					{
						float Resbase = ResDef.baseCost;
						Resbase = (float)(checked((int)Math.Round((double)(unchecked(Resbase * (Controller.Settings.ResPct / 100f))))));
						ResDef.baseCost = Resbase;
					}
				}
			}
			if (!Controller.Settings.BarrelReplace)
			{
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				if (thingDefs.Count > 0)
				{
					foreach (ThingDef thingDef in thingDefs)
					{
						if (thingDef.defName.StartsWith("Gun_CGear"))
						{
							bool turret = false;
							if (((thingDef != null) ? thingDef.weaponTags : null) != null)
							{
								List<string> tags = thingDef.weaponTags;
								if (tags.Count > 0)
								{
									using (List<string>.Enumerator enumerator3 = tags.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											if (enumerator3.Current == "TurretGun")
											{
												turret = true;
											}
										}
									}
								}
							}
							if (turret && ((thingDef != null) ? thingDef.Verbs : null) != null)
							{
								List<VerbProperties> list = thingDef.Verbs;
								if (list.Count > 0)
								{
									foreach (VerbProperties VP in list)
									{
										if (VP != null)
										{
											float consumeFuelPerShot = VP.consumeFuelPerShot;
											VP.consumeFuelPerShot = 0f;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002AAC File Offset: 0x00000CAC
		public static List<string> TCGResearchList()
		{
			List<string> list = new List<string>();
			GenCollection.AddDistinct<string>(list, "CGearTech");
			GenCollection.AddDistinct<string>(list, "CGearTech_Armour");
			GenCollection.AddDistinct<string>(list, "CGearTech_Turrets");
			GenCollection.AddDistinct<string>(list, "CGearTechEnhance");
			GenCollection.AddDistinct<string>(list, "CGearTechSpecialist");
			GenCollection.AddDistinct<string>(list, "CGearTech_AdvGrd");
			GenCollection.AddDistinct<string>(list, "CGearTech_AdvShells");
			GenCollection.AddDistinct<string>(list, "CGearTechTPC");
			GenCollection.AddDistinct<string>(list, "CGearTech_ArmJP");
			return list;
		}
	}
}
