using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : NetworkBehaviour
{
    public static EnemyWaveManager singleton = null;

    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public GameObject[] spawnPoints;

    public int waveNum = 0;

    void Awake() {
        if (singleton == null) {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        } 
        else if (singleton != this) {
            Debug.Log(singleton.name + " replaced me");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "LobbyScene") {

        }
        
    }

    public void startWave(int waveNum) {

    }
}
