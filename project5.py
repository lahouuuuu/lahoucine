# --- Projet 5 : Game

# Phase 1 : Collecte des perf du joueur
base_score = 1000  # score de base

# demande du nombre de pièces collectées et conversion en entier pour faire les calculs plus tard
pieces_trouvees_str = input("Combien de pièces as-tu collectées ? ")
pieces_trouvees = int(pieces_trouvees_str)

# demande si le joueur a terminé en no hit
no_hit_str = input("no hit ou pas? (oui/non) ")

no_hit_bonus = (no_hit_str.lower() == "oui") #le ".lower" transforme l'entrée en minuscule donc l'utilisateur peut écrire en majuscule

if no_hit_bonus:
    print("GG mon gars !")
else:
    print("Pas ouf tout ça...")

#calcul du score final avec bonus
coin_bonus = pieces_trouvees * 50  # Chaque pièce vaut 50 points

if no_hit_bonus:
    dmg_multiplier = 2  # score doublé si aucun dégât
else:
    dmg_multiplidmger = 1

final_score = (base_score + coin_bonus) * dmg_multiplier

# affichage
print(f"Score de base : {base_score}")
print(f"Bonus (pièces) : +{coin_bonus}")
print(f"Multiplicateur (sans dégâts) : x{dmg_multiplier}")
print(f"\nSCORE FINAL : {final_score}")
