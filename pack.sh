#!/bin/bash

VERSION_SUFFIX=$1

if [ -z $VERSION_SUFFIX ]; then
  VERSION=$(grep -Po ./src/Rlx/Rlx.csproj -e '(?<=<VersionPrefix>)[^<]+')
  echo "Using Version $VERSION"
  dotnet pack --configuration=Release /property:Version=$VERSION
else
  echo "Using Version Suffix $VERSION_SUFFIX"
  dotnet pack --configuration=Release --version-suffix="$VERSION_SUFFIX"
fi
