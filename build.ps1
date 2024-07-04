Invoke-Expression "& .\find-fapi-install.ps1"

echo $env:GameDir

dotnet build
mkdir "$env:GameDir\BepInEx\plugins\FapiQolPlugin" -erroraction silentlycontinue
cp .\bin\Debug\net472\FapiQolPlugin.dll "$env:GameDir\BepInEx\plugins\FapiQolPlugin"
cp .\bin\Debug\net472\FapiQolPlugin.pdb "$env:GameDir\BepInEx\plugins\FapiQolPlugin"
echo "Copy is done"
