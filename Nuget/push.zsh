#!/usr/bin/env zsh

set -euo pipefail

SCRIPT_DIRECTORY=$(dirname "$(readlink -f "$0")")

cd "$SCRIPT_DIRECTORY" || { echo "Failed to cd to $SCRIPT_DIRECTORY" >&2; exit 1; }
echo "Script directory: $PWD"

cd ".."
echo "Working directory: $PWD"

: "${NUGET_ORG_API_KEY:?"Environment variable is not set"}"

readonly CONFIGURATION="Release"
readonly VERSION="0.9.0"
readonly FEED="https://api.nuget.org/v3/index.json"
readonly API_KEY="${NUGET_ORG_API_KEY}"

packages=(
  "Rumrunner0.BackToReality.SharedExtensions"
)

for package in "${packages[@]}"; do
  nupkg="$package/bin/${CONFIGURATION}/$package.${VERSION}.nupkg"

  if [[ ! -f "$nupkg" ]]; then
    echo "Package not found: $nupkg" >&2
    exit 1
  fi

  echo "Pushing $nupkg..."
  dotnet nuget push "$nupkg" \
    --source "$FEED" \
    --api-key "$API_KEY"
done

echo "Done."