IFS=";"" ""|"
while true
do
read -p "Geef tekst in (J of j in om te stoppen): " ingevoerdeTekst
declare -a tabel
i=0
for t in $ingevoerdeTekst
do
	tabel[i]="$t"
	i=$(( i+1 ))
done
if [ ${tabel[0]} = J ] || [ ${tabel[0]} = j ]
then
exit
else
echo "Het eerste deel van $ingevoerdeTekst is ${tabel[0]}"
fi
done
