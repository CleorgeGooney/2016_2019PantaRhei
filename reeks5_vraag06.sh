resultaat=1
for (( i=1; i<=$1; i++ ))
do
resultaat=$((resultaat*i))
done
echo "$1! = $resultaat"
