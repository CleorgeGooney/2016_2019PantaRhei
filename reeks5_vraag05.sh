resultaat=2

if [ $1 -eq 0 ]
	then
	resultaat=0

elif [ $1 -lt 0 ]
	then
	abs=$((-$1))
	for (( i=1; i < $abs; i++ ))
	do
		resultaat=$((resultaat*2))
	done
	resultaat=($(bc -l <<< "1 / $resultaat"))

else
	for (( i=1; i < $1; i++ ))
	do
		resultaat=$((resultaat*2))
	done
fi

echo 2 tot de macht $1 = $resultaat
