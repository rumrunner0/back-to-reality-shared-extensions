#!/bin/sh

# Stop hook: blocks Claude from finishing a turn while the solution fails to compile.
# Incremental: skips the build when no .cs/.csproj/.props changed since the last SUCCESSFUL build.
# Loop guard: if a block already triggered a fix attempt this turn (stop_hook_active), notify the user via systemMessage instead of blocking again.

input=$(cat)
cd "${CLAUDE_PROJECT_DIR:-$(dirname "$0")/../..}" || exit 0

stamp=".claude/.build-stamp"
[ -f "$stamp" ] && [ -z "$(find . \( -name bin -o -name obj -o -name .git \) -prune -o \( -name '*.cs' -o -name '*.csproj' -o -name '*.props' \) -newer "$stamp" -print -quit 2>/dev/null)" ] && exit 0

output=$(dotnet build --nologo --verbosity quiet 2>&1) && { touch "$stamp"; exit 0; }

if printf '%s' "$input" | jq -e '.stop_hook_active == true' >/dev/null 2>&1; then
  printf '{"systemMessage":"Build gate: dotnet build is STILL failing after a fix attempt — needs your attention."}'
  exit 0
fi

# Prefer MSBuild diagnostic lines (path(line,col): error CSxxxx:); fall back to raw tail.
errors=$(printf '%s' "$output" | grep -iE ': error' | head -40)
[ -z "$errors" ] && errors=$(printf '%s' "$output" | tail -40)
printf '%s' "$errors" | jq -Rs '{decision:"block", reason:("dotnet build failed. Fix the compile errors before finishing:\n" + .)}'
exit 0