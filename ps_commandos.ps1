# Mathijs Deprez
# F8, runselection

#1
Get-Help Get-Help
#2
Get-Help date
    # zoek naar Get-Date
#3
Get-Date | Get-Member
#4
Get-Help Get-Date -examples
Get-Help Get-Date -full
#5
Get-Date -UFormat %A
#6
Get-Date -UFormat %Y
#7
Get-Date -UFormat %d/%m/%Y" "%T
#8
$vandaag = Get-Date
$datumTienDagenTerug = Get-Date -Day ((Get-Date -UFormat %d) - 10)
Write-Host $vandaag`n$datumTienDagenTerug
#9
Get-Help Read-Host
Get-Help Write-Host
#10
$invoer = Read-Host "Geef invoer aub"
#11
Write-Host $invoer
Write-Host $invoer.GetType()
#12
$x=10
[int]$x=10
[System.Int32]$x=10
Set-Variable -Name "x" -Value (10)
#13
$x=10
$x=$x+10
$x
$x=$x*20
$x
$x=$x+3
$x
$x=$x/3
$x
#14
[double]$x=10
$x=$x+10
$x
$x=$x*20
$x
$x=$x+3
$x
$x=$x/3
$x
#15
[int]$x=10
$x=$x+10
$x
$x=$x*20
$x
$x=$x+3
$x
$x=$x/3
$x
#16
$string = "Dit is een String"
$string | Get-Member
$string.Substring(0,1)
$string.Length
$string.Contains("Dit")
$array = $string.Split(" ")
#17
[string[]]$week = "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag", "Zondag"
#18
$week[4].ToUpper()
$week[4].Length
#19
$week[0].CompareTo($week[5])
    # -1 (Maandag komt alfabetisch voor Zaterdag)
#20
$week[3] + $week[5]
    # DonderdagZaterdag (concatenatie)
#21
$week[3] + " " + $week[5]
    # Donderdag Zaterdag
#22
$tweeDagen = $week[3] + " " + $week[5]
$tweeDagenTabel = $tweeDagen.Split(" ")
Write-Host $tweeDagenTabel.GetType()
    # we krijgen een tabel met 2 entries: Donderdag, Zaterdag
    # check:
    foreach ($elem in $tweeDagenTabel){Write-Host "nieuw element: $elem"}
