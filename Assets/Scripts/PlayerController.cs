// --- SCRIPT: PlayerController.cs ---
// Attach this script to your Spaceship GameObject.

using UnityEngine;
using UnityEngine.SceneManagement; // For scene transitions (EndGame)

public class PlayerController : MonoBehaviour
{
    // Public variables for easy adjustment in the Unity Inspector
    public float moveSpeed = 5f; // Speed of the spaceship
    public GameObject laserPrefab; // Assign your Laser Prefab in the Inspector
    public float fireRate = 0.5f; // Time between laser shots
    public float collisionPenalty = 10f; // Points deducted on asteroid collision

    private float nextFireTime; // To control firing rate

    // Reference to the GameManager to update score
    private GameManager gameManager;

    void Start()
    {
        // Find the GameManager object in the scene and get its component
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene! Make sure it exists.");
        }
        Debug.Log("PlayerController.cs Start() called."); // Thêm log
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        // Handle player movement
        HandleMovement();

        // Handle player shooting
        HandleShooting();
    }

    void HandleMovement()
    {
        // Get input from arrow keys (Horizontal: Left/Right, Vertical: Up/Down)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);

        // Move the spaceship using Transform.Translate
        // Time.deltaTime ensures movement is frame-rate independent
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Optional: Clamp player position to screen boundaries (more advanced, omitted for simplicity)
        // You might want to get camera bounds and restrict transform.position.
    }

    void HandleShooting()
    {
        // Check if Space key is pressed and enough time has passed since last shot
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate; // Set next allowed fire time

            // Instantiate a laser at the spaceship's position and rotation
            // Quaternion.identity means no rotation
            Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, 90));

            // Optional: Play a sound effect for shooting
            // GetComponent<AudioSource>().PlayOneShot(shootSound);
        }
    }

    // Called when the spaceship collides with another 2D Collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is an Asteroid
        if (other.CompareTag("Asteroid"))
        {
            //// Deduct points
            //if (gameManager != null)
            //{
            //    gameManager.AddScore(-Mathf.Abs(Mathf.RoundToInt(collisionPenalty))); // Deduct a positive value
            //    Debug.Log("Collided with Asteroid! Score: " + gameManager.CurrentScore);
            //}
            //// Destroy the asteroid after collision
            //Destroy(other.gameObject);

            //// Optional: Play explosion sound/animation
            //// Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);

            //// If game ends on single collision, transition to End Game Scene
            //// For this basic example, we just deduct points.
            //// If the score drops too low, you might want to end the game.
            //if (gameManager != null && gameManager.CurrentScore <= -100) // Example threshold
            //{
            gameManager.EndGame();
            //}
        }
        // Check if the collided object is a Star
        else if (other.CompareTag("Star"))
        {
            // Add points
            if (gameManager != null)
            {
                gameManager.AddScore(10); // Each star gives 10 points
                Debug.Log("Collected Star! Score: " + gameManager.CurrentScore);
            }
            // Destroy the star after collection
            Destroy(other.gameObject);

            // Optional: Play collection sound effect
        }
    }
}
