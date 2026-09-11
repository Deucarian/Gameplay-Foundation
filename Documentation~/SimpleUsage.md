# Simple usage

Copy the reference example into your project, add SimpleUsageExample and assign its scoped host references. Its serialized definition fields use the same typed keys as code.

Pass the player's existing StatBlock once to Configure. Initialize the stat once with `stats.SetBaseValue(Stats.MoveSpeed, 5)`. An uninitialized base value retains StatBlock's documented zero default; there is no separate stat catalog. Use `Tags.Flying.ToGameplayTag()` with the existing tag collection. The pure core retains stat evaluation; Unity keys live in a separate adapter assembly.

Definitions are authored once in SampleDefinitions.cs where applicable; the caller never invents an ID. Replace the sample set with your project's central definitions. A selected key proves its identity and payload type; startup still needs to bind that definition in the correct scope. Missing configuration reports how to fix it. Dynamic targets and choices are issued by their owner instead of selected from a definition dropdown.
```csharp
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
```
