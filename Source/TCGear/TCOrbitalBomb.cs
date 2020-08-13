using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear
{
	// Token: 0x0200000B RID: 11
	public class TCOrbitalBomb : Gas
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002C5B File Offset: 0x00000E5B
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, true);
			this.destroyTick = Find.TickManager.TicksGame + GenTicks.SecondsToTicks(this.def.gas.expireSeconds.RandomInRange);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002C90 File Offset: 0x00000E90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002CAC File Offset: 0x00000EAC
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(0);
			}
			this.graphicRotation += this.graphicRotationSpeed;
			if (!ThingUtility.DestroyedOrNull(this))
			{
				Map TargetMap = base.Map;
				IntVec3 TargetCell = base.Position;
				if (Find.TickManager.TicksGame % 10 == 0)
				{
					MoteMaker.ThrowSmoke(GenThing.TrueCenter(this) + new Vector3(0f, 0f, 0.1f), TargetMap, 1f);
				}
				if (Find.TickManager.TicksGame % 300 == 0)
				{
					TCBombardment tcbombardment = (TCBombardment)GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed("TCBombardment", true), TargetCell, TargetMap, 0);
					tcbombardment.duration = 120;
					tcbombardment.instigator = this;
					tcbombardment.weaponDef = ThingDefOf.OrbitalTargeterBombardment;
					tcbombardment.StartStrike();
					if (!ThingUtility.DestroyedOrNull(this))
					{
						this.Destroy(0);
					}
				}
			}
		}
	}
}
