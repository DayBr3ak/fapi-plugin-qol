
dotnet build --configuration Release
rm -Force -Recurse bin/tmp/FapiQolPlugin
mkdir bin/tmp/FapiQolPlugin  -erroraction silentlycontinue
cp bin/Release/net472/FapiQolPlugin.dll bin/tmp/FapiQolPlugin
cp bin/Release/net472/FapiQolPlugin.pdb bin/tmp/FapiQolPlugin
cp readme.md bin/tmp/FapiQolPlugin

$compress = @{
  Path = "bin/tmp/FapiQolPlugin"
  CompressionLevel = "Fastest"
  DestinationPath = "bin\FapiQolPlugin-Unity.Mono-win-x64-1.0.0.zip"
  Force = $true
}
Compress-Archive  @compress