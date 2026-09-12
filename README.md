# Deucarian Gameplay Foundation

## Typed definition workflow

The definition provides the initial value. Each StatsHost owns independent runtime values; changing this actor does not change the asset.

Start with the [Definition Workflow walkthrough](Documentation~/DefinitionWorkflow.md).
Import **Definition Workflow** in Package Manager for a configured sample scene
and short caller scripts. Definitions can be edited as assets or editable C# declarations; generated keys
work in code and Inspector dropdowns.


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

## Install

Stable:

```json
"com.deucarian.gameplay-foundation": "https://github.com/Deucarian/Gameplay-Foundation.git#main"
```

Development:

```json
"com.deucarian.gameplay-foundation": "https://github.com/Deucarian/Gameplay-Foundation.git#develop"
```

Use `#main` for stable package consumption and `#develop` when testing active package work.

## When To Use This

Use this package when you need Pure C# gameplay identifiers, deterministic random, clocks, ticks, tags, stats, modifiers, and generic timing primitives.

Do not use this package to take ownership of capabilities outside its `AGENTS.md` boundary. Reusable behavior should stay with the package that owns that capability in the Package Registry governance docs.

## Quick Start

1. Install the package through Deucarian Package Installer or Unity Package Manager using the URL above.
2. Let Unity finish resolving packages and compiling assemblies.
3. Import the `Stat Modifier Primer` sample if you want a working reference scene or setup.
4. Start from the package README sections above and the public runtime/editor APIs in this repository.

## Troubleshooting

- Package does not resolve: confirm the stable or development Git URL matches the Package Registry entry and that required Deucarian dependencies are installed.
- Unity compile errors after install: let Package Manager finish resolving dependencies, then check asmdef references against `package.json` dependencies.
- Behavior appears to belong in another package: consult `AGENTS.md` and the Package Registry governance docs before moving or duplicating code.

## Validation

Run the shared package validator from this repository root:

```powershell
python C:/Repositories/Package-Registry/Tools/deucarian_package_validator.py --registry-root C:/Repositories/Package-Registry --repository-root . --config deucarian-package.json
```

Documentation-only updates should still pass:

```powershell
git diff --check
```

## License

MIT. See `LICENSE.md`.

## Simple typed usage

See [Simple usage](Documentation~/SimpleUsage.md) for the short caller, Inspector selections and one-time scoped setup.
