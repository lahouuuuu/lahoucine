using UnityEngine;
using TMPro;

public class ExitDoor : MonoBehaviour
{
    [Header("Paramètres")]
    public int requiredDisks = 10;
    public string winMessage = "VOUS AVEZ SURVÉCU !";

    [Header("UI")]
    public TextMeshProUGUI interactionText; // On réutilise ton texte d'interaction

    private bool canExit = false;
    private PlayerInteraction playerInteraction;

    void Start()
    {
        // On va chercher le script de collecte sur le joueur
        playerInteraction = FindAnyObjectByType<PlayerInteraction>();
    }

    void Update()
    {
        // On vérifie en temps réel si le joueur a ramassé assez de disques
        if (playerInteraction != null && playerInteraction.GetCollectedCount() >= requiredDisks)
        {
            canExit = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canExit)
            {
                // Action de victoire (on peut charger une scène ou afficher un menu)
                Debug.Log("VICTOIRE !");
                ShowWinScreen();
            }
            else
            {
                // Message si la porte est verrouillée
                if (interactionText != null)
                {
                    interactionText.gameObject.SetActive(true);
                    interactionText.text = "Porte verrouillée. Il manque des disques.";
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void ShowWinScreen()
    {
        // Pour l'instant, on affiche juste un message, on fera le menu après
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true);
            interactionText.text = winMessage;
            Time.timeScale = 0; // Figurer le jeu à la victoire
        }
    }
}