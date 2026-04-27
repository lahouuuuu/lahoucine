using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Paramètres de Collecte")]
    public float interactDistance = 3f;
    public LayerMask interactableLayer;

    [Header("Interface (UI)")]
    public TextMeshProUGUI counterText;
    public GameObject interactUI;

    private int collectedDisks = 0;
    private int totalDisks = 10;
    private Camera playerCam;

    void Start()
    {
        playerCam = GetComponentInChildren<Camera>();
        if (playerCam == null) playerCam = Camera.main;

        UpdateCounterUI();
    }

    void Update()
    {
        RaycastHit hit;
        bool isLookingAtScrap = false;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, interactDistance, interactableLayer))
        {
            ScrapItem scrap = hit.collider.GetComponent<ScrapItem>();
            if (scrap != null)
            {
                isLookingAtScrap = true;

                if (interactUI != null) interactUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CollectDisk(hit.collider.gameObject);
                }
            }
        }

        if (!isLookingAtScrap && interactUI != null && interactUI.activeSelf)
        {
            PhoneInteraction phone = FindAnyObjectByType<PhoneInteraction>();
            if (phone == null || !phone.enabled)
            {
                interactUI.SetActive(false);
            }
        }
    }

    void CollectDisk(GameObject diskObj)
    {
        collectedDisks++;
        UpdateCounterUI();

        Destroy(diskObj);

        if (interactUI != null) interactUI.SetActive(false);

        if (collectedDisks >= totalDisks)
        {
            Debug.Log("10/10 ! La porte de sortie s'ouvre !");
        }
    }

    void UpdateCounterUI()
    {
        if (counterText != null)
        {
            counterText.text = collectedDisks + " / " + totalDisks + " Disques";
        }
    }

    // VOICI LA FAMEUSE FONCTION QUI MANQUAIT AU BON ENDROIT
    public int GetCollectedCount()
    {
        return collectedDisks;
    }
}