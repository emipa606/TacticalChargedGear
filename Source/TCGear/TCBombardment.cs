using System.Linq;
using RimWorld;
using Verse;

namespace TCGear
{
    // Token: 0x02000008 RID: 8
    public class TCBombardment : OrbitalStrike
    {
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

        // Token: 0x0600001C RID: 28 RVA: 0x00002816 File Offset: 0x00000A16
        // Note: this type is marked as 'beforefieldinit'.
        static TCBombardment()
        {
            var simpleCurve = new SimpleCurve
            {
                new CurvePoint(0f, 1f), new CurvePoint(30f, 0.1f)
            };
            DistanceChanceFactor = simpleCurve;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000026B2 File Offset: 0x000008B2
        public override void StartStrike()
        {
            base.StartStrike();
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000026BA File Offset: 0x000008BA
        public override void Tick()
        {
            base.Tick();
            if (!Destroyed)
            {
                if (Find.TickManager.TicksGame % 28 == 0)
                {
                    CreateRandomExplosion();
                }

                if (Find.TickManager.TicksGame % 30 == 0)
                {
                    StartRandomFire();
                }
            }
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000026F4 File Offset: 0x000008F4
        public void CreateRandomExplosion()
        {
            var def = this.def;
            var TCBombDmg = 40;
            var TCBombRad = 12;
            var TCBombMinBlast = 5;
            var TCBombMaxBlast = 7;
            var intVec = (from x in GenRadial.RadialCellsAround(Position, TCBombRad, true)
                where x.InBounds(Map)
                select x).RandomElementByWeight(x => DistanceChanceFactor.Evaluate(x.DistanceTo(Position)));
            var num = (float) Rand.Range(TCBombMinBlast, TCBombMaxBlast);
            var map = Map;
            var radius = num;
            var bomb = DamageDefOf.Bomb;
            var instigator = this.instigator;
            var weaponDef = this.weaponDef;
            GenExplosion.DoExplosion(intVec, map, radius, bomb, instigator, TCBombDmg, -1f, null, weaponDef, def);
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000027AC File Offset: 0x000009AC
        public void StartRandomFire()
        {
            var def = this.def;
            var TCFireRad = 15;
            FireUtility.TryStartFireIn((from x in GenRadial.RadialCellsAround(Position, TCFireRad, true)
                    where x.InBounds(Map)
                    select x).RandomElementByWeight(x => DistanceChanceFactor.Evaluate(x.DistanceTo(Position))), Map,
                Rand.Range(0.1f, 0.925f));
        }
    }
}