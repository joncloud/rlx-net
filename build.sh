dotnet restore
dotnet build --configuration=Release
find ./tests -mindepth 1 -maxdepth 1 -type d -name '*.Tests' | xargs -L1 dotnet test --configuration=Release
dotnet pack --configuration=Release --version-suffix="beta$TRAVIS_BUILD_ID"
./push_if_set.sh