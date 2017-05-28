#$env:Path += "C:\Users\mathi\Documents\avondschool\besturingssystemen\Praktijk\Powershell\oefeningen"
if ($args.Length -ne 3)
{
    Write-Host "Gelieve exact 3 parameters op te geven"
}
elseif (([int]$args[0] -lt 1) -or [int]$args[1] -lt 2)
{
    Write-Host "Gelieve een minimumwaarde van meer dan 1 op te geven"
}
elseif ($args[0] -ge $args[1])
{
    Write-Host "Minimumwaarde kan niet groter of gelijk aan maximumwaarde zijn"
}
else
{
    $min = $args[0]
    $max = $args[1]
    $aantalRondes = $args[2]
    $scorecomputer = 0
    $scorespeler = 0

    Write-Host “`nWelkom bij Hoger-Lager voor getallen van $min tot en met $max. We spelen $aantalRondes rondes.”
    for ($teller = 1 ; $teller -le $aantalRondes; $teller++)
    {
        $gewonnen = $false
        $computergewonnen = $false
        $random = (Get-Random -Maximum ($max + 1 - $min)) + $min
        $randomSpeler = (Get-Random -Maximum ($max + 1 - $min)) + $min
        
        Write-Host "`nRonde $teller`n"
        $invoer= Read-Host "De computer gooit $random. Gooi je hoger? (J/N)?"
        $invoercorrect = $false
        do 
        {
            if (($invoer.ToUpper() -eq "J") -or ($invoer.ToUpper() -eq "N"))
            {
                $invoercorrect = $true
            }
            else
            {
                $invoer= Read-Host "Gooi je hoger? (J/N)?"
            }
        }until ($invoercorrect)

        if ($randomSpeler -gt $random)
        {
            if($invoer.ToUpper() -eq "J")
            {
                $gewonnen = $true
            }
            if($invoer.ToUpper() -eq "N")
            {
                $computergewonnen =$true
            }
        }
        if ($randomSpeler -lt $random)
        {
            if($invoer.ToUpper() -eq "J")
            {
                $computergewonnen = $true
            }
            if($invoer.ToUpper() -eq "N")
            {
                $gewonnen = $true
            }
        }

        if($randomSpeler -eq $random)
        {
            $computergewonnen = $true
        }

        if ($gewonnen)
        {
            Write-Host "Je gooit $randomSpeler. Speler wint deze ronde!"
            $scorespeler++
        }
        if ($computergewonnen)
        {
            Write-Host "Je gooit $randomSpeler. Computer wint deze ronde!"
            $scorecomputer++
        }
    }
    Write-Host "`nScore na $aantalRondes rondes: Speler wint $scorespeler ronde(s), computer wint $scorecomputer ronde(s)."
    if ($scorecomputer -gt $scorespeler)
    {
        Write-Host "`nComputer wint!"
    }
    elseif ($scorecomputer -eq $scorespeler)
    {
        Write-Host "`nGelijkspel!"
    }
    else
    {
        Write-Host "`nSpeler wint spel!"
    }
}

