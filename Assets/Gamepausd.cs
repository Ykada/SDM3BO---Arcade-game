using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Add this namespace to load scenes for restarting

public class Gamepausd : MonoBehaviour
{
    // Reference to the UI Panel
    public GameObject pauseMenuUI;

    // Reference to the buttons
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    // Keep track of whether the game is paused or not
    private bool isGamePaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden at the start
        pauseMenuUI.SetActive(false);

        // Add listeners to the buttons
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                // Close the game entirely
                QuitGame();
            }
            else
            {
                // Show the pause menu and freeze the game
                ShowPauseMenu();
            }
        }
    }

    void ShowPauseMenu()
    {
        // Activate the pause menu
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        // Freeze the game by setting time scale to 0
        Time.timeScale = 0;

        // Set the game as paused
        isGamePaused = true;
    }

    void ResumeGame()
    {
        // Deactivate the pause menu
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        // Unfreeze the game by setting time scale back to 1
        Time.timeScale = 1;

        // Set the game as unpaused
        isGamePaused = false;
    }

    void RestartGame()
    {
        // Restart the game by loading the current scene
        Time.timeScale = 1;  // Ensure the game is unpaused before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reloads the current scene
    }

    void QuitGame()
    {
        // Quit the game entirely
#if UNITY_EDITOR
        // If in the Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If built, quit the application
        Application.Quit();
#endif
    }
}
