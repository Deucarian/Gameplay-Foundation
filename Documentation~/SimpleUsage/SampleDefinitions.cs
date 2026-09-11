namespace Deucarian.GameplayFoundation.Samples.SimpleUsage
{
    [StatKeySet]
    public static class Stats
    {
        public static StatKey MoveSpeed => new Definition();
        private sealed class Definition : StatKey
        {
            public Definition() : base("sample.move-speed") { }
        }
    }
    [GameplayTagKeySet]
    public static class Tags
    {
        public static GameplayTagKey Flying => new Definition();
        private sealed class Definition : GameplayTagKey
        {
            public Definition() : base("sample.flying") { }
        }
    }
}
