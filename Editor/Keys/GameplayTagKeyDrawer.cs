using System;
using Deucarian.Editor;
using UnityEditor;

namespace Deucarian.GameplayFoundation.Editor
{
    [CustomPropertyDrawer(typeof(GameplayTagKey), true)]
    public sealed class GameplayTagKeyDrawer : DeucarianKeyDrawer
    {
        public override Type KeyType => typeof(GameplayTagKey);
        public override Type DefinitionSetAttribute => typeof(GameplayTagKeySetAttribute);
        public override string SetupHint => "Select an existing GameplayTagKey; declare reusable keys once in a [GameplayTagKeySet] class.";
    }
}
