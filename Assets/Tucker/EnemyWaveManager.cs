using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : NetworkBehaviour
{
    public static EnemyWaveManager singleton = null;

    public List<NetworkObject> spawnedEnemies = new List<NetworkObject>();
    //public GameObject[] spawnPoints;
    
    public List<Transform> enemSpawners = new List<Transform>();

    //Boss
    [SerializeField] public NetworkObject Queenie;
    public List<NetworkObject> DC4Enems = new List<NetworkObject>();

    //DC 3
    [SerializeField] public NetworkObject Wanderer;
    public List<NetworkObject> DC3Enems = new List<NetworkObject>();

    //DC 2
    [SerializeField] public NetworkObject HardGunner;
    [SerializeField] public NetworkObject Lobber;
    public List<NetworkObject> DC2Enems = new List<NetworkObject>();

    //DC 1
    [SerializeField] public NetworkObject MediumGunner;
    public List<NetworkObject> DC1Enems = new List<NetworkObject>();

    //DC 0
    [SerializeField] public NetworkObject EasyGunner;
    [SerializeField] public NetworkObject Rusher;
    public List<NetworkObject> DC0Enems = new List<NetworkObject>();

    public List<List<NetworkObject>> DCListHolder = new List<List<NetworkObject>>();

    public bool isServer = false;

    public float timeToStart = 5f;
    public bool started = false;

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
        //DC 4
        DC4Enems.Add(Queenie);

        //DC 3
        DC3Enems.Add(Wanderer);

        //DC 2
        DC2Enems.Add(HardGunner);
        DC2Enems.Add(Lobber);

        //DC 1
        DC1Enems.Add(MediumGunner);

        //DC 0
        DC0Enems.Add(EasyGunner);
        DC0Enems.Add(Rusher);

        //Holder
        DCListHolder.Add(DC0Enems);
        DCListHolder.Add(DC1Enems);
        DCListHolder.Add(DC2Enems);
        DCListHolder.Add(DC3Enems);
        DCListHolder.Add(DC4Enems);

        
    }

    // Update is called once per frame
    void Update()
    {

        //Destroys on all but host instance
        if (!LobbySceneManagement.singleton.getLocalPlayer().getIsHost()) {
            Debug.Log("Destroying wave manager on client manager");
            Destroy(gameObject);
        } else {
            isServer = true;
        }

        if (SceneManager.GetActiveScene().name != "LobbyScene") {
            if (timeToStart > 0f) {
                timeToStart -= Time.deltaTime;
            } else if (!started) {
                //startSpawningTest();
                started = true;
            }



        }
        
    }

    public void startWave(int waveNum) {

    }

    public void startSpawningTest() {
        Debug.Log("Starting test spawn");
        int i = -1;
        int j = 0;
        foreach(List<NetworkObject> dcList in DCListHolder) {
            i = 0;
            foreach(NetworkObject enem in dcList) {
                Debug.Log("DC " + j + " enem " + i);
                int spawnInd = pickRandomSpawner();
                Debug.Log("Spawner " + spawnInd);
                Debug.Log(DCListHolder[j][i]);
                //spawnEnemyServerRpc(j, i, spawnInd);
                i++;
                spawnEnemyServerRpc(j, i, spawnInd);
                Debug.Log("ending spawn rpc");
            }
            j++;
        }
    }

    //Generates random spawner index
    public int pickRandomSpawner() {
        return (int) Mathf.Floor(Random.Range(0f, enemSpawners.Count - 0));

    }

    [ServerRpc]
    void spawnEnemyServerRpc(int DC, int indexInDC, int spawnerIndex) {
        spawnEnemyClientRpc(DC,indexInDC, spawnerIndex);
        Debug.Log("entering spawner rpc");
        Debug.Log("server spawning enemy " + indexInDC + " of DC " + DC + " on spawner " + spawnerIndex);
        //spawnEnemyClientRpc(DC, indexInDC, spawnerIndex);
        Debug.Log(DCListHolder[DC][indexInDC]);
        //NetworkObject newEnem = Instantiate(DCListHolder[DC][indexInDC], enemSpawners[spawnerIndex].position, Quaternion.identity);
        //newEnem.GetComponent<NetworkObject>().Spawn();
        //zone.GetComponent<Rigidbody>().AddForce(throwDir);
        //newEnem.GetComponent<EnemyHitRegister>().enemyID = spawnedEnemies.Count;
        //spawnedEnemies.Add(newEnem);
    }

    
    [ClientRpc]
    public void spawnEnemyClientRpc(int DC, int indexInDC, int spawnerIndex) {
        Debug.Log("client spawning enemy " + indexInDC + " of DC " + DC + " on spawner " + spawnerIndex);


    }
    
}
