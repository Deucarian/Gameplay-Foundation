using System;
using NUnit.Framework;

namespace Deucarian.GameplayFoundation.Tests
{
    public sealed class GameplayFoundationTests
    {
        private static readonly StatId Damage = new StatId("stat.damage");
        private static readonly ModifierSourceHandle SourceA = new ModifierSourceHandle("source.a");
        private static readonly ModifierSourceHandle SourceB = new ModifierSourceHandle("source.b");

        [Test]
        public void DeterministicRandom_EqualSeedsProduceEqualSequences()
        {
            var left = new DeterministicRandom(1234);
            var right = new DeterministicRandom(1234);

            for (int index = 0; index < 16; index++)
            {
                Assert.AreEqual(left.NextUInt(), right.NextUInt());
            }
        }

        [Test]
        public void DeterministicRandom_DifferentSeedsProduceDifferentSequences()
        {
            var left = new DeterministicRandom(1234);
            var right = new DeterministicRandom(5678);

            bool anyDifferent = false;
            for (int index = 0; index < 16; index++)
            {
                anyDifferent |= left.NextUInt() != right.NextUInt();
            }

            Assert.IsTrue(anyDifferent);
        }

        [Test]
        public void FixedTickStepper_AdvancesAndResets()
        {
            var stepper = new FixedTickStepper(0.25d);

            Assert.AreEqual(0, stepper.Advance(0.10d));
            Assert.AreEqual(0L, stepper.TickIndex);
            Assert.AreEqual(2, stepper.Advance(0.45d));
            Assert.AreEqual(2L, stepper.TickIndex);
            Assert.That(stepper.AccumulatorSeconds, Is.EqualTo(0.05d).Within(0.000001d));

            stepper.Reset();
            Assert.AreEqual(0L, stepper.TickIndex);
            Assert.AreEqual(0d, stepper.AccumulatorSeconds);
        }

        [Test]
        public void StatBlock_AdditiveModifiersAreApplied()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.add-a", SourceA, StatModifierOperation.Additive, 5d));
            stats.AddModifier(Mod("mod.add-b", SourceB, StatModifierOperation.Additive, 2d));

            Assert.AreEqual(17d, stats.GetValue(Damage));
        }

        [Test]
        public void StatBlock_MultiplicativeModifiersAreApplied()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.mul-a", SourceA, StatModifierOperation.Multiplicative, 2d));
            stats.AddModifier(Mod("mod.mul-b", SourceB, StatModifierOperation.Multiplicative, 1.5d));

            Assert.AreEqual(30d, stats.GetValue(Damage));
        }

        [Test]
        public void StatBlock_OverridePrecedenceUsesDeterministicPriorityAndInsertionOrder()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.override-low", SourceA, StatModifierOperation.Override, 12d, priority: 0));
            stats.AddModifier(Mod("mod.override-first", SourceA, StatModifierOperation.Override, 20d, priority: 5));
            stats.AddModifier(Mod("mod.override-second", SourceB, StatModifierOperation.Override, 30d, priority: 5));

            Assert.AreEqual(30d, stats.GetValue(Damage));
        }

        [Test]
        public void StatBlock_ClampOrderingRunsAfterOverride()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.add", SourceA, StatModifierOperation.Additive, 90d));
            stats.AddModifier(Mod("mod.override", SourceA, StatModifierOperation.Override, 50d));
            stats.AddModifier(Mod("mod.min", SourceA, StatModifierOperation.Minimum, 60d));
            stats.AddModifier(Mod("mod.max", SourceA, StatModifierOperation.Maximum, 55d));

            Assert.AreEqual(55d, stats.GetValue(Damage));
        }

        [Test]
        public void StatBlock_EqualPriorityModifiersUseInsertionOrder()
        {
            StatBlock stats = CreateBaseDamage(0d);
            stats.AddModifier(Mod("mod.mul-a", SourceA, StatModifierOperation.Multiplicative, 10d));
            stats.AddModifier(Mod("mod.add-a", SourceA, StatModifierOperation.Additive, 2d));
            stats.AddModifier(Mod("mod.add-b", SourceB, StatModifierOperation.Additive, 3d));

            Assert.AreEqual(50d, stats.GetValue(Damage));
        }

        [Test]
        public void StatBlock_RemovesModifiersBySource()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.a1", SourceA, StatModifierOperation.Additive, 5d));
            stats.AddModifier(Mod("mod.a2", SourceA, StatModifierOperation.Additive, 1d));
            stats.AddModifier(Mod("mod.b1", SourceB, StatModifierOperation.Additive, 7d));

            Assert.AreEqual(2, stats.RemoveModifiersFromSource(SourceA));
            Assert.AreEqual(17d, stats.GetValue(Damage));
        }

        [Test]
        public void StatBlock_RejectsDuplicateHandles()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.same", SourceA, StatModifierOperation.Additive, 1d));

            Assert.IsFalse(stats.TryAddModifier(Mod("mod.same", SourceB, StatModifierOperation.Additive, 2d)));
            Assert.Throws<InvalidOperationException>(() => stats.AddModifier(Mod("mod.same", SourceB, StatModifierOperation.Additive, 2d)));
        }

        [Test]
        public void StatBlock_SnapshotIsConsistentAfterSourceRemoval()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.a", SourceA, StatModifierOperation.Additive, 5d));
            StatSnapshot before = stats.CreateSnapshot();
            stats.RemoveModifiersFromSource(SourceA);
            StatSnapshot after = stats.CreateSnapshot();

            Assert.AreEqual(15d, before.GetValueOrDefault(Damage));
            Assert.AreEqual(10d, after.GetValueOrDefault(Damage));
        }

        [Test]
        public void StatBlock_ZeroModifiersReturnBaseValue()
        {
            StatBlock stats = CreateBaseDamage(42d);

            Assert.AreEqual(42d, stats.GetValue(Damage));
        }

        [Test]
        public void InvalidNumericInputThrows()
        {
            StatBlock stats = CreateBaseDamage(1d);

            Assert.Throws<ArgumentOutOfRangeException>(() => stats.SetBaseValue(Damage, double.NaN));
            Assert.Throws<ArgumentOutOfRangeException>(() => new FixedTickStepper(double.PositiveInfinity));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CooldownTimer().Start(double.NaN));
            Assert.Throws<ArgumentException>(() => new ContentId("Bad Id"));
        }

        [Test]
        public void RepresentativeStatEvaluation_HasNoSteadyStateAllocationsAfterWarmup()
        {
            StatBlock stats = CreateBaseDamage(10d);
            stats.AddModifier(Mod("mod.add", SourceA, StatModifierOperation.Additive, 5d));
            stats.AddModifier(Mod("mod.mul", SourceA, StatModifierOperation.Multiplicative, 2d));
            stats.AddModifier(Mod("mod.max", SourceB, StatModifierOperation.Maximum, 40d));

            for (int index = 0; index < 1000; index++)
            {
                stats.GetValue(Damage);
            }

            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int index = 0; index < 10000; index++)
            {
                stats.GetValue(Damage);
            }

            long after = GC.GetAllocatedBytesForCurrentThread();
            Assert.AreEqual(0L, after - before);
            Assert.AreEqual(30d, stats.GetValue(Damage));
        }

        private static StatBlock CreateBaseDamage(double value)
        {
            var stats = new StatBlock();
            stats.SetBaseValue(Damage, value);
            return stats;
        }

        private static StatModifier Mod(
            string handle,
            ModifierSourceHandle source,
            StatModifierOperation operation,
            double value,
            int priority = 0)
        {
            return new StatModifier(new StatModifierHandle(handle), source, Damage, operation, value, priority);
        }
    }
}
