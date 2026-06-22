using System;

namespace Deucarian.GameplayFoundation
{
    /// <summary>
    /// Read-only clock abstraction for gameplay systems.
    /// </summary>
    public interface IGameClock
    {
        /// <summary>Gets elapsed time in seconds.</summary>
        double TimeSeconds { get; }
    }

    /// <summary>
    /// Manually advanced deterministic clock for tests and gameplay simulation.
    /// </summary>
    public sealed class ManualGameClock : IGameClock
    {
        /// <inheritdoc />
        public double TimeSeconds { get; private set; }

        /// <summary>Advances the clock.</summary>
        public void Advance(double deltaSeconds)
        {
            Numeric.GuardFinite(deltaSeconds, nameof(deltaSeconds));
            if (deltaSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), "Delta cannot be negative.");
            }

            TimeSeconds += deltaSeconds;
        }

        /// <summary>Resets the clock to zero or a supplied time.</summary>
        public void Reset(double timeSeconds = 0d)
        {
            Numeric.GuardFinite(timeSeconds, nameof(timeSeconds));
            if (timeSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(timeSeconds), "Time cannot be negative.");
            }

            TimeSeconds = timeSeconds;
        }
    }

    /// <summary>
    /// Deterministic fixed-step tick accumulator.
    /// </summary>
    public sealed class FixedTickStepper
    {
        private double _accumulatorSeconds;

        /// <summary>Creates a fixed tick stepper.</summary>
        public FixedTickStepper(double tickDurationSeconds)
        {
            Numeric.GuardFinite(tickDurationSeconds, nameof(tickDurationSeconds));
            if (tickDurationSeconds <= 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(tickDurationSeconds), "Tick duration must be greater than zero.");
            }

            TickDurationSeconds = tickDurationSeconds;
        }

        /// <summary>Gets the configured fixed tick duration.</summary>
        public double TickDurationSeconds { get; }

        /// <summary>Gets the total number of completed ticks since reset.</summary>
        public long TickIndex { get; private set; }

        /// <summary>Gets accumulated fractional time not yet converted to a tick.</summary>
        public double AccumulatorSeconds => _accumulatorSeconds;

        /// <summary>Advances the stepper and returns the number of whole ticks produced.</summary>
        public int Advance(double deltaSeconds)
        {
            Numeric.GuardFinite(deltaSeconds, nameof(deltaSeconds));
            if (deltaSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), "Delta cannot be negative.");
            }

            _accumulatorSeconds += deltaSeconds;
            int producedTicks = 0;
            while (_accumulatorSeconds + Numeric.Epsilon >= TickDurationSeconds)
            {
                _accumulatorSeconds -= TickDurationSeconds;
                TickIndex++;
                producedTicks++;
            }

            if (_accumulatorSeconds < Numeric.Epsilon)
            {
                _accumulatorSeconds = 0d;
            }

            return producedTicks;
        }

        /// <summary>Resets tick index and accumulated fractional time.</summary>
        public void Reset()
        {
            _accumulatorSeconds = 0d;
            TickIndex = 0L;
        }
    }

    /// <summary>
    /// Generic cooldown timer.
    /// </summary>
    public struct CooldownTimer
    {
        private double _remainingSeconds;

        /// <summary>Gets the remaining cooldown in seconds.</summary>
        public double RemainingSeconds => _remainingSeconds;

        /// <summary>Gets whether the cooldown is complete.</summary>
        public bool IsReady => _remainingSeconds <= 0d;

        /// <summary>Starts or restarts the cooldown.</summary>
        public void Start(double durationSeconds)
        {
            Numeric.GuardFinite(durationSeconds, nameof(durationSeconds));
            if (durationSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Duration cannot be negative.");
            }

            _remainingSeconds = durationSeconds;
        }

        /// <summary>Advances the cooldown.</summary>
        public void Advance(double deltaSeconds)
        {
            Numeric.GuardFinite(deltaSeconds, nameof(deltaSeconds));
            if (deltaSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), "Delta cannot be negative.");
            }

            _remainingSeconds = Math.Max(0d, _remainingSeconds - deltaSeconds);
        }

        /// <summary>Clears the cooldown.</summary>
        public void Reset()
        {
            _remainingSeconds = 0d;
        }
    }

    /// <summary>
    /// Generic duration timer.
    /// </summary>
    public struct DurationTimer
    {
        private double _remainingSeconds;

        /// <summary>Gets the remaining duration in seconds.</summary>
        public double RemainingSeconds => _remainingSeconds;

        /// <summary>Gets whether the duration has expired.</summary>
        public bool IsExpired => _remainingSeconds <= 0d;

        /// <summary>Starts or restarts the duration.</summary>
        public void Start(double durationSeconds)
        {
            Numeric.GuardFinite(durationSeconds, nameof(durationSeconds));
            if (durationSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Duration cannot be negative.");
            }

            _remainingSeconds = durationSeconds;
        }

        /// <summary>Advances the duration.</summary>
        public void Advance(double deltaSeconds)
        {
            Numeric.GuardFinite(deltaSeconds, nameof(deltaSeconds));
            if (deltaSeconds < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), "Delta cannot be negative.");
            }

            _remainingSeconds = Math.Max(0d, _remainingSeconds - deltaSeconds);
        }

        /// <summary>Clears the duration.</summary>
        public void Reset()
        {
            _remainingSeconds = 0d;
        }
    }
}
