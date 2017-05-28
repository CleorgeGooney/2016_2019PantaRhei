#$env:Path += "C:\Users\mathi\Documents\avondschool\besturingssystemen\Praktijk\Powershell\oefeningen"

#oefening 1

<#

for ($i=1;$i -lt 11;$i++)
{
    Write-Host $i
}

#>

#oefening 2

<#

[string[]]$week = "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag", "zondag"

for ($i=0;$i -lt 7;$i++)
{
    Write-Host $week[$i].Substring($i,1)
}
#geeft een fout want er zijn slechts 6 letters in de zevende dag (zondag)

#>

#oefening 3

<#

[string[]]$week = "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"

foreach ($day in $week)
{
    if ($day.Substring(3,1) -eq "d")
    {
        Write-Host $day
    }
}

#>

#oefening 4

<#

param($positie, $letter)
$week = "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"


foreach ($i in $week)
{
    if ($i.Substring($positie-1,1) -eq $letter)
    {
    Write-Host $i
    }
}

#>

#oefening 5

<#
param([int]$eersteGetal, [int]$tweedeGetal)


for ($eersteGetal; $eersteGetal -le $tweedeGetal; $eersteGetal++)
{
    Write-Host $eersteGetal
}

#>

#oefening 6

<#

param([int]$eersteGetal, [int]$tweedeGetal)

if ($eersteGetal -lt $tweedeGetal)
{
    for ($eersteGetal; $eersteGetal -le $tweedeGetal; $eersteGetal++)
    {
        Write-Host $eersteGetal
    }
}
else
{
    for ($eersteGetal; $eersteGetal -ge $tweedeGetal; $eersteGetal--)
    {
        Write-Host $eersteGetal
    }
}

#>

#oefening 7

<#
param([int]$eersteGetal, [int]$tweedeGetal)

while ($eersteGetal -gt $tweedeGetal)
{
    [int]$eersteGetal = Read-Host "$eersteGetal is groter dan $tweedeGetal, geef nieuwe startwaarde in"
}

for ($eersteGetal; $eersteGetal -le $tweedeGetal; $eersteGetal++)
{
    Write-Host $eersteGetal
}
#>

#oefening 8

<#

Write-Host "# parameters:"$args.Count

for ($i=1; $i -lt $args.Count; $i = $i + 2)
{
    Write-Host $args[$i]
}

#>

#oefening 9

<#

$resultaat = 1

for ($i=1 ; $i -le [int]$args[0]; $i++)
{
    $resultaat = $resultaat * 2
}
Write-Host $resultaat

#>

<#

$resultaat = 1
[int]$grondgetal = $args[1]
[int]$macht = $args[0]

for ($i=1 ; $i -le $macht; $i++)
{
    $resultaat = $resultaat * $grondgetal
}
Write-Host "$grondgetal tot de $macht macht is $resultaat"

#>

#oefening 10

<#

$beginwaarde = 8
$resultaat = 1

for ($teller = 1; $teller -le $beginwaarde; $teller++)
{
    $resultaat = $resultaat * $teller
}
Write-Host $beginwaarde! = $resultaat

$beginwaarde = 8
$resultaat = 1

for ($teller = $beginwaarde; $teller -ge 1; $teller--)
{
    $resultaat = $resultaat * $teller
}
Write-Host $beginwaarde! = $resultaat

#>

#oefening 11 - 13


$tabel = @()
for ($teller = 1; $teller -le 7; $teller++)
{
    $tabel += Get-Date -Day $teller -Uformat %A
}
#[array]::Sort($tabel)
#$tabel | Sort-Object

for ($i = 0; $i -lt $tabel.Count; $i++)
{
    $indexKleiner = $i

    for ($k = $i + 1; $k -lt $tabel.Count; $k++)
        {
        if (($tabel[$k].CompareTo($tabel[$indexKleiner])) -eq -1)
            {
                $indexKleiner = $k
            }
        }
    $backup = $tabel[$indexKleiner]
    $tabel[$indexKleiner] = $tabel[$i]
    $tabel[$i] = $backup
}
Write-Host $tabel


#oefening 14

<#

do
{
    $invoer = Read-Host "Geef tekst in (J of j om te stoppen)"
    $tabel = $invoer.Split(" ")
    Write-Host "Het eerste deel van $invoer is: $($tabel[0])"

} until($tabel[0] -eq "J" -or $tabel[0] -eq "j")

#>

#oefening 15

<#

param($aantalKolommen, $aantRijen)
for ($y = 0; $y -lt $aantRijen; $y++)
{
    if ($y%2 -eq 0)
    {
    for ($x = 0; $x -lt $aantalKolommen; $x++)
        {
            if ($x%2 -eq 0)
            {
                Write-Host "X" -NoNewline
            }
            else
            {
                Write-Host "O" -NoNewline
            }
        }
    }
    else
    {
        for ($x = 0; $x -lt $aantalKolommen; $x++)
        {
            if ($x%2 -ne 0)
           {
                Write-Host "X" -NoNewline
            }
            else
            {
                Write-Host "O" -NoNewline
            }
        }
    }
    Write-Host
}

#>