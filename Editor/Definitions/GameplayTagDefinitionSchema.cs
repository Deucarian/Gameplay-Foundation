using System;
using System.Linq;
using Deucarian.Editor;
using Deucarian.Editor.Definitions;
using Deucarian.GameplayFoundation.Unity;
using UnityEditor;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Editor.Definitions
{
    public sealed class GameplayTagDefinitionSchema : DeucarianSerializedDefinitionSchema<GameplayTagDefinitionAsset, GameplayTagDefinitionSpec>
    {
        public override string Id => "tags";
        public override string DisplayName => "Gameplay tags";
        public override void ValidateAssetReady(ScriptableObject asset)
        {
            base.ValidateAssetReady(asset);
            ((GameplayTagDefinitionAsset)asset).ToRuntimeDefinition();
        }
        public override void RefreshCatalog(bool validateOnly = false) { ProjectDefinitionCatalogAuthoring.Refresh(validateOnly); }
        [MenuItem("Assets/Create/Deucarian/Gameplay Foundation/GameplayTag Definition")]
        private static void CreateDefinition() { Selection.activeObject = DeucarianDefinitionSync.Create(new GameplayTagDefinitionSchema(), "NewGameplayTag"); }
    }
}
