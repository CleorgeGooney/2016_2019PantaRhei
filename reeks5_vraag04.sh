echo "Naam van het script: $0"
echo "# parameters: $#"
declare -a parameterArray
i=0
for parameter in "$@"
do
	parameterArray[i]="$parameter"
	i=$((i + 1))
done

for (( i=1; i < ${#parameterArray[@]}; i=$((i + 2)) ))
do
	echo ${parameterArray[i]}
done
