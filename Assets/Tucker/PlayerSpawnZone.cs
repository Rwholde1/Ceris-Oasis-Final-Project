using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnZone : MonoBehaviour
{
    public float radius = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbySceneManagement.singleton.playerSpawnZone == null) {
            LobbySceneManagement.singleton.playerSpawnZone = GetComponent<Transform>();
            LobbySceneManagement.singleton.playerSpawnZoneRadius = radius;
        }
    }
}
