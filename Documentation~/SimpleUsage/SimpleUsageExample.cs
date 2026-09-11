using UnityEngine;
using System;
namespace Deucarian.GameplayFoundation.Samples.SimpleUsage
{
    public sealed class SimpleUsageExample : MonoBehaviour
    {
        [SerializeField] private StatKey speed = Stats.MoveSpeed;
        private StatBlock stats;
        public void Configure(StatBlock value) => stats = value ?? throw new ArgumentNullException(nameof(value));
        public double Speed => (stats ?? throw new InvalidOperationException("Configure this example with the player StatBlock first.")).GetValue(speed);
    }
}
