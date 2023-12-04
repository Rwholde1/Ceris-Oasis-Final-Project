using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MolotovCocktail : MonoBehaviour
{
    public GameObject fireEffectPrefab; // Assign the fire particle system prefab in the Inspector
    public AudioClip igniteSound; // Assign the ignite sound in the Inspector
    public AudioClip crackleSound;
    private bool hit = false;
    public GameObject player;
    public GameObject DamageSphere;
    public int pID = -1;
    private void OnCollisionEnter(Collision collision)
    {        
        if(collision.gameObject.tag == "Player")
        {
            return;
        }
        print(collision.collider.name);
        if (!hit)
        {
            hit = true;

            // Instantiate the fire particle effect
            GameObject fireEffect = Instantiate(fireEffectPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            if (pID == LobbySceneManagement.singleton.getLocalPlayer().identity) {
                GameObject sphere = Instantiate(DamageSphere, fireEffect.transform.position, fireEffect.transform.rotation);
                sphere.GetComponent<MolotovSphere>().player = player;
            }
            // Play the ignite sound
            AudioSource.PlayClipAtPoint(igniteSound, fireEffect.transform.position);
            AudioSource.PlayClipAtPoint(crackleSound, fireEffect.transform.position, 0.7f);

            // Destroy the Molotov cocktail object (you might want to disable it instead, depending on your game design)
            Destroy(fireEffect, 5f);
            Destroy(gameObject, 6);
        }
    }


}
