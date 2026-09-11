using System;

namespace Deucarian.GameplayFoundation
{
    /// <summary>Typed definition access to the existing authoritative stat block.</summary>
    public static class TypedStatExtensions
    {
        public static double GetValue(this StatBlock stats, IStatKey stat)
        {
            if (stats == null) throw new ArgumentNullException(nameof(stats));
            return stats.GetValue(Id(stat));
        }

        public static double GetBaseValue(this StatBlock stats, IStatKey stat)
        {
            if (stats == null) throw new ArgumentNullException(nameof(stats));
            return stats.GetBaseValue(Id(stat));
        }

        public static void SetBaseValue(this StatBlock stats, IStatKey stat, double value)
        {
            if (stats == null) throw new ArgumentNullException(nameof(stats));
            stats.SetBaseValue(Id(stat), value);
        }

        public static GameplayTag ToGameplayTag(this IGameplayTagKey tag) =>
            new GameplayTag(tag != null ? tag.Id : throw new ArgumentNullException(nameof(tag), "Select an existing gameplay tag or pass its named key."));

        private static StatId Id(IStatKey stat) => new StatId(stat != null ? stat.Id :
            throw new ArgumentNullException(nameof(stat), "Select an existing stat in the Inspector or pass a named StatKey."));
    }
}
