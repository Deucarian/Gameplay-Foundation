using System;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Unity
{
    /// <summary>One object's stat owner. Each host receives an independent block from reusable definitions.</summary>
    [DefaultExecutionOrder(-1000), DisallowMultipleComponent, AddComponentMenu("Deucarian/Gameplay/Stats Host")]
    public sealed class StatsHost : MonoBehaviour
    {
        [SerializeField] private GameplayDefinitionCatalog definitions;
        private StatBlock block;
        public StatBlock Block => block ?? throw new InvalidOperationException("Enable StatsHost '" + name + "' or configure its StatBlock before use.");
        public void Configure(StatBlock value)
        {
            if (block != null) throw new InvalidOperationException("This StatsHost already has a stat block.");
            block = value ?? throw new ArgumentNullException(nameof(value));
        }
        private void Awake() { if (block == null) block = (definitions != null ? definitions : GameplayDefinitionCatalog.LoadProject()).CreateStats(); }
        public double Get(StatKey stat) { Require(stat); return Block.GetValue(stat); }
        public void Set(StatKey stat, double value)
        {
            Require(stat);
            Block.SetBaseValue(new StatId(stat.Id), value);
        }
        private void Require(StatKey stat)
        {
            if (stat == null) throw new ArgumentNullException(nameof(stat), "Select an existing stat definition.");
            if (!Block.Contains(new StatId(stat.Id))) throw new InvalidOperationException("StatsHost '" + name + "' has no stat '" + stat.Id + "'. Add it to this host's definitions or supplied StatBlock.");
        }
    }
}
