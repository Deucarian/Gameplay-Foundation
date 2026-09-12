using System;
using System.Linq;
using Deucarian.Editor;
using Deucarian.Editor.Definitions;
using Deucarian.GameplayFoundation.Unity;
using UnityEditor;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Editor.Definitions
{
    public sealed class StatDefinitionSchema : DeucarianSerializedDefinitionSchema<StatDefinitionAsset, StatDefinitionSpec>
    {
        public override string Id => "stats";
        public override string DisplayName => "Stats";
        public override void ValidateAssetReady(ScriptableObject asset)
        {
            base.ValidateAssetReady(asset);
            var block = new StatBlock(); ((StatDefinitionAsset)asset).ApplyTo(block);
        }
        public override void RefreshCatalog(bool validateOnly = false) { ProjectDefinitionCatalogAuthoring.Refresh(validateOnly); }
        [MenuItem("Assets/Create/Deucarian/Gameplay Foundation/Stat Definition")]
        private static void CreateDefinition() { Selection.activeObject = DeucarianDefinitionSync.Create(new StatDefinitionSchema(), "NewStat"); }
    }
}
