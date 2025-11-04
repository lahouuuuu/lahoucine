#Projet 4 : ingrédients de recettte

# on demande la quantité originale de l’ingrédient (la valeure est stocké sous forme de chaine de caractères
quantite_str = input("Entrez la quantité de l’ingrédient en grammes : ")
quantite = float(quantite_str)  # On convertit de string en nombre décimal float

# on demande pour combien de personnes la recette originale est prévue
portions_str = input("Pour combien de personnes la recette est prévue ? ")
portions = int(portions_str)  # On convertit de sting en nombre entier

# on demande pour combien de personnes on veut cuisiner
portions_souhaitees_str = input("Pour combien de personnes voulez-vous cuisiner ? ")
portions_souhaitees = int(portions_souhaitees_str) #conversion semblable à la précédente pour pouvoir faire des calculs avec

# phase 2 : Calcul
# je calcule le facteur d’ajustement (le ratio entre les portions souhaitées et les portions originales)
facteur_ajustement = portions_souhaitees / portions

# ici je calcule la nouvelle quantité d’ingrédient
nouvelle_quantite = quantite * facteur_ajustement

# on arrondit à un chiffre après la virgule
nouvelle_quantite = round(nouvelle_quantite, 1)

# affichage
print(f"La recette est prévue pour {portions} personnes.")
print(f"Pour cuisiner pour {portions_souhaitees} personnes, vous aurez besoin de {nouvelle_quantite} g.")
