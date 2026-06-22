using Deucarian.GameplayFoundation;

namespace Deucarian.GameplayFoundation.Samples
{
    public static class StatModifierPrimer
    {
        public static double BuildExampleDamage()
        {
            var damage = new StatId("stat.damage");
            var source = new ModifierSourceHandle("sample.research");
            var stats = new StatBlock();
            stats.SetBaseValue(damage, 10d);
            stats.AddModifier(new StatModifier(
                new StatModifierHandle("sample.research.damage-add"),
                source,
                damage,
                StatModifierOperation.Additive,
                5d));
            stats.AddModifier(new StatModifier(
                new StatModifierHandle("sample.research.damage-mul"),
                source,
                damage,
                StatModifierOperation.Multiplicative,
                2d));

            return stats.GetValue(damage);
        }
    }
}
