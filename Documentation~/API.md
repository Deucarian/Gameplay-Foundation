# Gameplay Foundation API

## Identifiers

`ContentId`, `GameplayTag`, and `StatId` are strongly typed wrappers around stable string identifiers. They accept lowercase letters, digits, `.`, `-`, `_`, and `/`.

Use `ContentId` for authored or generated content, `GameplayTag` for classification, and `StatId` for stat definitions.

## Content Validation

`ContentValidationReport` collects `ContentValidationIssue` values with `Info`, `Warning`, or `Error` severity. `Succeeded` and `IsValid` are true when the report contains no errors.

`ContentReferenceSet` stores known stable IDs for reference checks. `ContentValidation` contains small helpers for unique IDs, required IDs, required references, known references, and simple numeric bounds.

The helpers are intentionally domain-neutral. Callers supply labels and keep gameplay-specific rules in their own packages or templates.

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
