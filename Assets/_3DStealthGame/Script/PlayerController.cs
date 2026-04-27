using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private CapsuleCollider col; // Permet de mesurer tes jambes automatiquement

    [Header("Animation")]
    public Animator animator;

    [Header("Caméra")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>(); // On récupère ton collider
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- ROTATION SOURIS ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // --- ANIMATION ---
        bool isMoving = Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) ||
                        Input.GetKey(KeyCode.S) ||
                        Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.D);

        if (animator != null)
        {
            animator.SetBool("IsWalking", isMoving);
        }

        // --- SAUT ---
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            // On remet la vitesse verticale à zéro pour que le saut soit toujours le même
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float moveH = 0;
        float moveV = 0;

        // DÉTECTION TOUCHES
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W)) moveV = 1;
        else if (Input.GetKey(KeyCode.S)) moveV = -1;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A)) moveH = -1;
        else if (Input.GetKey(KeyCode.D)) moveH = 1;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        Vector3 movement = (transform.right * moveH + transform.forward * moveV).normalized;

        rb.linearVelocity = new Vector3(movement.x * currentSpeed, rb.linearVelocity.y, movement.z * currentSpeed);
    }

    // --- LA VÉRIFICATION DU SOL AUTOMATIQUE ---
    bool IsGrounded()
    {
        if (col == null) return false;

        // On calcule la distance exacte entre le centre de ton joueur et le bout de ses pieds
        float distanceToFeet = col.bounds.extents.y;

        // On tire un rayon depuis le centre, jusqu'aux pieds + 10 centimètres de marge
        bool grounded = Physics.Raycast(col.bounds.center, Vector3.down, distanceToFeet + 0.1f);

        // BONUS : On dessine le rayon pour toi ! 
        // Si tu regardes la vue "Scene" pendant que tu joues, tu verras une ligne sous tes pieds (Verte = Sol, Rouge = En l'air)
        Debug.DrawRay(col.bounds.center, Vector3.down * (distanceToFeet + 0.1f), grounded ? Color.green : Color.red);

        return grounded;
    }

    // --- SYSTÈME DE MORT ---
    public void Die(Vector3 impactDirection)
    {
        // 1. On coupe l'animation si tu en as une
        if (animator != null)
        {
            animator.enabled = false;
        }

        // 2. On débloque toutes les rotations du corps (le Faux Ragdoll)
        rb.constraints = RigidbodyConstraints.None;

        // 3. On applique une grosse force pour te faire tomber en arrière
        rb.AddForce(impactDirection * 15f + Vector3.up * 5f, ForceMode.Impulse);

        // NOUVEAU : On prévient le GameManager que tu es mort pour lancer les 2 secondes d'attente !
        FindAnyObjectByType<GameManager>().ShowGameOver();

        // 4. On détache la caméra de la souris pour qu'elle tombe avec la tête
        this.enabled = false; // Désactive ce script (tu ne peux plus bouger ni regarder)
    }
}