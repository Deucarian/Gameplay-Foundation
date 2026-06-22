using System;

namespace Deucarian.GameplayFoundation
{
    /// <summary>
    /// Deterministic random source abstraction for gameplay rules.
    /// </summary>
    public interface IRandomSource
    {
        /// <summary>Returns the next unsigned 32-bit value.</summary>
        uint NextUInt();

        /// <summary>Returns a value in the range [0, 1).</summary>
        double NextDouble();

        /// <summary>Returns an integer in the range [minInclusive, maxExclusive).</summary>
        int Range(int minInclusive, int maxExclusive);

        /// <summary>Returns a double in the range [minInclusive, maxInclusive).</summary>
        double Range(double minInclusive, double maxInclusive);
    }

    /// <summary>
    /// Seeded deterministic random source based on xoshiro128**.
    /// </summary>
    public sealed class DeterministicRandom : IRandomSource
    {
        private uint _a;
        private uint _b;
        private uint _c;
        private uint _d;

        /// <summary>Creates a deterministic random source from a 32-bit seed.</summary>
        public DeterministicRandom(int seed)
            : this(unchecked((uint)seed))
        {
        }

        /// <summary>Creates a deterministic random source from a 32-bit seed.</summary>
        public DeterministicRandom(uint seed)
        {
            Reset(seed);
        }

        /// <summary>Resets the source to the supplied seed.</summary>
        public void Reset(uint seed)
        {
            SplitMix32 mixer = new SplitMix32(seed == 0u ? 0x9E3779B9u : seed);
            _a = mixer.Next();
            _b = mixer.Next();
            _c = mixer.Next();
            _d = mixer.Next();
            if ((_a | _b | _c | _d) == 0u)
            {
                _a = 1u;
            }
        }

        /// <inheritdoc />
        public uint NextUInt()
        {
            uint result = RotateLeft(_b * 5u, 7) * 9u;
            uint t = _b << 9;
            _c ^= _a;
            _d ^= _b;
            _b ^= _c;
            _a ^= _d;
            _c ^= t;
            _d = RotateLeft(_d, 11);
            return result;
        }

        /// <inheritdoc />
        public double NextDouble()
        {
            return NextUInt() / 4294967296.0;
        }

        /// <inheritdoc />
        public int Range(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(maxExclusive), "Maximum must be greater than minimum.");
            }

            uint span = (uint)(maxExclusive - minInclusive);
            return minInclusive + (int)(NextUInt() % span);
        }

        /// <inheritdoc />
        public double Range(double minInclusive, double maxInclusive)
        {
            Numeric.GuardFinite(minInclusive, nameof(minInclusive));
            Numeric.GuardFinite(maxInclusive, nameof(maxInclusive));
            if (maxInclusive <= minInclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(maxInclusive), "Maximum must be greater than minimum.");
            }

            return minInclusive + ((maxInclusive - minInclusive) * NextDouble());
        }

        private static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        private struct SplitMix32
        {
            private uint _state;

            public SplitMix32(uint seed)
            {
                _state = seed;
            }

            public uint Next()
            {
                uint z = (_state += 0x9E3779B9u);
                z = (z ^ (z >> 16)) * 0x85EBCA6Bu;
                z = (z ^ (z >> 13)) * 0xC2B2AE35u;
                return z ^ (z >> 16);
            }
        }
    }
}
