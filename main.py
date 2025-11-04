# projet 1 : Calculateur de pourboire

print("Voici le calculateur de pourboire ")

# on demande la facture à l'utilisateur avec le input, elle sera stocké sous forme de float
facture_total = float(input("de combien est votre facture ? "))

# on demande combien de pourboire (en %), ici on stock la valeur sous forme d'entier, donc pas de "%" à virgule
pourboire = int(input("en pourcentage, combien de pourboire allez-vous laisser ? "))

# combien de personnes vont partager la note
nbre_personne = int(input("Combien de personnes partagent la facture ? "))

# on calcule
montant_pourboire = facture_total * (pourboire / 100)

# on additionne le pourboir total au montant de la facture
pourboire_total = facture_total + montant_pourboire

# division du total entre le nombre de personnes
amount_per_person = pourboire_total / nbre_personne

# on arrondit à 2 chiffres après la virgule grace à "round(... , 2)"
montant_final_par_personne = round(amount_per_person, 2)

# on affiche le résultat à l'aide de placeholders qui récupèrent les info stockées
# on dit aussi au programme d'afficher un float avec 2 chiffres après la virgule  ":.2f"
print(f"Montant total avec pourboire : {pourboire_total:.2f} €")
print(f"Chaque personne doit payer : {montant_final_par_personne:.2f} €")
print(f"(facture de {facture_total:.2f} € avec un pourboire de {pourboire}% partagé entre {nbre_personne} personnes)")
nbre_personne
