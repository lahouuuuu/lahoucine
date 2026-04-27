using UnityEngine;

public class PhoneInteraction : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource ringSource;   // Le lecteur de la sonnerie
    public AudioSource voiceSource;  // Le lecteur de la voix

    [Header("Mécanique")]
    public CoilHeadAI monsterAI;     // Le script du monstre à réveiller
    public float interactDistance = 3f;
    public GameObject interactUI;    // Le texte "Appuyez sur E"

    private bool isRinging = true;
    private bool callStarted = false;
    private Transform player;

    void Start()
    {
        // On trouve le joueur automatiquement
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // On lance la sonnerie dès le début du niveau
        if (ringSource != null) ringSource.Play();
    }

    void Update()
    {
        // On calcule la distance entre le joueur et la cabine
        float dist = Vector3.Distance(transform.position, player.position);

        // Affiche ou cache le texte selon la distance
        if (dist <= interactDistance && !callStarted) 
        {
            if (interactUI != null) interactUI.SetActive(true);
            
            // Si le joueur est proche et appuie sur E
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCall();
            }
        }
        else 
        {
            if (interactUI != null) interactUI.SetActive(false);
        }

        // Si le call est en cours, on vérifie quand il se termine
        if (callStarted && voiceSource != null && !voiceSource.isPlaying)
        {
            FinishGameStart();
        }
    }

    void StartCall()
    {
        callStarted = true;
        if (ringSource != null) ringSource.Stop(); // Coupe la sonnerie
        if (voiceSource != null) voiceSource.Play(); // Lance la voix du Phone Guy
        if (interactUI != null) interactUI.SetActive(false); // Cache le texte
    }

    void FinishGameStart()
    {
        // On donne le feu vert au monstre et au chrono !
        if (monsterAI != null) monsterAI.gameStarted = true;
        
        // On désactive ce script pour qu'il ne serve plus
        this.enabled = false; 
    }
}