# Deucarian Gameplay Foundation

## Overview

Gameplay Foundation is a small, standalone runtime package for pure gameplay primitives:

- stable content identifiers
- runtime-safe content validation reports and ID/reference helpers
- lightweight gameplay tags
- deterministic random sources
- manual clocks and fixed ticks
- generic cooldown and duration timers
- stats, modifiers, modifier source handles, safe removal, and snapshots

Package ID: `com.deucarian.gameplay-foundation`

This package intentionally does not include health, damage, weapons, progression, saving, encounters, UI, networking, GameObjects, MonoBehaviours, service locators, Entities, or global mutable runtime state.

## Installation

For local development, reference this package by file path from a Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.deucarian.gameplay-foundation": "file:C:/Repositories/Deucarian/Gameplay-Foundation"
  }
}
```

Remote publication is a later release step. No remote URL is declared for this Phase 1A local package.

## Public API

- `ContentId`, `GameplayTag`, `StatId`
- `ContentValidationReport`, `ContentValidationIssue`, `ContentValidationSeverity`
- `ContentReferenceSet`, `ContentValidation`
- `IRandomSource`, `DeterministicRandom`
- `IGameClock`, `ManualGameClock`, `FixedTickStepper`
- `CooldownTimer`, `DurationTimer`
- `StatModifierOperation`
- `StatModifierHandle`, `ModifierSourceHandle`, `StatModifier`
- `StatBlock`, `StatSnapshot`

## Content Validation

`ContentValidationReport` and `ContentValidation` provide small runtime-safe helpers for recurring authored-content checks:

- unique stable IDs
- required IDs
- known reference sets
- required references
- simple numeric bounds

Domain-specific validation stays with the package or template that owns the content. Gameplay Foundation does not know about weapons, enemies, movement, rewards, save data, JSON, ScriptableObjects, or editor UI.

## Modifier Evaluation

Stats are evaluated in deterministic phases:

1. additive
2. multiplicative
3. override
4. minimum
5. maximum

Within each phase, modifiers are ordered by ascending priority and then insertion order. Later override modifiers in that deterministic order replace earlier override values. Minimum and maximum modifiers act as clamps after override.

## Samples

`Samples~/StatModifierPrimer` contains a tiny pure C# example that can be imported into a test project.

## Compatibility

The runtime assembly has `noEngineReferences` enabled and targets Unity `2021.3` or newer. Phase 1A validation uses Unity `6000.3.5f1`, matching the Phase 0 donor/editor decision.
