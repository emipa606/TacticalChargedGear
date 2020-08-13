using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace TCGear
{
	// Token: 0x0200000C RID: 12
	public class TCPowerBeam : OrbitalStrike
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002D97 File Offset: 0x00000F97
		public override void StartStrike()
		{
			base.StartStrike();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public override void Tick()
		{
			base.Tick();
			if (!base.Destroyed)
			{
				ThingDef TCBeamDef = this.def;
				int NumFires = 5;
				for (int i = 0; i < NumFires; i++)
				{
					this.StartRandomFireAndDoFlameDamage(TCBeamDef);
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002DD8 File Offset: 0x00000FD8
		public void StartRandomFireAndDoFlameDamage(ThingDef OPBeamDef)
		{
			float EffRadius = 9f;
			IntVec3 c = GenCollection.RandomElementByWeight<IntVec3>(from x in GenRadial.RadialCellsAround(base.Position, EffRadius, true)
			where GenGrid.InBounds(x, this.Map)
			select x, (IntVec3 x) => 1f - Mathf.Min(IntVec3Utility.DistanceTo(x, this.Position) / EffRadius, 1f) + 0.05f);
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.5f));
			TCPowerBeam.tmpThings.Clear();
			TCPowerBeam.tmpThings.AddRange(GridsUtility.GetThingList(c, base.Map));
			for (int i = 0; i < TCPowerBeam.tmpThings.Count; i++)
			{
				int num = (!(TCPowerBeam.tmpThings[i] is Corpse)) ? TCPowerBeam.FlameDamageAmountRange.RandomInRange : TCPowerBeam.CorpseFlameDamageAmountRange.RandomInRange;
				float beamFactor = 1.5f;
				num = (int)((float)num * beamFactor);
				if (num < 1)
				{
					num = 1;
				}
				if (num > 99)
				{
					num = 99;
				}
				Pawn pawn = TCPowerBeam.tmpThings[i] as Pawn;
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				if (pawn != null)
				{
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_PowerBeam, this.instigator as Pawn);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
				}
				Thing thing = TCPowerBeam.tmpThings[i];
				DamageDef flame = DamageDefOf.Flame;
				float amount = (float)num;
				Thing instigator = this.instigator;
				ThingDef weaponDef = this.weaponDef;
				thing.TakeDamage(new DamageInfo(flame, amount, 0f, -1f, instigator, null, weaponDef, 0, null)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			TCPowerBeam.tmpThings.Clear();
		}

		// Token: 0x0400000F RID: 15
		public const float Radius = 8f;

		// Token: 0x04000010 RID: 16
		private const int FiresStartedPerTick = 5;

		// Token: 0x04000011 RID: 17
		private static readonly IntRange FlameDamageAmountRange = new IntRange(25, 50);

		// Token: 0x04000012 RID: 18
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(3, 5);

		// Token: 0x04000013 RID: 19
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
