# saap
C:\Users\franz-filip\Downloads\nuget.exe pack C:\FH\Master\SAAP\projectrepo\saap\SeleniumBasicUtilities\SeleniumBasicUtilities\SeleniumBasicUtilities.csproj -IncludeReferencedProjects -Prop Configuration=Release

dotnet pack .\SeleniumBasicUtilities\SeleniumBasicUtilities.csproj --configuration Release --include-source --include-symbols

## this worked
nuget pack -properties version=1.0.0 -NoDefaultExcludes
dotnet nuget push SeleniumBasicUtilities.1.0.2.nupkg --api-key oy2i53yht3sb5tom2exqj5b2p7jsttuehpdrdjfpflotf4 --source https://api.nuget.org/v3/index.json