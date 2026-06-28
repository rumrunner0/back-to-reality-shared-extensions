---
name: release
description: Release a new version of Rumrunner0.BackToReality.SharedExtensions to nuget.org — bump the version everywhere, commit, clean Release build, pack, push. Usage /release <version>, e.g. /release 0.9.1
disable-model-invocation: true
---

Release version `$ARGUMENTS` of the package to nuget.org. If no version was given, or it isn't a bare `X.Y.Z` (optionally with a `-suffix`), stop and ask for one.

Work from the repository root. Follow the steps in order. If any step fails, stop and report — do not improvise around a failed step, and never run the push after a failure.

1. **Preflight.**
   - `git status` must show no modified or staged files. If dirty, stop and ask.
   - Verify the API key is available without printing it: `[[ -n "$NUGET_ORG_API_KEY" ]] && echo ok || echo MISSING`. If missing, tell the user to export it (it lives in their shell profile) and stop.
   - Confirm the new version is higher than the current `<VersionPrefix>` in `Directory.Build.props`.

2. **Bump the version in both places.**
   - `Directory.Build.props`: set `<VersionPrefix>` to the new version (leave `<VersionSuffix>` empty for a normal release).
   - `Nuget/push.zsh`: set `readonly VERSION="<version>"`.
   - Grep both files afterwards to confirm they agree.

3. **Commit** directly on `main` with the message `Release <version>` (e.g. `Release 0.9.1`).

4. **Clean** so the pack comes from a fresh build: `rm -rf Rumrunner0.BackToReality.SharedExtensions/bin Rumrunner0.BackToReality.SharedExtensions/obj`.

5. **Validate the build.** `dotnet build -c Release` must succeed with 0 warnings and 0 errors, and `dotnet test -c Release --no-build` must pass with 0 failures. Otherwise stop and report — do not pack or push.

6. **Pack.** Run `Nuget/pack.zsh`.

7. **Push to nuget.org.** Run `Nuget/push.zsh`. This is permanent — a pushed version number can never be reused or overwritten on nuget.org.

8. **Push the commit.** `git push origin main`.

9. **Report**: the released version, the `.nupkg` path, and the package URL `https://www.nuget.org/packages/Rumrunner0.BackToReality.SharedExtensions/<version>`.
