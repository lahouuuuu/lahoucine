using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class CoilHeadAI : MonoBehaviour
{
    [Header("État du Jeu")]
    public bool gameStarted = false; // NOUVEAU : La fameuse variable qui manquait !

    [Header("Cibles")]
    public Transform player;
    public Camera playerCamera;
    
    [Header("Déplacement")]
    public float sprintSpeed = 12f;
    public float roamSpeed = 3.5f; 
    public float roamRadius = 15f; 

    [Header("Vision du Monstre")]
    public float visionAngle = 45f; 
    public float visionDistance = 20f; 
    
    [Header("Paramètres du Malaise")]
    public float timeToDance = 10f; 
    public float rotationSpeed = 5f; 

    [Header("Mode Rage (3 minutes)")]
    public float timeBeforeRage = 180f; 
    public float musicDuration = 43f; 
    public float rageSprintSpeed = 25f; 
    
    [Header("Sons (À Remplir)")]
    public AudioSource footstepSource; 
    public AudioSource musicSource;    
    public AudioSource warningSource;  
    public AudioSource deathSource;    
    public AudioSource musicBoxSource; 
    public float warningDistance = 3f;

    [Header("UI (Interface)")]
    public TextMeshProUGUI timerText; 

    private NavMeshAgent agent;
    private Animator anim; 
    private bool isChasing = false; 
    private float stareTimer = 0f; 
    private float globalTimer = 0f; 
    
    private bool isEnraged = false; 
    private bool isPreparingRage = false; 
    private bool warningPlayed = false; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = roamSpeed;
        anim = GetComponentInChildren<Animator>();

        // Fige le monstre et le chrono au lancement du jeu
        if (!gameStarted)
        {
            agent.isStopped = true;
            if (timerText != null) timerText.text = "03:00";
        }
    }

    void Update()
    {
        // SI LE JEU N'A PAS COMMENCÉ (TANT QU'ON A PAS RÉPONDU), ON NE FAIT RIEN D'AUTRE
        if (!gameStarted) return;

        // 1. GESTION DU MODE RAGE ET DU CHRONO SYNCHRONISÉ
        if (!isEnraged)
        {
            globalTimer += Time.deltaTime;
            float timeLeft = timeBeforeRage - globalTimer;
            if (timeLeft < 0) timeLeft = 0;

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timeLeft / 60);
                int seconds = Mathf.FloorToInt(timeLeft % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                if (timeLeft <= musicDuration) timerText.color = Color.red;
                else timerText.color = Color.white;
            }
            
            if (timeLeft <= musicDuration && !isPreparingRage)
            {
                isPreparingRage = true;
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                
                if (musicSource != null) musicSource.Stop(); 
                if (footstepSource != null) footstepSource.Pause();
                if (anim != null) { anim.speed = 0f; anim.SetBool("IsDancing", false); anim.SetBool("IsWalking", false); }
                
                if (musicBoxSource != null) musicBoxSource.Play(); 
            }

            if (timeLeft <= 0 && !isEnraged)
            {
                isEnraged = true;
                isPreparingRage = false;
                if (timerText != null) timerText.text = "RUN.";
            }
        }

        // 2. COMPORTEMENT EN MODE RAGE
        if (isEnraged)
        {
            agent.isStopped = false;
            agent.speed = rageSprintSpeed;
            agent.SetDestination(player.position); 
            
            if (anim != null) { anim.speed = rageSprintSpeed / roamSpeed; anim.SetBool("IsWalking", true); anim.SetBool("IsDancing", false); }
            if (footstepSource != null && agent.velocity.magnitude > 0.1f)
            {
                if (!footstepSource.isPlaying) footstepSource.Play();
                footstepSource.pitch = agent.velocity.magnitude / roamSpeed;
            }
            return; 
        }

        if (isPreparingRage) return;

        // --- 3. CODE NORMAL ---
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (!warningPlayed && distToPlayer <= warningDistance)
        {
            if (warningSource != null) warningSource.Play();
            warningPlayed = true; 
        }
        else if (warningPlayed && distToPlayer > warningDistance * 1.5f)
        {
            warningPlayed = false; 
        }

        if (footstepSource != null)
        {
            if (agent.velocity.magnitude > 0.1f && !agent.isStopped)
            {
                if (!footstepSource.isPlaying) footstepSource.Play();
                footstepSource.pitch = agent.velocity.magnitude / roamSpeed;
            }
            else
            {
                if (footstepSource.isPlaying) footstepSource.Pause();
            }
        }

        if (IsLookedAt())
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero; 

            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0; 
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            stareTimer += Time.deltaTime;

            if (stareTimer >= timeToDance)
            {
                if (anim != null) { anim.speed = 1f; anim.SetBool("IsDancing", true); anim.SetBool("IsWalking", false); }
                if (musicSource != null && !musicSource.isPlaying) musicSource.Play();
            }
            else
            {
                if (anim != null) { anim.speed = 0f; anim.SetBool("IsDancing", false); }
            }
            return; 
        }

        stareTimer = 0f; 
        agent.isStopped = false;
        
        if (musicSource != null && musicSource.isPlaying) musicSource.Stop();

        if (CanSeePlayer()) isChasing = true; 

        if (isChasing)
        {
            agent.speed = sprintSpeed;
            agent.SetDestination(player.position);
            if (anim != null) { anim.SetBool("IsDancing", false); anim.SetBool("IsWalking", true); anim.speed = sprintSpeed / roamSpeed; }
            if (Vector3.Distance(transform.position, player.position) > visionDistance * 1.5f) isChasing = false; 
        }
        else
        {
            agent.speed = roamSpeed;
            if (anim != null) { anim.SetBool("IsDancing", false); anim.SetBool("IsWalking", true); anim.speed = 1f; }
            if (!agent.pathPending && agent.remainingDistance < 2f) RoamToNewPosition();
        }
    }

    void RoamToNewPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1)) agent.SetDestination(hit.position); 
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle < visionAngle && directionToPlayer.magnitude < visionDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out hit, visionDistance))
            {
                if (hit.transform == player) return true; 
            }
        }
        return false;
    }

    bool IsLookedAt()
    {
        Vector3 screenPoint = playerCamera.WorldToViewportPoint(transform.position + Vector3.up);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (onScreen)
        {
            Vector3 directionToPlayer = (transform.position + Vector3.up) - playerCamera.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, directionToPlayer, out hit))
            {
                if (hit.transform == this.transform) return true;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            agent.isStopped = true;
            if (anim != null) anim.speed = 0f; 
            if (musicSource != null) musicSource.Stop(); 
            if (footstepSource != null) footstepSource.Stop(); 
            
            if (deathSource != null) deathSource.Play();
            
            this.enabled = false; 

            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                playerScript.Die(pushDirection);
            }
        }
    }
}