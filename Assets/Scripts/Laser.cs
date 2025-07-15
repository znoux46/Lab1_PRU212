using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserSpeed = 10f; // Speed of the laser
    public float lifetime = 2f; // How long the laser exists before being destroyed

    void Start()
    {
        // Destroy the laser after its lifetime to prevent accumulating off-screen lasers
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the laser forward (assuming it's facing up or right initially)
        transform.Translate(Vector3.right * laserSpeed * Time.deltaTime); // Or Vector3.right if laser faces right

        // Optional: Check if laser is off-screen and destroy it sooner
        if (transform.position.y > 10f || transform.position.y < -10f ||
            transform.position.x > 10f || transform.position.x < -10f) // Adjust bounds
        {
            Destroy(gameObject);
        }
    }

    // Called when the laser collides with another 2D Collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the laser hits an asteroid, destroy both
        if (other.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject); // Destroy the asteroid
            Destroy(gameObject);       // Destroy the laser

            // Optional: Add score for destroying asteroids (e.g., if PlayerController tracks this)
            // Or, let GameManager handle this if Asteroid had a Health component.
        }
    }
}