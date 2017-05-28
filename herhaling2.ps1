#$env:Path += "C:\Users\mathi\Documents\avondschool\besturingssystemen\Praktijk\Powershell\oefeningen"
Param($stopLetter, [int]$index)

$totaleInvoer = 0
$totaleInvoerLengte = 0
$gevonden = $false;
do
{
    $invoer = Read-Host "Geef tekst in (indien $stopLetter zich op positie $index bevindt stoppen we): "
    $aantalKarakters = $invoer.Length
    $tabel = @();
   
    for ($i=0; $i -lt $aantalKarakters; $i++)
    {
        $letter = $invoer.Substring($i, 1)
        $tabel += $letter;
    }
    $gevonden = $tabel[$index-1] -eq $stopLetter
    
    if ($gevonden -eq $false)
    {
        $totaleInvoer++
        $totaleInvoerLengte +=$aantalKarakters
    }

}until ($gevonden -eq $true);

if ($totaleInvoer -eq 0){$totaleInvoer = 1}
Write-Host "Aantal correcte invoeren = $totaleInvoer`nTotaal aantal karakters = $totaleInvoerLengte`nGemiddelde invoerlengte = $($totaleInvoerLengte/$totaleInvoer)"