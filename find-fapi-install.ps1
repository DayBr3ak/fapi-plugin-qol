# todo
# this script will need to find the fapi install and update paths in build.ps1 and FapiQolPlugin.csproj
# Path to the configuration file
$configFilePath = "config.conf"

# Function to read key-value pairs from the config file
function Get-ConfigValue {
    param (
        [string]$key
    )
    $configLines = Get-Content $configFilePath
    foreach ($line in $configLines) {
        if ($line -match "^$key=(.*)$") {
            return $matches[1]
        }
    }
    return $null
}

# Read the GameDir value from the config file
$gameDir = Get-ConfigValue -key "FAPI_GAME_DIR"

# Check if the value was retrieved successfully
if ($null -eq $gameDir) {
    Write-Error "GameDir not found in config file."
    exit 1
}

# Set the environment variable for the current session
$env:GameDir = $gameDir

# Optional: Verify the environment variable
Write-Host "GameDir is set to: $env:GameDir"

# Path to the .csproj file
$csprojFilePath = "FapiQolPlugin-csproj.template"

function Update-Placeholder {
    param (
        [string]$filePath,
        [string]$newValue,
        [string]$placeholder,
        [string]$OutFile
    )

    $fileContent = Get-Content -Path $filePath
    $updatedContent = $fileContent -replace $placeholder, $newValue
    Set-Content -Path $OutFile -Value $updatedContent

    Write-Host "Placeholder '$placeholder' replaced with '$newValue' in '$OutFile'"
}

# Update the placeholder
Update-Placeholder -filePath $csprojFilePath -newValue $gameDir -placeholder '__REPLACED_VALUE__' -OutFile "FapiQolPlugin.csproj"

