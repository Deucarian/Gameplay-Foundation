using System;
using System.Linq;
using Deucarian.Editor.Definitions;
using Deucarian.GameplayFoundation.Unity;
using UnityEditor;

namespace Deucarian.GameplayFoundation.Editor.Definitions
{
    internal static class ProjectDefinitionCatalogAuthoring
    {
        internal static void Refresh(bool validateOnly = false)
        {
            var stats = AssetDatabase.FindAssets("t:StatDefinitionAsset", new[] { "Assets" })
                .Select(x => AssetDatabase.LoadAssetAtPath<StatDefinitionAsset>(AssetDatabase.GUIDToAssetPath(x)))
                .Where(x => x != null).OrderBy(x => x.Id, StringComparer.Ordinal).ToArray();
            if (stats.GroupBy(x => x.Id).Any(x => x.Count() > 1)) throw new InvalidOperationException("Stat definition IDs must be unique.");
            DeucarianDefinitionCatalog.Update<GameplayDefinitionCatalog>("Assets/DeucarianDefinitions/Resources/Deucarian/Definitions/GameplayDefinitionCatalog.asset", "stats", stats, validateOnly);
            var tags = AssetDatabase.FindAssets("t:GameplayTagDefinitionAsset", new[] { "Assets" })
                .Select(x => AssetDatabase.LoadAssetAtPath<GameplayTagDefinitionAsset>(AssetDatabase.GUIDToAssetPath(x)))
                .Where(x => x != null).OrderBy(x => x.Id, StringComparer.Ordinal).ToArray();
            if (tags.GroupBy(x => x.Id).Any(x => x.Count() > 1)) throw new InvalidOperationException("GameplayTag definition IDs must be unique.");
            DeucarianDefinitionCatalog.Update<GameplayDefinitionCatalog>("Assets/DeucarianDefinitions/Resources/Deucarian/Definitions/GameplayDefinitionCatalog.asset", "tags", tags, validateOnly);
        }
    }
}
