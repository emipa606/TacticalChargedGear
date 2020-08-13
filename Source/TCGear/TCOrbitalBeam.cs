using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear
{
	// Token: 0x0200000A RID: 10
	public class TCOrbitalBeam : Gas
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002B21 File Offset: 0x00000D21
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, true);
			this.destroyTick = Find.TickManager.TicksGame + GenTicks.SecondsToTicks(this.def.gas.expireSeconds.RandomInRange);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002B56 File Offset: 0x00000D56
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B70 File Offset: 0x00000D70
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
					TCPowerBeam tcpowerBeam = (TCPowerBeam)GenSpawn.Spawn(DefDatabase<ThingDef>.GetNamed("TCPowerBeam", true), TargetCell, TargetMap, 0);
					tcpowerBeam.duration = 120;
					tcpowerBeam.instigator = this;
					tcpowerBeam.weaponDef = ThingDefOf.OrbitalTargeterPowerBeam;
					tcpowerBeam.StartStrike();
					if (!ThingUtility.DestroyedOrNull(this))
					{
						this.Destroy(0);
					}
				}
			}
		}
	}
}
