using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    public GameObject controller;
    private void Update()
    {
        // Check for the 'F' key press
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Raycast to check for interactable objects
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                print("hit smth");
                // Check if the hit object has an Interactable script
                ShopKeep shop = hit.collider.GetComponent<ShopKeep>();
                if (shop != null)
                {
                    print("is Shop");
                    // Interact with the object
                    shop.getInteractionPoint(controller);
                    shop.ToggleShop();
                }
            }
        }
    }
}