if [ $# -eq 0 ]
then
	read -p "Geef een bronbestand in... " bronbestand
	while [[ ! -f $bronbestand ]]
	do
	read -p "Dit is geen bestand, geef een correct bronbestand in... " bronbestand
	done

	read -p "Geef een doelmap in... " doelmap
	while [[ ! -d $doelmap ]]
	do
	read -p "Dit is geen map, geef een correct map in... " doelmap
	done

	cp -t $doelmap $bronbestand
	echo "$bronbestand gekopieerd naar $doelmap!"
	
else
echo "Er mag geen parameter meegegeven worden!"
exit
fi
