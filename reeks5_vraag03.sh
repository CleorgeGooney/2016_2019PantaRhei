startWaarde=$1
while [ $startWaarde -gt $2 ]
do
	read -p "$startWaarde is groter dan $2, geef een nieuwe startwaarde in:
>> " startWaarde
#$'string /n string' is hier mss mooiere oplossing, maar $waarden worden letterlijk afgedrukt dr single quotes.
#en $"string /n string" negeert de /n...
done

echo "Script dat telt van $startWaarde tot en met $2:"
for (( i = $startWaarde; i <= $2; i++ )) 
do
	echo $i
done
