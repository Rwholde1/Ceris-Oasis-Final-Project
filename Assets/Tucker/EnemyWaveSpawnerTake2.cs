using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class EnemyWaveSpawnerTake2 : NetworkBehaviour
{
    public static EnemyWaveSpawnerTake2 singleton = null;

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

    //DC, index in DC, spawner
    public List<int[]> enemsInWave = new List<int[]>();

    public bool isServer = false;

    public float timeToStart = 5f;
    public bool started = false;

    public int waveCount = 0;
    public int waveIndex = 0;

    //Proportion of total enemies in wave to spawn
    public float spawnCountModifier = 1f;
    public float payoutModifier;
    //Wave length in seconds, total enemies in wave, prop. of DC0 Enems, prop. of DC1 Enems, prop. of DC2 Enems, prop. of DC3 Enems, prop of DC4 Enems, isBreak(0 false, 1 true))
    //Proportions given in int 0-100
    public int[,] waveData = {
        //Wave data
        {15, 0, 0, 0, 0, 0, 0, 1},

        //Waves 1-5
        {15, 25, 100, 0, 0, 0, 0, 0},
        {30, 30, 100, 0, 0, 0, 0, 0},
        {30, 50, 90, 10, 0, 0, 0, 0},
        {45, 80, 80, 20, 0, 0, 0, 0},
        {45, 100, 75, 25, 0, 0, 0, 0},

        //Break
        {75, 0, 0, 0, 0, 0, 0, 1},

        //Waves 6-10
        {60, 110, 70, 30, 0, 0, 0, 0},
        {60, 115, 70, 25, 5, 0, 0, 0},
        {60, 120, 69, 24, 7, 0, 0, 0},
        {60, 130, 67, 23, 10, 0, 0, 0},
        {60, 140, 64, 23, 13, 0, 0, 0},

        //Break
        {75, 0, 0, 0, 0, 0, 0, 1},

        //Waves 11-15
        {75, 145, 65, 20, 15, 0, 0, 0},
        {75, 150, 68, 23, 8, 1, 0, 0},
        {75, 155, 70, 23, 5, 2, 0, 0},
        {75, 170, 72, 20, 6, 2, 0, 0},
        {75, 200, 72, 18, 7, 3, 0, 0},

        //Break
        {75, 0, 0, 0, 0, 0, 0, 1},

        //Waves 16-20
        {90, 220, 70, 19, 7, 4, 0, 0},
        {90, 240, 70, 16, 8, 6, 0, 0},
        {90, 260, 65, 17, 10, 8, 0, 0},
        {90, 280, 60, 21, 9, 10, 0, 0},
        {90, 300, 55, 21, 9, 15, 0, 0},

        //Break
        {75, 0, 0, 0, 0, 0, 0, 1},

        //Waves 21-25
        {90, 290, 55, 27, 10, 8, 0, 0},
        {75, 280, 55, 25, 12, 8, 0, 0},
        {75, 270, 55, 23, 13, 9, 0, 0},
        {75, 280, 55, 22, 15, 8, 0, 0},
        {90, 300, 55, 22, 13, 10, 0, 0},

        //Break???
        /*
        {75, 0, 0, 0, 0, 0, 0, 1};
        */

        //Boss Wave
        {0, 1, 0, 0, 0, 0, 100, 0}
    };


    //Timer stuff
    public int waveDuration = 60;
    public int timeRemaining = 0;
    public bool isCountingDown = false;
    public bool startedWaveStuff = false;

    public int spawnPerTick = 0;

    public int enemsInWaveSpawned = 0;

    public int buffer = 0;

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

    void OnEnable() {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("Connected players: " + NetworkManager.ConnectedClientsIds.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnCountModifier = spawnCountModifier /(float) NetworkManager.ConnectedClientsIds.Count;
        payoutModifier = 1f / spawnCountModifier;
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
        Debug.Log("started: " + started);

        //Destroys on all but host instance
        
        if (!LobbySceneManagement.singleton.getLocalPlayer().getIsHost()) {
            Debug.Log("Destroying wave manager on client manager");
            Destroy(gameObject);
        } else {
            isServer = true;
        }
        
        /*
        if (SceneManager.GetActiveScene().name != "LobbyScene" && !startedWaveStuff) {
            Debug.Log("called first wave");
            startWave();
            
            if (timeToStart > 0f) {
                timeToStart -= Time.deltaTime;
            } else if (!started) {
                started = true;
                startSpawningTest();
            }



        }*/

        if (startedWaveStuff && !isCountingDown && LobbySceneManagement.singleton.playerCamObject != null) {
            Debug.Log("called wave index " + waveIndex);
            startWave();
        }

        if (SceneManager.GetActiveScene().name != "LobbyScene" && !startedWaveStuff && LobbySceneManagement.singleton.playerCamObject != null) {
            Debug.Log("Starting first wave");
            spawnCountModifier = 0.25f * (float) NetworkManager.ConnectedClientsIds.Count;
            payoutModifier = 1f / spawnCountModifier;
            Debug.Log("spawn modifier: " + spawnCountModifier + " pay modifier: " + payoutModifier);
            startWave();
        }

        //For testing
        if (Input.GetKey(KeyCode.T)) {
            Debug.Log("passing time");
            Debug.Log("tick");
            timeRemaining--;
            if(timeRemaining > 0) {

            
            LobbySceneManagement.singleton.getLocalPlayer().decrementSecondServerRpc();
            } else {
                isCountingDown = false;
            }
        }
        
    }

    public void startSpawningTest() {
        started = true;
        Debug.Log("Starting test spawn");
        int i = -1;
        int j = 0;
        foreach(List<NetworkObject> dcList in DCListHolder) {
            i = -1;
            foreach(NetworkObject enem in dcList) {
                Debug.Log("DC " + j + " enem " + i);
                int spawnInd = pickRandomSpawner();
                Debug.Log("Spawner " + spawnInd);
                //Debug.Log(DCListHolder[j][i]);
                //spawnEnemyServerRpc(j, i, spawnInd);
                i++;
                //Debug.Log(enemSpawners.Count);
                //Debug.Log(enemSpawners[spawnInd]); 
                //Debug.Log(DCListHolder[j]);               
                //NetworkObject newEnem = Instantiate(DCListHolder[j][i], enemSpawners[spawnInd].position, Quaternion.identity);
                //newEnem.GetComponent<EnemyHitRegister>().enemyID = spawnedEnemies.Count;
                //spawnedEnemies.Add(newEnem);
                Debug.Log(j + " " + i + " " + spawnInd);
                //LobbySceneManagement.singleton.getLocalPlayer().spawnTestServerRpc(j, i, spawnInd);
                LobbySceneManagement.singleton.getLocalPlayer().spawnEnemyServerRpc(j, i, spawnInd, payoutModifier);
                //LobbySceneManagement.singleton.getLocalPlayer().spawnTestServerRpc(j, i, spawnInd);
                Debug.Log("ending spawn rpc");
            }
            //j++;
        }
    }

    //Generates random spawner index
    public int pickRandomSpawner() {
        return (int) Mathf.Floor(Random.Range(0f, enemSpawners.Count - 0.01f));

    }

    //timer ticker    
    private void _tick() {
        Debug.Log("tick");
        timeRemaining--;
        int thisBuffer = buffer;
        if (thisBuffer == 0) { thisBuffer = 3; }
        if(timeRemaining > 0) {
            if (timeRemaining > thisBuffer) {
                int startCount = enemsInWaveSpawned;
                for (int i = enemsInWaveSpawned; i < (startCount + spawnPerTick); i++) {
                    if (i < enemsInWave.Count) {
                        Debug.Log("calling enem spawn for enem " + enemsInWave[i][1] + " of dc " + enemsInWave[i][0] + " to spawn on spawner " + enemsInWave[i][2]);
                        LobbySceneManagement.singleton.getLocalPlayer().spawnEnemyServerRpc(enemsInWave[i][0], enemsInWave[i][1], enemsInWave[i][2], payoutModifier);
                        enemsInWaveSpawned++;
                    }
                }
            }
            
        
            LobbySceneManagement.singleton.getLocalPlayer().decrementSecondServerRpc();
            Invoke ( "_tick", 1f );
        } else {
            isCountingDown = false;
        }
    }

    /*
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("entered scene " + scene);
        if (scene.name != "LobbyScene") {
            Debug.Log("Starting first wave");
            startWave();
        }

    }*/

    private void startWave() {
        enemsInWave.Clear();
        isCountingDown = true;
        startedWaveStuff = true;
                Debug.Log("starting to start wave " + waveCount);

        int[] thisWave = new int[8];
        for(int i = 0; i < 8; i++) {
            thisWave[i] = waveData[waveIndex, i];
        }
        int enemsToSpawnCount = (int) ((float) thisWave[1] * spawnCountModifier);
        for (int i = 0; i < enemsToSpawnCount; i++) {
            enemsInWave.Add(getRandomEnem(thisWave));
        }
        bool isBreak = false;
        if (thisWave[7] == 1) {
            isBreak = true;
        }
        Debug.Log("starting to start wave " + waveCount);
        LobbySceneManagement.singleton.getLocalPlayer().setupWaveServerRpc(thisWave[0], waveCount, isBreak);
        Debug.Log("starting wave index " + waveIndex + " timer for " + thisWave[0]);
        startTimer(thisWave[0], thisWave[1]);
            //Wave length in seconds, total enemies in wave, prop. of DC0 Enems, prop. of DC1 Enems, prop. of DC2 Enems, prop. of DC3 Enems, prop of DC4 Enems, isBreak(0 false, 1 true))
        //NetworkObject[]
        //Generate list of enemies for this wave



        if (thisWave[7] == 0) { waveCount++; }
        waveIndex++;
    }

    private int[] getRandomEnem(int[] thisWave) {
        int[] result = new int[3];
        float selector = Random.Range(0f, 1f);
        result[0] = 0;
        float chance = 0f;
        //DC 0
        chance += ((float) thisWave[2] / 100f);
        if (selector < chance) {
            result[0] = 0; 
        } else {
            //DC 1
            chance += ((float) thisWave[3] / 100f);
            if (selector < chance) {
                result[0] = 1;
            } else {
                //DC 2
                chance += ((float) thisWave[4] / 100f);
                if (selector < chance) {
                    result[0] = 2;
                } else {
                    //DC 3
                    chance += ((float) thisWave[5] / 100f);
                    if (selector < chance) {
                        result[0] = 3;
                    } else {
                        //DC 4 (Boss)
                        chance += ((float) thisWave[6] / 100f);
                        if (selector < chance) {
                            result[0] = 4;
                        }
                    }
                }
            }
        }

        result[1] = Random.Range(0, DCListHolder[result[0]].Count);
        result[2] = pickRandomSpawner();
        Debug.Log("chose enemy " + result[1] + " of dc " + result[0] + " to spawn on spawner " + result[2]);
        return result;
    }

    private void startTimer(int secsIn, int enemCount) {
        //Starts timer
        waveDuration = secsIn;
        enemsInWaveSpawned = 0;
        timeRemaining = secsIn;
        float prop = 0.33f;
        spawnFirstPortion(prop);
        buffer = 30;
        if (secsIn < 45) {
            buffer = 0;
        }
        //Debug.Log()
        spawnPerTick = ((int) (((1f - prop) * enemCount) / (secsIn - buffer)));
        Debug.Log("will spawn " + spawnPerTick + "enems per seconds tick");
        if (secsIn != 0) {
            Invoke( "_tick", 1f );
        }

    }

    private void spawnFirstPortion(float portion) {
        int numToSpawn = ((int) (portion * enemsInWave.Count));
        Debug.Log("first portion: " + numToSpawn);
        for (int i = enemsInWaveSpawned; i < numToSpawn; i++) {
            Debug.Log("calling enem spawn for enem " + enemsInWave[i][1] + " of dc " + enemsInWave[i][0] + " to spawn on spawner " + enemsInWave[i][2]);
            LobbySceneManagement.singleton.getLocalPlayer().spawnEnemyServerRpc(enemsInWave[i][0], enemsInWave[i][1], enemsInWave[i][2], payoutModifier);
            enemsInWaveSpawned++;
        }
    }
}



    


    
