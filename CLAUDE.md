# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

.NET solution with the class library `Rumrunner0.BackToReality.SharedExtensions` (published to nuget.org) and its xunit test project `Rumrunner0.BackToReality.SharedExtensions.Tests`. The library has zero external NuGet dependencies (BCL only) — don't add packages to it unless asked. There is no CI: `dotnet build` + `dotnet test` are the verification steps, and the build must stay at 0 warnings (missing XML docs on public members produce warnings; xunit analyzers lint the tests).

## Gotchas

- The release version lives in TWO places that must stay in sync: `<VersionPrefix>` in `Rumrunner0.BackToReality.SharedExtensions/Rumrunner0.BackToReality.SharedExtensions.csproj` and `VERSION=` in `Nuget/push.zsh`.
- Cross-project MSBuild config (target framework `net9.0`, language settings, build matrix, assembly metadata) lives in `Directory.Build.props`; packaging, versioning, strong naming, and doc generation live in the library csproj.
- Assemblies are strong-named with a key outside the repo (`../documents/rumrunner0_backtoreality_sharedextensions.snk`, gitignored) — a fresh clone won't build without it.
- `ImplicitUsings` is disabled — every file lists explicit `using` directives.
- Package versions are centralized (`Directory.Packages.props`, CPM): a `PackageReference` must not carry `Version`; add a `PackageVersion` entry there instead.
- The test project explicitly opts out of packing, signing, and doc generation (`IsPackable`/`SignAssembly`/`GenerateDocumentationFile` all `false`) — keep it that way.
- Old `.nupkg`/`.snupkg` files accumulate in `bin/Release` across releases: `dotnet clean` only deletes recorded build outputs, and `Pack` never registers its packages there. This is expected and accepted — NEVER delete them (no `rm`, no cleanup steps in scripts). `Nuget/push.zsh` picks the exact file by version, so stale packages are harmless.

## Releases

Use `/release <version>`. Manual flow: bump the version in both places above → `dotnet clean --configuration Release` → verify a clean `dotnet build --configuration Release` and a green `dotnet test --configuration Release --no-build` → commit `Release X.Y.Z` → `Nuget/pack.zsh` → `Nuget/push.zsh` (needs `NUGET_ORG_API_KEY`, exported in the shell profile). A pushed version can never be overwritten on nuget.org. Packages from earlier releases remain in `bin/Release` after `dotnet clean` — that's expected (see Gotchas); leave them in place.

## Code style

Style is codified in `.editorconfig`. Check with `dotnet format style --verify-no-changes`; NEVER run plain `dotnet format` or `dotnet format whitespace` — Roslyn has no option for the `new (` spacing rule below and strips the space. Key points:

- Tabs for indentation; Allman braces. Single-line guard clauses stay unbraced on one line: `if (source is null) return;`.
- Target-typed `new` takes a space before its parentheses: `return new (args);` — do not collapse to `new(`.
- File-scoped namespaces mirroring the feature folder: `Rumrunner0.BackToReality.SharedExtensions.<Folder>`. New feature areas get their own folder.
- XML doc comments (`///`) on every public member.
- Extension classes are `public static class <Type>Extensions`.

## Git

Commit directly to `main` — no feature branches, PRs, or tags. Messages are short and sentence-case (`Added X`, `Fixed Y`, `Improved Z`); releases are `Release X.Y.Z`; unstable APIs are flagged `(EXPERIMENTAL)` in the commit message.
