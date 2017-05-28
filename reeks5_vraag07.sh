if  [ $# -ne 1 ]
then
echo "Gelieve het script met slechts 1 parameter op te roepen."
exit

elif [ -f $1 ]
then
	IFS="."
	i=0
	declare -a bestand
	for b in $1
	do
		bestand[i]="$b"
		i=$((i+1))
	done
	
	if [ ${#bestand[@]} -eq 1 ]
	then
		cp "$1" "${bestand[0]}_kopie"
		echo "Het bestand $1 is gekopieerd naar ${bestand[0]}_kopie."
	else
		cp "$1" "${bestand[0]}_kopie.${bestand[1]}"
		echo "Het bestand $1 is gekopieerd naar ${bestand[0]}_kopie.${bestand[1]}."
	fi
else
echo "Het bestand $1 bestaat niet."
exit
fi
