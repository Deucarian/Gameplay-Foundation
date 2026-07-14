# Security policy

## Supported versions

Security fixes are considered for the current `main` package channel. `develop` is a preview channel and may receive a fix before promotion. Older commits and locally modified copies are not guaranteed to receive backports.

## Report a vulnerability privately

Use [GitHub private vulnerability reporting](https://github.com/Deucarian/Gameplay-Foundation/security/advisories/new). Do not open a public issue for a suspected vulnerability.

Include the affected package version or commit, impact, deterministic inputs or proof of concept, and any known mitigations. Remove personal or unreleased game data and use synthetic examples where possible.

The maintainers will triage the report in GitHub's private advisory, may ask for additional evidence, and will coordinate disclosure after a fix or mitigation is available. No response or remediation deadline is guaranteed.

Security scope covers the pure-C# primitives shipped by this package. Unity, the host runtime, and downstream game systems remain under their respective security processes.
