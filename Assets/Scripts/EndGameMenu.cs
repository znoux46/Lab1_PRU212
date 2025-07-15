// --- SCRIPT: EndGameMenu.cs ---
// Attach this script to a GameObject in your End Game Scene (e.g., a UI Manager object).

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    public TMP_Text finalScoreText; // Assign your UI Text object for final score

    void Start()
    {
        // Retrieve the final score saved by GameManager
        int score = PlayerPrefs.GetInt("FinalScore", 0); // 0 is default if key not found
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score;
        }
    }

    // Called when the "Return to Main Menu" button is clicked
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene"); // Load the Main Menu Scene
    }

    // Called when the "Quit Game" button is clicked
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game from End Game Scene...");
    }
}
