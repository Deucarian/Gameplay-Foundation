# Third-party notices

This notice describes the dependency and distribution inventory for `com.deucarian.gameplay-foundation` `0.1.0`. It does not replace the repository's [MIT license](LICENSE.md), and it does not grant rights to software supplied separately.

## Review basis

The reviewed baseline is `origin/main` commit `f6682ab41af197adc072ef4cad11e9e18f06da3f`. Its `npm pack --dry-run` inventory contained 34 package files. The tracked and packed inventories were checked for common vendor/third-party directories, compiled binaries and archives, Git submodules, Git LFS pointers, separate license markers, and media/font assets.

That inventory identified no files marked or located as vendored third-party source, no compiled binary/archive candidates, no submodules, no LFS pointers, and no media/font asset candidates.

## Direct package dependencies

The reviewed `package.json` declares no direct package dependencies. The runtime assembly is configured as a pure-C# assembly with no Unity engine references.

## Host platform

The manifest requires Unity `2021.3` as the package host even though its runtime assembly is pure C#. Unity is not included in this package. Use of Unity is governed by the applicable [Unity Editor Software Terms](https://unity.com/legal/editor-terms-of-service/software).

Re-run the inventory and update this notice whenever dependencies or distributed content change.
