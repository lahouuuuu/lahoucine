using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Charge la scène de jeu (qui sera la scène numéro 1)
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Le jeu se ferme !");
        Application.Quit();
    }
}