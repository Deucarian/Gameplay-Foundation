using System;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Unity
{
    public sealed class GameplayDefinitionCatalog : ScriptableObject
    {
        public const string ResourcePath = "Deucarian/Definitions/GameplayDefinitionCatalog";
        [SerializeField] private StatDefinitionAsset[] stats = Array.Empty<StatDefinitionAsset>();
        [SerializeField] private GameplayTagDefinitionAsset[] tags = Array.Empty<GameplayTagDefinitionAsset>();
        public static GameplayDefinitionCatalog LoadProject() => Resources.Load<GameplayDefinitionCatalog>(ResourcePath) ?? throw new InvalidOperationException("Create a stat or gameplay tag in Definitions before loading the project catalog.");
        public StatBlock CreateStats()
        {
            var result = new StatBlock();
            var ids = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            foreach (var stat in stats)
            {
                if (stat == null || !ids.Add(stat.Id)) throw new InvalidOperationException("The stat catalog contains a missing or duplicate definition. Synchronize it in Definitions.");
                stat.ApplyTo(result);
            }
            return result;
        }
        public GameplayTag[] CreateTags()
        {
            var result = new GameplayTag[tags.Length];
            for (int i = 0; i < result.Length; i++) result[i] = tags[i] != null ? tags[i].ToRuntimeDefinition() : throw new InvalidOperationException("Select a definition for every gameplay tag in the catalog.");
            return result;
        }
    }
}
