using System;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Unity
{
    /// <summary>Reusable defaults for code calls and serialized typed keys; runtime state remains in the core service.</summary>
    public sealed class StatDefinitionAsset : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private double baseValue = 0d;
        public string Id => id;
        public string DisplayName => displayName;
        public StatKey Key => new AssetKey(id);
        public double BaseValue => baseValue;
        public void ApplyTo(StatBlock block) { if (block == null) throw new ArgumentNullException(nameof(block)); block.SetBaseValue(new StatId(Id), baseValue); }
        private sealed class AssetKey : StatKey { public AssetKey(string value) : base(value) { } }
    }
}
