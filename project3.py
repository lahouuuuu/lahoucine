# projet 3 : Vérificateur de force de mot de passe simple

# Phase 1 : Saisie et analyse du mot de passe
mot_de_passe = input("Entrez votre mot de passe : ")

# on mesure la longueur du mot de passe avec len()
longueur_pwd = len(mot_de_passe)

# ici la condition a_un_chiffre est de base placé à false puis on dit si mot de passe contient un chiffre entre 0 et 9 on passe la condition a vrai
a_un_chiffre = False
for caractere in mot_de_passe:
    if caractere in "0123456789":
        a_un_chiffre = True

#on fait en sorte à ce que la longueur du mdp soit > 8 charactères
assez_long = longueur_pwd >= 8
#on vérifie les 2 conditions en meme temps
mot_de_passe_fort = assez_long and a_un_chiffre

# on affiche les résultats en fonction des conditions
print(f"Longueur d’au moins 8 caractères : {'merci !' if assez_long else 'pas assez long !'}")
print(f"Contient au moins un chiffre : {'merci !' if a_un_chiffre else 'ajoutez un chiffre au minimum'}")

# conclusion finale
if mot_de_passe_fort:
    print(" Votre mot de passe est bon !")
else:
    print("Votre mot de passe n'est pas assez sécurisé.")
