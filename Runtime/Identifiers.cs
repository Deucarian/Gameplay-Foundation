using System;

namespace Deucarian.GameplayFoundation
{
    /// <summary>
    /// Stable identifier for authored or generated gameplay content.
    /// </summary>
    public readonly struct ContentId : IEquatable<ContentId>, IComparable<ContentId>
    {
        /// <summary>Represents an unset content identifier.</summary>
        public static readonly ContentId Empty = new ContentId(string.Empty, false);

        private readonly string _value;

        /// <summary>
        /// Creates a content identifier after validating that the value is non-empty and stable-id safe.
        /// </summary>
        /// <param name="value">Identifier text, such as <c>weapon.arc-bolt</c>.</param>
        /// <exception cref="ArgumentException">Thrown when the value is empty or contains unsupported characters.</exception>
        public ContentId(string value)
            : this(value, true)
        {
        }

        private ContentId(string value, bool validate)
        {
            if (validate)
            {
                IdentifierRules.ThrowIfInvalid(value, nameof(value));
            }

            _value = value ?? string.Empty;
        }

        /// <summary>Gets the identifier value.</summary>
        public string Value => _value ?? string.Empty;

        /// <summary>Gets whether this identifier has a non-empty value.</summary>
        public bool IsEmpty => string.IsNullOrEmpty(Value);

        /// <inheritdoc />
        public bool Equals(ContentId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is ContentId other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        /// <inheritdoc />
        public int CompareTo(ContentId other)
        {
            return string.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }

        /// <summary>Compares two identifiers for equality.</summary>
        public static bool operator ==(ContentId left, ContentId right)
        {
            return left.Equals(right);
        }

        /// <summary>Compares two identifiers for inequality.</summary>
        public static bool operator !=(ContentId left, ContentId right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Lightweight tag identifier for gameplay classification.
    /// </summary>
    public readonly struct GameplayTag : IEquatable<GameplayTag>, IComparable<GameplayTag>
    {
        /// <summary>Represents an unset tag.</summary>
        public static readonly GameplayTag Empty = new GameplayTag(string.Empty, false);

        private readonly string _value;

        /// <summary>Creates a gameplay tag.</summary>
        public GameplayTag(string value)
            : this(value, true)
        {
        }

        private GameplayTag(string value, bool validate)
        {
            if (validate)
            {
                IdentifierRules.ThrowIfInvalid(value, nameof(value));
            }

            _value = value ?? string.Empty;
        }

        /// <summary>Gets the tag value.</summary>
        public string Value => _value ?? string.Empty;

        /// <summary>Gets whether this tag has a non-empty value.</summary>
        public bool IsEmpty => string.IsNullOrEmpty(Value);

        /// <inheritdoc />
        public bool Equals(GameplayTag other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is GameplayTag other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        /// <inheritdoc />
        public int CompareTo(GameplayTag other)
        {
            return string.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Stable identifier for a stat definition.
    /// </summary>
    public readonly struct StatId : IEquatable<StatId>, IComparable<StatId>
    {
        /// <summary>Represents an unset stat identifier.</summary>
        public static readonly StatId Empty = new StatId(string.Empty, false);

        private readonly string _value;

        /// <summary>Creates a stat identifier.</summary>
        public StatId(string value)
            : this(value, true)
        {
        }

        private StatId(string value, bool validate)
        {
            if (validate)
            {
                IdentifierRules.ThrowIfInvalid(value, nameof(value));
            }

            _value = value ?? string.Empty;
        }

        /// <summary>Gets the stat identifier value.</summary>
        public string Value => _value ?? string.Empty;

        /// <summary>Gets whether this stat identifier is empty.</summary>
        public bool IsEmpty => string.IsNullOrEmpty(Value);

        /// <inheritdoc />
        public bool Equals(StatId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is StatId other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        /// <inheritdoc />
        public int CompareTo(StatId other)
        {
            return string.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }

    internal static class IdentifierRules
    {
        public static void ThrowIfInvalid(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Identifier cannot be empty.", paramName);
            }

            string trimmed = value.Trim();
            if (!string.Equals(value, trimmed, StringComparison.Ordinal))
            {
                throw new ArgumentException("Identifier cannot contain leading or trailing whitespace.", paramName);
            }

            for (int index = 0; index < value.Length; index++)
            {
                char character = value[index];
                bool valid =
                    character >= 'a' && character <= 'z' ||
                    character >= '0' && character <= '9' ||
                    character == '.' ||
                    character == '-' ||
                    character == '_' ||
                    character == '/';

                if (!valid)
                {
                    throw new ArgumentException("Identifier can contain only lowercase letters, digits, '.', '-', '_' or '/'.", paramName);
                }
            }
        }
    }
}
