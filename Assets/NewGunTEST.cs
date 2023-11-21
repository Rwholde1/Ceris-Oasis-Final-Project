using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGunTEST : MonoBehaviour
{
    public int gunIdToAcquire = 0;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLISION");
        // Check if the colliding object is the player
        GunManager playerGunManager = other.GetComponent<GunManager>();
        if (playerGunManager != null)
        {
            // Get the PlayerGunManager component from the player
            // Check if the PlayerGunManager component is present
            if (playerGunManager != null)
            {
                // Call the changegunid method with the specified gunId
                playerGunManager.AcquireNewGun(gunIdToAcquire);

                // Optionally, you can disable the collider to prevent multiple pickups

                // Destroy the gun object (optional, if you want the gun to disappear after picking it up)
            }
        }
    }


}
