# Deucarian Gameplay Foundation Agent Notes

Package ID: `com.deucarian.gameplay-foundation`
Repository: `Deucarian/Gameplay-Foundation`

Follow the canonical Deucarian governance docs in [Package Registry](https://github.com/Deucarian/Package-Registry/blob/develop/ARCHITECTURE.md), especially capability ownership and dependency rules.

## Ownership

This package owns:

- Pure C# gameplay identifiers, validation report primitives, gameplay tags, deterministic random sources, clocks, ticks, timers, stats, modifiers, and snapshots.

Registered capabilities:
- None.

This package must not own:

- Health/damage/combat, weapons, progression, saving, encounters, UI, networking, GameObjects, MonoBehaviours, service locators, Entities, or global mutable runtime state.

## Dependencies

Allowed dependency shape:

- Dependency-free runtime package.
- Runtime assembly keeps `noEngineReferences` enabled.

Required dependencies and why:

- None.

Optional/version-defined dependencies:

- None.

Architecture exceptions:

- None.

## Policies

- Keep the package pure C# and runtime-safe.
- Do not add UnityEngine or UnityEditor dependencies to the runtime assembly.
- Domain-specific gameplay validation belongs in the package or template that owns that domain content.
- Logging: Do not introduce direct Unity Debug calls.
- Testing: Keep deterministic IDs, random, clock/tick, timer, validation, stat, and modifier behavior covered by EditMode tests.

## Validation

Run the shared validator before committing:

```powershell
python C:/Repositories/Package-Registry/Tools/deucarian_package_validator.py --registry-root C:/Repositories/Package-Registry --repository-root . --config deucarian-package.json
```

Also run existing repository tests when changing code or asmdefs. Documentation-only updates should still run `git diff --check`.

## Codex Guidance

- Inspect current files before changing anything.
- Work on `develop`; do not edit or merge `main` unless the task is promotion-only.
- Do not edit `Library/PackageCache`.
- Do not guess package versions or dependency versions.
- Do not add package dependencies casually; update asmdefs, `package.json`, `deucarian-package.json`, Package Registry, and fallback catalogs together when a dependency is truly required.
- Do not create local copies of shared helpers.
- Keep commits focused and report exactly what changed and what was validated.

