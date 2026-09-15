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
    public sealed class GameplayTagDefinitionSpec : DeucarianDefinitionSpec
    {
        [DefinitionField("description")] public string Description = string.Empty;
    }
}
