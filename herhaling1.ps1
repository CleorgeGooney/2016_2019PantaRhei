Param(
  [int]$aantal
)
$legetabel = @("N")*$aantal

for ($i = 1; $i -lt $aantal+1; $i++)
{
    for ($j = 1; $j -lt $aantal+1; $j++)
    {
      $waarde = $j%$i;

      if ($waarde -eq 0)
        {
            if ($legetabel[$j-1] -eq "N")
            {
            $legetabel[$j-1] = "J"
            }
            else 
            {
            $legetabel[$j-1]="N"
            }
        }
    }
    Write-Host "Situatie na ronde $i = $legetabel"
}
$jaIndexen = ""
for ($i = 1; $i -lt $aantal+1; $i++)
{
    if ($legetabel[$i-1] -eq "J"){
    $jaIndexen = $jaIndexen + [string]$i + " "}
}
Write-Host Posities met waarde 'J' na $aantal rondjes: $jaIndexen