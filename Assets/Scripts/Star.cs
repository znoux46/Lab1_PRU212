// --- SCRIPT: Star.cs ---
// Attach this script to your Star Prefab.

using UnityEngine;

public class Star : MonoBehaviour
{
    // Stars don't need complex movement, they just wait to be collected.
    // They might have a subtle animation (e.g., pulsating), but for basic functionality, this is enough.
    // The collection logic is primarily handled by the PlayerController's OnTriggerEnter2D.

    // You might add an effect or sound when collected, handled by PlayerController.
    // This script can be very simple.
}