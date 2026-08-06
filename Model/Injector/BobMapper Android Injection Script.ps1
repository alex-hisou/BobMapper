param
(
    [Parameter(Mandatory=$true)][string]$moddedPath,
    [Parameter(Mandatory=$true)][string]$toolsPath
)

#THE MOST FRAGILE SCRIPT EVER DEVELOPED IN HUMAN HISTORY
#BE AWARE

$apktool = Join-Path -Path $toolsPath -ChildPath "apktool.jar"
$destination = Split-Path -Path $moddedPath -Parent
$unsignedapk = Join-Path -Path $destination "Modded Robbery Bob.apk"
java -jar $apktool b $moddedPath -o $unsignedapk
$uberapk = Join-Path -Path $toolsPath -ChildPath "uber-apk-signer.jar"
java -jar $uberapk --apks $unsignedapk --out $destination
Remove-Item -Path $unsignedapk
$signedapk = Join-Path -Path $destination -ChildPath "Modded Robbery Bob-aligned-debugSigned.apk"
$adbconfirm = Read-Host "Apk compiled to $signedapk. If your phone is plugged in and has USB debugging enabled, type y to immediatley install it. Otherwise, type n to show the output folder."
if($adbconfirm -eq "y")
{
    $adblocation = Join-Path -Path $toolsPath -ChildPath "platform-tools-latest-windows\platform-tools"
    Set-Location -Path $adblocation
    .\adb install -r -d $signedapk
    Read-Host "If you see no errors above, the script ran successfully. Otherwise, consult developer.android.com/tools/adb. Press any key to exit."
    exit 0
}
else {
    explorer /select,$signedapk
    exit 0
}