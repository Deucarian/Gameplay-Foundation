# Gameplay Foundation API

## Identifiers

`ContentId`, `GameplayTag`, and `StatId` are strongly typed wrappers around stable string identifiers. They accept lowercase letters, digits, `.`, `-`, `_`, and `/`.

Use `ContentId` for authored or generated content, `GameplayTag` for classification, and `StatId` for stat definitions.

## Random

`IRandomSource` is the deterministic random contract. `DeterministicRandom` is a seeded implementation intended for tests and simulation rules.

## Time And Ticks

`ManualGameClock` is a deterministic clock controlled by callers. `FixedTickStepper` turns variable elapsed time into fixed ticks and exposes total tick index plus fractional accumulator.

`CooldownTimer` and `DurationTimer` are simple value-type timers for generic gameplay timing.

## Stats

`StatBlock` stores base values and modifiers. Modifiers have:

- a unique `StatModifierHandle`
- a grouping `ModifierSourceHandle`
- a `StatId`
- an operation
- a finite numeric value
- a priority

Duplicate modifier handles are rejected. Source handles allow safe batch removal.

`StatSnapshot` captures evaluated stat values at one point in time.
