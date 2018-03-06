#!/bin/bash

dotnet restore
dotnet build --configuration=Release
find ./tests -mindepth 1 -maxdepth 1 -type d -name '*.Tests' | xargs -L1 dotnet test --configuration=Release

if [ -z $VERSION_SUFFIX ]; then
  VERSION=$(grep -Po ./src/Rlx/Rlx.csproj -e '(?<=<VersionPrefix>)[^<]+')
  dotnet pack --configuration=Release /p:Version=$VERSION
else
  dotnet pack --configuration=Release --version-suffix="$VERSION_SUFFIX$TRAVIS_BUILD_ID"
fi

./push_if_set.sh
