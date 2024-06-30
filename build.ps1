dotnet build
mkdir "D:\SteamLibrary\steamapps\common\Farmer Against Potatoes Idle\BepInEx\plugins\FapiQolPlugin" -erroraction silentlycontinue
cp .\bin\Debug\net472\FapiQolPlugin.dll "D:\SteamLibrary\steamapps\common\Farmer Against Potatoes Idle\BepInEx\plugins\FapiQolPlugin"
cp .\bin\Debug\net472\FapiQolPlugin.pdb "D:\SteamLibrary\steamapps\common\Farmer Against Potatoes Idle\BepInEx\plugins\FapiQolPlugin"
echo "Copy is done"