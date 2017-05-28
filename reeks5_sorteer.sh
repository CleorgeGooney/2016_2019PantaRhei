if [ $# -eq 2 -o $# -eq 3 ]
then
	if [ -f $1 ]
	then
		if [ $2 = "A" -o $2 = "O" ]
		then
			if [ -z $3 ]
			then
			aantalRegels=20
			elif [ $3 -gt 0 ]
			then
			aantalRegels=$3
			else
			echo "Aantal regels moet hoger zijn dan 0."
			exit
			fi
			#instructies
				if [ $2 = "A" ]
				then
				sort $1 | head -$aantalRegels
				else
				sort $1 | head -$aantalRegels | tac
				fi
		else
		echo "Geen juiste richting meegegeven"
		exit
		fi
	else
	echo "Geen bestand meegegeven"
	exit
	fi

else
echo -e "\033[1msorteer [FILE] [RICHTING] [AANTAL_REGELS]\033[0m
FILE		bestand dat gesorteerd zal worden  
RICHTING	de richting waarin gesorteerd wordt (O of A)
AANTAL_REGELS	het aantal regels dat getoond wordt na sorteren (default 20)"
fi

