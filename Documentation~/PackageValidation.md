# Package Validation

Phase 1A validation requirements:

- import in a clean Unity `6000.3.5f1` project
- run package EditMode tests
- consume the package through a local `file:` reference in a compatibility harness
- run the harness tests
- run a donor integration proof without migrating the full donor
- confirm no package dependency cycle
- confirm no duplicated Deucarian package responsibility
- record hot-path allocation behavior

## Recorded Phase 1A Validation

Unity version: `6000.3.5f1`.

Clean validation project:

`C:\Repositories\Deucarian\GameplayFoundation-TestProject`

Local package reference:

```json
"com.deucarian.gameplay-foundation": "file:C:/Repositories/Deucarian/Gameplay-Foundation"
```

Import command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.5f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Repositories\Deucarian\GameplayFoundation-TestProject' -logFile 'C:\Repositories\Deucarian\GameplayFoundation-TestProject-harness-import.log'
```

Import result: Unity returned `0`; no `error CS`, `Compilation failed`, or `Scripts have compiler errors` entries were present in the final import log.

EditMode test execution command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.5f1\Editor\Unity.exe' -batchmode -projectPath 'C:\Repositories\Deucarian\GameplayFoundation-TestProject' -executeMethod BatchEditModeTestRunner.Run -batchTestResults 'C:\Repositories\Deucarian\GameplayFoundation-TestProject\Logs\batch-editmode-results.txt' -logFile 'C:\Repositories\Deucarian\GameplayFoundation-TestProject-batch-tests.log'
```

EditMode result:

```text
result=Passed; passCount=16; failCount=0; skipCount=0; duration=0,094
```

The 16 tests include the package EditMode suite plus a local-file compatibility harness and a donor stat-workflow proof.

Hot-path allocation result: `RepresentativeStatEvaluation_HasNoSteadyStateAllocationsAfterWarmup` passed. The test warms `StatBlock.GetValue`, measures `GC.GetAllocatedBytesForCurrentThread` across 10,000 representative evaluations, and asserts `0` steady-state bytes allocated.

Dependency-cycle result: package dependencies are empty in `package.json`, so no package dependency cycle exists in Phase 1A.

Responsibility overlap result: compared against the cloned Deucarian packages in `C:\Repositories\Deucarian\Registry-Clones`; Gameplay Foundation does not duplicate Core State, Logging, Diagnostics, Editor, UI Binding, UI Flow, or Theming ownership.
