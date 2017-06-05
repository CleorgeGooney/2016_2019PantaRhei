declare -a array

for ((i=0 ; i<100 ; i++))
do
	array[$i]="N"
done

for ((j=0 ; j <100 ; j++))
do
	for ((k=0 ; k<100 ; k++))
	do
		deeltal=$((k+1))
		deler=$((j+1))
		waarde=$((deeltal%deler))
		if [ $waarde -eq 0 ]
		then
			if [ ${array[$k]} = "N" ]
			then
				array[$k]="J"
			else
				array[$k]="N"
			fi
		fi
	done
	echo "Situatie na ronde $((j+1)): ${array[@]}"
done

declare -a indexenMetJ

for ((i=0 ; i<100 ; i++))
do
	if [ ${array[$i]} = "J" ]
	then
		positie=`expr $i + 1`
		indexenMetJ+=($positie)
	fi
done

echo -n "Posities met J: "

for elem in "${indexenMetJ[@]}"
do
	echo -n "$elem"
	echo -n " "
	
done

echo
