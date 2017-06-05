min=$1
max=$2
gewonnenComputer=false
gewonnenSpeler=false
aantalRondes=$3
aantalSpeler=0
aantalComputer=0

if [ $# -ne 3 ]
then
	echo "Gelieve exact 3 parameters mee te geven"
	exit
fi

if [ $min -lt 0 ]
then
	echo "Eerste parameter moet groter of gelijk aan 0 zijn"
	exit
fi

if [ $min -ge $max ]
then
	echo "Eerste parameter moet kleiner zijn dan de tweede"
	exit
fi

if [ $aantalRondes -le 0 ]
	then
	echo "Derde parameter moet groter zijn dan 0"
exit
fi

echo "Welkom bij Hoger-Lager voor getallen van $min tot en met $max. We spelen $aantalRondes rondes"

for ((i=0; $i < $aantalRondes; i++))
do
	randomComputer=$(( RANDOM % (max - min) + min ))
	randomSpeler=$(( RANDOM % (max - min) + min ))
	read -p "De computer speelt $randomComputer. Gooi jij hoger (J/N)? " invoer

	while [ $invoer != "J" -a $invoer != "N" ]
	do
		echo "Gooi jij hoger (J/N)? "
		read invoer
	done

	if [ $randomComputer -eq $randomSpeler ]
	then
		gewonnenComputer=true
	else
		if [ $randomComputer -gt $randomSpeler ]
		then
			if [ $invoer = "N" ]
			then
				gewonnenSpeler=true
				aantalSpeler=$((aantalSpeler+1))
			else
				gewonnenComputer=true
				aantalComputer=$((aantalComputer+1))
			fi
		elif [ $randomComputer -lt $randomSpeler ]
		then
			if [ $invoer = "N" ]
			then
				gewonnenComputer=true
				aantalComputer=$((aantalComputer+1))
			else
				gewonnenSpeler=true
				aantalSpeler=$((aantalSpeler+1))
			fi
		fi
	fi
	echo "Jij gooit $randomSpeler. "

	if [ $gewonnenComputer = true ]
	then
		echo "Computer wint ronde!"
	elif [ $gewonnenSpeler = true ]
	then
		echo "Speler wint ronde!"
	fi
	gewonnenComputer=false
	gewonnenSpeler=false
done

echo -n "Speler wint $aantalSpeler keer, computer wint $aantalComputer keer, " 
if [ $aantalSpeler -eq $aantalComputer ]
then
	echo "gelijkstand!"
elif [ $aantalSpeler -lt $aantalComputer ]
then
	echo "computer wint!"
else
	echo "speler wint!"
fi
