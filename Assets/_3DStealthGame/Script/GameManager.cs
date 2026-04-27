using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Indispensable pour utiliser les délais (Coroutines)

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverPanel;

    // Cette fonction est appelée quand le joueur meurt
    public void ShowGameOver()
    {
        // Au lieu d'afficher le menu tout de suite, on lance la séquence avec délai
        StartCoroutine(GameOverSequence());
    }

    // Voici la fameuse Coroutine qui permet de mettre le code en pause
    private IEnumerator GameOverSequence()
    {
        // 1. On attend 2 secondes pour laisser le joueur admirer son ragdoll
        yield return new WaitForSeconds(2f);

        // 2. Les 2 secondes sont passées, on affiche l'écran de défaite
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 3. On débloque la souris pour pouvoir cliquer
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 4. On fige enfin le temps 
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Remet le temps à la normale avant de recharger
        Time.timeScale = 1f;

        // Recharge la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}