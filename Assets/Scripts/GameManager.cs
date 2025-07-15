// --- SCRIPT: GameManager.cs ---
// Create an empty GameObject in your Gameplay scene and name it "GameManager".
// Attach this script to the "GameManager" GameObject.
// This script will manage game state, score, and scene transitions.

using UnityEngine;
using UnityEngine.UI; // For UI elements like Text
using UnityEngine.SceneManagement; // For scene management
using TMPro; // Add this for TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern to easily access GameManager

    [Header("UI References (Assign in Inspector if preferred, or ensure names match for auto-find)")]
    public TextMeshProUGUI scoreText; // Change from Text to TextMeshProUGUI if using TextMeshPro
    public GameObject pausePanel; // Assign your Pause UI Panel in the Inspector

    public float asteroidSpawnInterval = 2f; // How often asteroids spawn
    public float starSpawnInterval = 5f; // How often stars spawn

    public GameObject asteroidPrefab; // Assign your Asteroid Prefab
    public GameObject starPrefab;     // Assign your Star Prefab

    public int initialScore = 0; // Starting score

    private int currentScore;
    public int CurrentScore
    {
        get { return currentScore; }
        private set
        {
            currentScore = value;
            UpdateScoreUI(); // Update UI whenever score changes
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // IMPORTANT: We use DontDestroyOnLoad to persist GameManager across scenes.
            // This requires re-finding UI references when GameplayScene is loaded.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManagers
            return; // Exit Awake to prevent further execution on a destroyed object
        }
        Debug.Log("GameManager.cs Awake() called. Instance: " + (Instance != null ? "Assigned" : "Destroyed Duplicate"));
    }

    void OnEnable()
    {
        // Subscribe to sceneLoaded event to re-assign UI references when a new scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("GameManager.cs OnEnable() called. Subscribed to sceneLoaded event.");
    }

    void OnDisable()
    {
        // Unsubscribe from sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("GameManager.cs OnDisable() called. Unsubscribed from sceneLoaded event.");
    }

    // This method is called every time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called. Scene: " + scene.name + ", Mode: " + mode);

        // Only re-assign UI references if we are in the GameplayScene
        if (scene.name == "GameplayScene")
        {
            Debug.Log("GameplayScene loaded. Attempting to re-assign UI references for GameManager.");

            // Attempt to find ScoreText
            // PRIORITIZE Inspector assignment. If null, then try to find by name.
            if (scoreText == null) // Only attempt to find if not already assigned in Inspector
            {
                GameObject scoreTextObj = GameObject.Find("ScoreText");
                if (scoreTextObj != null)
                {
                    scoreText = scoreTextObj.GetComponent<TextMeshProUGUI>();
                    if (scoreText != null) Debug.Log("ScoreText found and re-assigned via Find.");
                }
            }
            if (scoreText == null) Debug.LogError("ScoreText GameObject not found or TextMeshProUGUI component missing in GameplayScene! Please name your score TextMeshProUGUI 'ScoreText' in Hierarchy, or assign it in Inspector.");

            // Attempt to find PausePanel
            // PRIORITIZE Inspector assignment. If null, then try to find by name.
            if (pausePanel == null) // Only attempt to find if not already assigned in Inspector
            {
                pausePanel = GameObject.Find("PausePanel");
                if (pausePanel != null) Debug.Log("PausePanel found and re-assigned via Find.");
            }
            if (pausePanel == null) Debug.LogError("PausePanel GameObject not found in GameplayScene! Please name your Pause Panel 'PausePanel' in Hierarchy, or assign it in Inspector.");

            // IMPORTANT: Re-assign button OnClick events for Pause Panel buttons
            if (pausePanel != null)
            {
                // Find Resume Button
                Button resumeButton = pausePanel.transform.Find("ResumeButton")?.GetComponent<Button>();
                if (resumeButton != null)
                {
                    resumeButton.onClick.RemoveAllListeners(); // Clear existing listeners to prevent duplicates
                    resumeButton.onClick.AddListener(() => ResumeGame()); // Add new listener
                    Debug.Log("ResumeButton OnClick re-assigned.");
                }
                else
                {
                    Debug.LogError("ResumeButton not found as child of PausePanel! Ensure it's named 'ResumeButton'.");
                }

                // Find End Game From Pause Button
                Button endGameButton = pausePanel.transform.Find("EndGameFromPauseButton")?.GetComponent<Button>();
                if (endGameButton != null)
                {
                    endGameButton.onClick.RemoveAllListeners(); // Clear existing listeners
                    endGameButton.onClick.AddListener(() => ReturnToMainMenuFromPause()); // Add new listener
                    Debug.Log("EndGameFromPauseButton OnClick re-assigned.");
                }
                else
                {
                    Debug.LogError("EndGameFromPauseButton not found as child of PausePanel! Ensure it's named 'EndGameFromPauseButton'.");
                }
            }


            // Initialize score and UI state for the newly loaded GameplayScene
            CurrentScore = initialScore; // Reset score for new game
            if (pausePanel != null)
            {
                pausePanel.SetActive(false); // Make sure pause panel is hidden at start
                Debug.Log("Pause Panel set to inactive after scene load.");
            }
            else
            {
                Debug.LogWarning("Pause Panel reference is null after scene load, cannot set inactive. This might be okay if it was assigned in Inspector.");
            }

            // Ensure Time.timeScale is 1.0 when GameplayScene is loaded
            Time.timeScale = 1f;
            Debug.Log("Time.timeScale set to 1f in OnSceneLoaded for GameplayScene.");

            // Start spawning only if this is a fresh start of Gameplay (not just re-enabling an existing scene)
            CancelInvoke(); // Cancel any existing invokes from previous games
            InvokeRepeating("SpawnAsteroid", 1f, asteroidSpawnInterval);
            InvokeRepeating("SpawnStar", 3f, starSpawnInterval);
            Debug.Log("Spawning initialized in GameplayScene.");

        }
        else if (scene.name == "MainMenuScene" || scene.name == "EndGameScene")
        {
            // For other scenes, ensure Time.timeScale is normal
            Time.timeScale = 1f;
            Debug.Log($"Time.timeScale set to 1f for {scene.name}.");
            // Also, clear any active InvokeRepeating if the game isn't running (e.g., if you return to main menu from pause)
            CancelInvoke();
        }
    }


    void Start()
    {
        // The main initialization for GameplayScene will now happen in OnSceneLoaded.
        // This Start() method will only be called once when GameManager is first created.
        // We will keep general setup for GameManager here, not scene-specific UI setup.
        Debug.Log("GameManager.cs Start() called. (This runs only once when GameManager is first created, then OnSceneLoaded handles scene-specific setup).");

        // The initial assignment in Inspector should happen here, for first time load
        // But re-assignments are handled by OnSceneLoaded
        // We call UpdateScoreUI here to ensure initial score is displayed if UI is already assigned at GameManager creation.
        UpdateScoreUI(); // Update initial score display
    }

    void Update()
    {
        // Xử lý nút Pause (Esc key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed. Current Time.timeScale: " + Time.timeScale + ". Active Pause Panel: " + (pausePanel != null ? pausePanel.activeSelf.ToString() : "NULL"));
            if (Time.timeScale == 1f) // Nếu game đang chạy
            {
                PauseGame();
            }
            else // Nếu game đang tạm dừng
            {
                ResumeGame();
            }
        }
    }

    void SpawnAsteroid()
    {
        // Define spawn area (adjust based on your camera view)
        // Spawn slightly off-screen to the right
        float spawnX = 10f; // Example X position
        float spawnY = Random.Range(-5f, 5f); // Random Y position
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnStar()
    {
        // Define spawn area
        float spawnX = Random.Range(-8f, 8f); // Random X position
        float spawnY = Random.Range(-4f, 4f); // Random Y position
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        Instantiate(starPrefab, spawnPosition, Quaternion.identity);
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + CurrentScore;
        }
        else
        {
            Debug.LogError("Score Text is null when trying to update UI. Check GameManager's UI reference. Is it named 'ScoreText' in GameplayScene?");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Dừng mọi thứ trong game
        if (pausePanel != null)
        {
            pausePanel.SetActive(true); // Hiển thị Pause Panel
            Debug.Log("Pause Panel set to active.");
        }
        else
        {
            Debug.LogError("Cannot show Pause Panel: reference is null in PauseGame(). Please check if PausePanel GameObject exists in GameplayScene and is assigned/named correctly.");
        }
        Debug.Log("Game Paused. Time.timeScale = 0.");
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // Ẩn Pause Panel
            Debug.Log("Pause Panel set to inactive.");
        }
        else
        {
            Debug.LogError("Cannot hide Pause Panel: reference is null in ResumeGame(). Please check if PausePanel GameObject exists in GameplayScene and is assigned/named correctly.");
        }
        Time.timeScale = 1f; // Tiếp tục game
        Debug.Log("Game Resumed. Time.timeScale = 1.");
    }

    // Hàm này được gọi từ nút "End Game" trong Pause Menu
    public void ReturnToMainMenuFromPause()
    {
        Debug.Log("Returning to Main Menu from Pause. Resetting Time.timeScale.");
        Time.timeScale = 1f; // Đảm bảo Time.timeScale được reset trước khi chuyển cảnh
        // Đảm bảo ẩn panel trước khi chuyển scene nếu nó đang hiển thị
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        SceneManager.LoadScene("MainMenuScene");
    }

    public void EndGame()
    {
        // Save final score (e.g., using PlayerPrefs or passing to EndGameScene)
        PlayerPrefs.SetInt("FinalScore", CurrentScore);
        Debug.Log("GameManager.cs EndGame() called. Loading EndGameScene.");

        // Tạm dừng game khi kết thúc để tránh mọi tương tác không mong muốn
        Time.timeScale = 0f; // Đảm bảo game dừng lại
        Debug.Log("Time.timeScale set to 0f in GameManager.cs EndGame().");

        SceneManager.LoadScene("EndGameScene"); // Load the End G
    }
}