// --- SCRIPT: MainMenu.cs ---
// Attach this script to a GameObject in your Main Menu Scene (e.g., a UI Manager object).
// This script assumes you have UI Buttons.

using UnityEngine;
using UnityEngine.SceneManagement; // For scene transitions
using UnityEngine.UI; // For Button and Panel

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsPanel; // Assign your Instructions UI Panel in the Inspector

    void Start()
    {
        // Ensure the instructions panel is initially hidden
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }

    // Called when the "Play" button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene("GameplayScene"); // Load the Gameplay Scene
    }

    // Called when the "Instructions" button is clicked
    public void ShowInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true); // Show the instructions panel
        }
    }

    // Called when a "Close Instructions" button (or similar) is clicked on the panel
    public void HideInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false); // Hide the instructions panel
        }
    }

    // Called when the "Quit" button is clicked
    public void QuitGame()
    {
        // This only works in a build application, not directly in the editor
        Application.Quit();
        Debug.Log("Quitting Game..."); // For testing in editor
    }
}
