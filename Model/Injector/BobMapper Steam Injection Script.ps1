param 
(
    [Parameter(Mandatory=$true)][string]$moddedPath,
    [Parameter(Mandatory=$true)][string]$destination
)

try {
    $stream = [System.IO.File]::Open(
        $destination,
        [System.IO.FileMode]::Open,
        [System.IO.FileAccess]::ReadWrite,
        [System.IO.FileShare]::None
    )
    $stream.Close()
}
catch {
    Write-Host "resources.dat is in use. Make sure that Robbery Bob or any other process using the file is closed"
    Write-Host "Press any key to close this window"
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
    exit 1
}

Copy-Item $moddedPath -Destination $destination
Start-Process "steam://rungameid/372960"