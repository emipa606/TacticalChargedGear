using System;
using System.Linq;
using RimWorld;
using Verse;

namespace TCGear
{
	// Token: 0x02000008 RID: 8
	public class TCBombardment : OrbitalStrike
	{
		// Token: 0x06000017 RID: 23 RVA: 0x000026B2 File Offset: 0x000008B2
		public override void StartStrike()
		{
			base.StartStrike();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026BA File Offset: 0x000008BA
		public override void Tick()
		{
			base.Tick();
			if (!base.Destroyed)
			{
				if (Find.TickManager.TicksGame % 28 == 0)
				{
					this.CreateRandomExplosion();
				}
				if (Find.TickManager.TicksGame % 30 == 0)
				{
					this.StartRandomFire();
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026F4 File Offset: 0x000008F4
		public void CreateRandomExplosion()
		{
			ThingDef def = this.def;
			int TCBombDmg = 40;
			int TCBombRad = 12;
			int TCBombMinBlast = 5;
			int TCBombMaxBlast = 7;
			IntVec3 intVec = GenCollection.RandomElementByWeight<IntVec3>(from x in GenRadial.RadialCellsAround(base.Position, (float)TCBombRad, true)
			where GenGrid.InBounds(x, base.Map)
			select x, (IntVec3 x) => TCBombardment.DistanceChanceFactor.Evaluate(IntVec3Utility.DistanceTo(x, base.Position)));
			float num = (float)Rand.Range(TCBombMinBlast, TCBombMaxBlast);
			Map map = base.Map;
			float radius = num;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing instigator = this.instigator;
			ThingDef weaponDef = this.weaponDef;
			GenExplosion.DoExplosion(intVec, map, radius, bomb, instigator, TCBombDmg, -1f, null, weaponDef, def, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027AC File Offset: 0x000009AC
		public void StartRandomFire()
		{
			ThingDef def = this.def;
			int TCFireRad = 15;
			FireUtility.TryStartFireIn(GenCollection.RandomElementByWeight<IntVec3>(from x in GenRadial.RadialCellsAround(base.Position, (float)TCFireRad, true)
			where GenGrid.InBounds(x, base.Map)
			select x, (IntVec3 x) => TCBombardment.DistanceChanceFactor.Evaluate(IntVec3Utility.DistanceTo(x, base.Position))), base.Map, Rand.Range(0.1f, 0.925f));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002816 File Offset: 0x00000A16
		// Note: this type is marked as 'beforefieldinit'.
		static TCBombardment()
		{
			SimpleCurve simpleCurve = new SimpleCurve();
			simpleCurve.Add(new CurvePoint(0f, 1f), true);
			simpleCurve.Add(new CurvePoint(30f, 0.1f), true);
			TCBombardment.DistanceChanceFactor = simpleCurve;
		}

		// Token: 0x04000007 RID: 7
		private const int ImpactAreaRadius = 12;

		// Token: 0x04000008 RID: 8
		private const int ExplosionRadiusMin = 5;

		// Token: 0x04000009 RID: 9
		private const int ExplosionRadiusMax = 7;

		// Token: 0x0400000A RID: 10
		public const int EffectiveRadius = 13;

		// Token: 0x0400000B RID: 11
		public const int RandomFireRadius = 15;

		// Token: 0x0400000C RID: 12
		private const int BombIntervalTicks = 28;

		// Token: 0x0400000D RID: 13
		private const int StartRandomFireEveryTicks = 30;

		// Token: 0x0400000E RID: 14
		private static readonly SimpleCurve DistanceChanceFactor;
	}
}
