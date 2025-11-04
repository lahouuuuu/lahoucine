# projet 2 : Planificateur de budget

print("PLANIFICATEUR DE BUDGET")

# on récupère les info avec des inputs
distance_km = float(input("Quelle est la distance aller-retour du voyage (km) ? "))
consomation = float(input("Quelle est la consommation de la voiture (en litres / 100 km) ? "))
prix_littre = float(input("Quel est le prix actuel du litre de carburant (€) ? "))

# phase 2 : Calcul de la consomation et du prix
carburant_besoin = (distance_km / 100) * consomation
prix_total = carburant_besoin * prix_littre
prix_total = round(prix_total, 2)  # on arrondit à 2 chiffres après la virgule

# phase 3 : Affichage
print(f"Distance totale du voyage : {distance_km} km")
print(f"Consommation totale estimée : {carburant_besoin:.2f} litres")
print(f"Coût total estimé du carburant : {prix_total:.2f} €")
