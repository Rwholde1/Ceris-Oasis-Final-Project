using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    //public float radius = 2f;
    public float timeToRegister = 1f;
    public bool registered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!LobbySceneManagement.singleton.getLocalPlayer().getIsHost()) {
            Debug.Log("Destroying spawn location on client");
            Destroy(gameObject);
        }

        if (timeToRegister >= 0f) {
            timeToRegister -= Time.deltaTime;
            
        } /*else if (!EnemyWaveManager.singleton.enemSpawners.Contains(GetComponent<Transform>())) {
            Debug.Log("registering spawn zone " + this);
            EnemyWaveManager.singleton.enemSpawners.Add(GetComponent<Transform>());
            registered = true;
            //LobbySceneManagement.singleton.playerSpawnZoneRadius = radius;
        
        }*/
         else if (!EnemyWaveSpawnerTake2.singleton.enemSpawners.Contains(GetComponent<Transform>())) {
            Debug.Log("registering spawn zone " + this);
            EnemyWaveSpawnerTake2.singleton.enemSpawners.Add(GetComponent<Transform>());
            registered = true;
            //LobbySceneManagement.singleton.playerSpawnZoneRadius = radius;
        
        }
    }
}
