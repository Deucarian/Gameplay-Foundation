using System;
using UnityEngine;
using Deucarian.GameplayFoundation;
namespace Deucarian.GameplayFoundation.Unity.Samples.DefinitionWorkflow
{
    /// <summary>Small caller example. The configured scene hosts own services and resource lifetimes.</summary>
    public sealed class GameplayFoundationWorkflow : MonoBehaviour
    {
        [SerializeField] private StatsHost host;
        [SerializeField] private StatKey stat;
        private string status = "Ready. Choose an action below.";
        public string Status => status;
        public void Read() { status = "Current value: " + host.Get(stat); }
        public void Increase() { host.Set(stat, host.Get(stat) + 1); Read(); }
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(24, 24, Math.Min(540, Screen.width - 48), Screen.height - 48), GUI.skin.box);
            GUILayout.Label("Gameplay-Foundation — definition workflow");
            GUILayout.Label("The definition provides the initial value. Each StatsHost owns independent runtime values; changing this actor does not change the asset.");
            GUILayout.Space(12);
            if (GUILayout.Button("Read stat", GUILayout.Height(32))) { try { Read(); } catch (Exception error) { status = error.Message; } }
            if (GUILayout.Button("Increase stat", GUILayout.Height(32))) { try { Increase(); } catch (Exception error) { status = error.Message; } }
            GUILayout.Space(12);
            GUILayout.Label(status);
            GUILayout.EndArea();
        }
    }
}
