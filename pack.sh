#!/bin/bash

VALUE=$1

if [ -z $VERSION_SUFFIX ]; then
  VERSION=$(grep -Po ./src/Rlx/Rlx.csproj -e '(?<=<VersionPrefix>)[^<]+')
  echo "Using Version $VERSION"
  dotnet pack --configuration=Release /property:Version=$VERSION
else
  echo "Using Version Suffix $VALUE"
  dotnet pack --configuration=Release --version-suffix="$VALUE"
fi
