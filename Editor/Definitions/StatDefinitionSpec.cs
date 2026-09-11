using System;
using System.Linq;
using Deucarian.Editor;
using Deucarian.Editor.Definitions;
using Deucarian.GameplayFoundation.Unity;
using UnityEditor;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Editor.Definitions
{
    [Serializable]
    public sealed class StatDefinitionSpec : DeucarianDefinitionSpec
    {
        [DefinitionField("baseValue")] public double BaseValue = 0d;
    }
}
