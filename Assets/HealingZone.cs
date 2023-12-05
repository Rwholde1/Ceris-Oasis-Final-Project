using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingZone : MonoBehaviour
{
    public GameObject healEffectPrefab; // Assign the fire particle system prefab in the Inspector
    public AudioClip castSound; // Assign the ignite sound in the Inspector
    //public AudioClip persistentSound;
    private bool hit = false;
    public GameObject player;
    public GameObject HealSphere;
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
            //GameObject boom = Instantiate(explosion, transform.position, transform.rotation);
            //boom.GetComponent<Transform>().eulerAngles = new Vector3(0f, 0f, 0f);

            hit = true;
            Debug.Log("instantiating health effect");
            // Instantiate the fire particle effect
            GameObject healEffect = Instantiate(healEffectPrefab, collision.contacts[0].point, Quaternion.identity);
            AudioSource.PlayClipAtPoint(castSound, healEffect.transform.position, 0.3f);
            
            Debug.Log("healing identities: " + pID + " " + LobbySceneManagement.singleton.getLocalPlayer().identity);
            if (pID == LobbySceneManagement.singleton.getLocalPlayer().identity) {

                GameObject sphere = Instantiate(HealSphere, healEffect.transform.position, healEffect.transform.rotation);
                //sphere.GetComponent<MolotovSphere>().player = player;
            }
            // Play the ignite sound
            AudioSource.PlayClipAtPoint(castSound, healEffect.transform.position);
            //AudioSource.PlayClipAtPoint(persistentSound, healEffect.transform.position, 0.7f);

            // Destroy the Molotov cocktail object (you might want to disable it instead, depending on your game design)
            Destroy(healEffect, 7f);
            Destroy(gameObject, 7f);
        }
    }


}
