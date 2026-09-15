using System;
using System.Linq;
using Deucarian.Editor;
using Deucarian.Editor.Definitions;
using Deucarian.GameplayFoundation.Unity;
using UnityEditor;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Editor.Definitions
{
    public sealed class StatKeySource : DeucarianAssetKeySource<StatDefinitionAsset>
    {
        public override Type KeyType => typeof(StatKey);
        public override Type DefinitionSetAttribute => typeof(StatKeySetAttribute);
        public override string GeneratedClassName => "ProjectStats";
        protected override DeucarianKeyChoice ReadDefinition(StatDefinitionAsset asset) => new DeucarianKeyChoice(asset.Id, asset.DisplayName);
    }
}
