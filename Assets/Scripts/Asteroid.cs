// --- SCRIPT: Asteroid.cs ---
// Attach this script to your Asteroid Prefab.

using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float minSpeed = 1f; // Minimum movement speed
    public float maxSpeed = 3f; // Maximum movement speed
    public float rotationSpeed = 50f; // Speed of rotation

    private Vector2 moveDirection; // Direction the asteroid will move
    private float currentSpeed; // Actual speed of this specific asteroid

    void Start()
    {
        // Randomly choose a direction for the asteroid to move
        // Ensure asteroids move towards the player's view or across the screen
        // For a simple example, let's make them move left
        moveDirection = new Vector2(Random.Range(-1f, -0.5f), Random.Range(-0.5f, 0.5f)).normalized; // Normalized for consistent speed

        // Randomly choose a speed within the defined range
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        // Optional: Random initial rotation
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    void Update()
    {
        // Move the asteroid
        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

        // Rotate the asteroid
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Destroy asteroid if it moves off-screen to prevent memory leaks
        // This requires knowing screen bounds, which is more advanced.
        // For now, a simple example: destroy if X position is too far left
        if (transform.position.x < -15f) // Adjust this value based on your camera view
        {
            Destroy(gameObject);
        }
    }
}
