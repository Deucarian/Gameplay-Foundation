using System;
using System.Linq;
using Deucarian.Editor;
using Deucarian.Editor.Definitions;
using Deucarian.GameplayFoundation.Unity;
using UnityEditor;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Editor.Definitions
{
    public sealed class GameplayTagKeySource : DeucarianAssetKeySource<GameplayTagDefinitionAsset>
    {
        public override Type KeyType => typeof(GameplayTagKey);
        public override Type DefinitionSetAttribute => typeof(GameplayTagKeySetAttribute);
        public override string GeneratedClassName => "ProjectTags";
        protected override DeucarianKeyChoice ReadDefinition(GameplayTagDefinitionAsset asset) => new DeucarianKeyChoice(asset.Id, asset.DisplayName);
    }
}
