IFS=" "
datum=$(date)
count=0
for element in $datum
do
count=$((count+1))
echo "$element"
done
echo "
Aantal elementen in date commando: $count"
