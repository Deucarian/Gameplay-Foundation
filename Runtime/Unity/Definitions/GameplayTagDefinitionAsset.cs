using System;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Unity
{
    /// <summary>Reusable defaults for code calls and serialized typed keys; runtime state remains in the core service.</summary>
    public sealed class GameplayTagDefinitionAsset : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description = string.Empty;
        public string Id => id;
        public string DisplayName => displayName;
        public GameplayTagKey Key => new AssetKey(id);
        public GameplayTag ToRuntimeDefinition() => new GameplayTag(Id);
        private sealed class AssetKey : GameplayTagKey { public AssetKey(string value) : base(value) { } }
    }
}
