using System;
using System.Collections.Generic;

namespace Deucarian.GameplayFoundation
{
    /// <summary>
    /// Operation applied by a stat modifier.
    /// </summary>
    public enum StatModifierOperation
    {
        /// <summary>Adds a value to the base stat.</summary>
        Additive = 0,

        /// <summary>Multiplies the current stat by the modifier value.</summary>
        Multiplicative = 1,

        /// <summary>Overrides the current stat value.</summary>
        Override = 2,

        /// <summary>Raises the current stat to at least the modifier value.</summary>
        Minimum = 3,

        /// <summary>Lowers the current stat to at most the modifier value.</summary>
        Maximum = 4
    }

    /// <summary>
    /// Stable unique handle for a modifier.
    /// </summary>
    public readonly struct StatModifierHandle : IEquatable<StatModifierHandle>
    {
        private readonly string _value;

        /// <summary>Creates a modifier handle.</summary>
        public StatModifierHandle(string value)
        {
            IdentifierRules.ThrowIfInvalid(value, nameof(value));
            _value = value;
        }

        /// <summary>Gets the handle value.</summary>
        public string Value => _value ?? string.Empty;

        /// <inheritdoc />
        public bool Equals(StatModifierHandle other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is StatModifierHandle other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Stable source handle used to remove related modifiers together.
    /// </summary>
    public readonly struct ModifierSourceHandle : IEquatable<ModifierSourceHandle>
    {
        private readonly string _value;

        /// <summary>Creates a source handle.</summary>
        public ModifierSourceHandle(string value)
        {
            IdentifierRules.ThrowIfInvalid(value, nameof(value));
            _value = value;
        }

        /// <summary>Gets the source value.</summary>
        public string Value => _value ?? string.Empty;

        /// <inheritdoc />
        public bool Equals(ModifierSourceHandle other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is ModifierSourceHandle other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Immutable stat modifier definition.
    /// </summary>
    public readonly struct StatModifier
    {
        /// <summary>Creates a stat modifier.</summary>
        public StatModifier(
            StatModifierHandle handle,
            ModifierSourceHandle source,
            StatId statId,
            StatModifierOperation operation,
            double value,
            int priority = 0)
        {
            if (statId.IsEmpty)
            {
                throw new ArgumentException("Stat id cannot be empty.", nameof(statId));
            }

            Numeric.GuardFinite(value, nameof(value));
            Handle = handle;
            Source = source;
            StatId = statId;
            Operation = operation;
            Value = value;
            Priority = priority;
        }

        /// <summary>Gets the unique modifier handle.</summary>
        public StatModifierHandle Handle { get; }

        /// <summary>Gets the source handle.</summary>
        public ModifierSourceHandle Source { get; }

        /// <summary>Gets the affected stat.</summary>
        public StatId StatId { get; }

        /// <summary>Gets the operation.</summary>
        public StatModifierOperation Operation { get; }

        /// <summary>Gets the modifier value.</summary>
        public double Value { get; }

        /// <summary>Gets deterministic ordering priority within the operation phase.</summary>
        public int Priority { get; }
    }

    /// <summary>
    /// Mutable stat block with deterministic modifier evaluation.
    /// </summary>
    public sealed class StatBlock
    {
        private readonly Dictionary<StatId, double> _baseValues = new Dictionary<StatId, double>();
        private readonly Dictionary<StatModifierHandle, ModifierEntry> _modifiersByHandle = new Dictionary<StatModifierHandle, ModifierEntry>();
        private readonly List<ModifierEntry> _modifiers = new List<ModifierEntry>();
        private ulong _nextSequence;

        /// <summary>Whether this block contains a base value or modifier for the stat.</summary>
        public bool Contains(StatId statId)
        {
            if (_baseValues.ContainsKey(statId)) return true;
            foreach (var entry in _modifiers) if (entry.Modifier.StatId.Equals(statId)) return true;
            return false;
        }

        /// <summary>Sets the base value for a stat.</summary>
        public void SetBaseValue(StatId statId, double value)
        {
            if (statId.IsEmpty)
            {
                throw new ArgumentException("Stat id cannot be empty.", nameof(statId));
            }

            Numeric.GuardFinite(value, nameof(value));
            _baseValues[statId] = value;
        }

        /// <summary>Gets a base value or zero when the stat has no base value.</summary>
        public double GetBaseValue(StatId statId)
        {
            return _baseValues.TryGetValue(statId, out double value) ? value : 0d;
        }

        /// <summary>Adds a modifier, throwing if the handle already exists.</summary>
        public void AddModifier(StatModifier modifier)
        {
            if (!TryAddModifier(modifier))
            {
                throw new InvalidOperationException($"Modifier handle '{modifier.Handle}' already exists.");
            }
        }

        /// <summary>Attempts to add a modifier.</summary>
        public bool TryAddModifier(StatModifier modifier)
        {
            if (modifier.StatId.IsEmpty)
            {
                throw new ArgumentException("Modifier stat id cannot be empty.", nameof(modifier));
            }

            if (_modifiersByHandle.ContainsKey(modifier.Handle))
            {
                return false;
            }

            ModifierEntry entry = new ModifierEntry(modifier, _nextSequence++);
            _modifiersByHandle.Add(modifier.Handle, entry);
            _modifiers.Add(entry);
            return true;
        }

        /// <summary>Removes one modifier by handle.</summary>
        public bool RemoveModifier(StatModifierHandle handle)
        {
            if (!_modifiersByHandle.TryGetValue(handle, out ModifierEntry entry))
            {
                return false;
            }

            _modifiersByHandle.Remove(handle);
            _modifiers.Remove(entry);
            return true;
        }

        /// <summary>Removes all modifiers from a source and returns the number removed.</summary>
        public int RemoveModifiersFromSource(ModifierSourceHandle source)
        {
            int removed = 0;
            for (int index = _modifiers.Count - 1; index >= 0; index--)
            {
                ModifierEntry entry = _modifiers[index];
                if (!entry.Modifier.Source.Equals(source))
                {
                    continue;
                }

                _modifiers.RemoveAt(index);
                _modifiersByHandle.Remove(entry.Modifier.Handle);
                removed++;
            }

            return removed;
        }

        /// <summary>Gets the evaluated value for a stat.</summary>
        public double GetValue(StatId statId)
        {
            double value = GetBaseValue(statId);
            value = ApplyOperation(statId, value, StatModifierOperation.Additive);
            value = ApplyOperation(statId, value, StatModifierOperation.Multiplicative);
            value = ApplyOperation(statId, value, StatModifierOperation.Override);
            value = ApplyOperation(statId, value, StatModifierOperation.Minimum);
            value = ApplyOperation(statId, value, StatModifierOperation.Maximum);
            return value;
        }

        /// <summary>Creates an immutable snapshot of all known stat values.</summary>
        public StatSnapshot CreateSnapshot()
        {
            Dictionary<StatId, double> values = new Dictionary<StatId, double>(_baseValues.Count);
            foreach (StatId statId in _baseValues.Keys)
            {
                values[statId] = GetValue(statId);
            }

            for (int index = 0; index < _modifiers.Count; index++)
            {
                StatId statId = _modifiers[index].Modifier.StatId;
                if (!values.ContainsKey(statId))
                {
                    values[statId] = GetValue(statId);
                }
            }

            return new StatSnapshot(values);
        }

        private double ApplyOperation(StatId statId, double currentValue, StatModifierOperation operation)
        {
            double value = currentValue;
            bool hasPrevious = false;
            int previousPriority = 0;
            ulong previousSequence = 0UL;

            while (TryResolveNextEntry(statId, operation, hasPrevious, previousPriority, previousSequence, out ModifierEntry entry))
            {
                double modifierValue = entry.Modifier.Value;
                switch (operation)
                {
                    case StatModifierOperation.Additive:
                        value += modifierValue;
                        break;
                    case StatModifierOperation.Multiplicative:
                        value *= modifierValue;
                        break;
                    case StatModifierOperation.Override:
                        value = modifierValue;
                        break;
                    case StatModifierOperation.Minimum:
                        value = Math.Max(value, modifierValue);
                        break;
                    case StatModifierOperation.Maximum:
                        value = Math.Min(value, modifierValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, "Unsupported modifier operation.");
                }

                hasPrevious = true;
                previousPriority = entry.Modifier.Priority;
                previousSequence = entry.Sequence;
            }

            return value;
        }

        private bool TryResolveNextEntry(
            StatId statId,
            StatModifierOperation operation,
            bool hasPrevious,
            int previousPriority,
            ulong previousSequence,
            out ModifierEntry entry)
        {
            bool hasBest = false;
            ModifierEntry best = default;
            for (int index = 0; index < _modifiers.Count; index++)
            {
                ModifierEntry candidate = _modifiers[index];
                if (!candidate.Modifier.StatId.Equals(statId) || candidate.Modifier.Operation != operation)
                {
                    continue;
                }

                if (hasPrevious &&
                    (candidate.Modifier.Priority < previousPriority ||
                     candidate.Modifier.Priority == previousPriority && candidate.Sequence <= previousSequence))
                {
                    continue;
                }

                if (!hasBest ||
                    candidate.Modifier.Priority < best.Modifier.Priority ||
                    candidate.Modifier.Priority == best.Modifier.Priority && candidate.Sequence < best.Sequence)
                {
                    best = candidate;
                    hasBest = true;
                }
            }

            entry = best;
            return hasBest;
        }

        private readonly struct ModifierEntry : IEquatable<ModifierEntry>
        {
            public ModifierEntry(StatModifier modifier, ulong sequence)
            {
                Modifier = modifier;
                Sequence = sequence;
            }

            public StatModifier Modifier { get; }

            public ulong Sequence { get; }

            public bool Equals(ModifierEntry other)
            {
                return Modifier.Handle.Equals(other.Modifier.Handle);
            }
        }
    }

    /// <summary>
    /// Immutable evaluated stat snapshot.
    /// </summary>
    public sealed class StatSnapshot
    {
        private readonly Dictionary<StatId, double> _values;

        internal StatSnapshot(Dictionary<StatId, double> values)
        {
            _values = values ?? new Dictionary<StatId, double>();
        }

        /// <summary>Gets the number of captured stat values.</summary>
        public int Count => _values.Count;

        /// <summary>Attempts to get a captured value.</summary>
        public bool TryGetValue(StatId statId, out double value)
        {
            return _values.TryGetValue(statId, out value);
        }

        /// <summary>Gets a captured value or zero when the stat is absent.</summary>
        public double GetValueOrDefault(StatId statId)
        {
            return _values.TryGetValue(statId, out double value) ? value : 0d;
        }
    }

    internal static class Numeric
    {
        public const double Epsilon = 0.0000000001d;

        public static void GuardFinite(double value, string paramName)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(paramName, "Value must be finite.");
            }
        }
    }
}
