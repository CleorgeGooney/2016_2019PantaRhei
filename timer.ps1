clear
for ($seconde = 10; $seconde -ge 1; $seconde--)
{
    Write-Host $seconde
    Start-Sleep -Seconds 1
    clear
}
clear
Write-Host "boom"