using System;
using Deucarian.Editor;
using UnityEditor;

namespace Deucarian.GameplayFoundation.Editor
{
    [CustomPropertyDrawer(typeof(StatKey), true)]
    public sealed class StatKeyDrawer : DeucarianKeyDrawer
    {
        public override Type KeyType => typeof(StatKey);
        public override Type DefinitionSetAttribute => typeof(StatKeySetAttribute);
        public override string SetupHint => "Select an existing StatKey; declare reusable keys once in a [StatKeySet] class.";
    }
}
