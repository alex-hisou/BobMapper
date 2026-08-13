param
(
    [Parameter(Mandatory=$true)][string]$moddedPath,
    [Parameter(Mandatory=$true)][string]$bkCrackPath,
    [Parameter(Mandatory=$true)][string]$xmlHeader
)

Write-Host "resources.dat is encrypted. Running decryption script..."
Set-Location -Path $bkCrackPath
$decryptedZip = "decryptedresources.zip"
& ".\bkcrack" -C $moddedPath -k 9c20904a 888d1a8d 9483810d -D $decryptedZip
Copy-Item $decryptedZip -Destination $moddedPath
Write-Host "Decryption complete. Press any key to continue the injection process"
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
exit 0