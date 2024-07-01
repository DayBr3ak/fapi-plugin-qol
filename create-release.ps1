
dotnet build --configuration Release
rm -Force -Recurse bin/tmp/FapiQolPlugin
mkdir bin/tmp/FapiQolPlugin  -erroraction silentlycontinue
cp bin/Release/net472/FapiQolPlugin.dll bin/tmp/FapiQolPlugin
cp bin/Release/net472/FapiQolPlugin.pdb bin/tmp/FapiQolPlugin
cp readme.md bin/tmp/FapiQolPlugin

# extract version number from project
[xml]$xmlDocument = Get-Content -Path "FapiQolPlugin.csproj"
$version = $xmlDocument.SelectNodes("//Project//PropertyGroup")[0].SelectSingleNode("Version").InnerText
Write-Output "ver: $version"

$compress = @{
  Path = "bin/tmp/FapiQolPlugin"
  CompressionLevel = "Fastest"
  DestinationPath = "bin\FapiQolPlugin-Unity.Mono-win-x64-$version.zip"
  Force = $true
}
Compress-Archive  @compress

git add -A
git commit -m "Bump release $version"
git tag $version
git push origin master
git push origin tag $version
