using System;
using UnityEngine;

namespace Deucarian.GameplayFoundation.Unity
{
    [AddComponentMenu("Deucarian/Gameplay/Stat Value Trigger")]
    public sealed class StatValueTrigger : MonoBehaviour
    {
        [SerializeField] private StatsHost host;
        [SerializeField] private StatKey stat;
        [SerializeField] private double value;
        public void Apply()
        {
            if (host == null) throw new InvalidOperationException("Assign a StatsHost to StatValueTrigger '" + name + "'.");
            host.Set(stat, value);
        }
    }
}
