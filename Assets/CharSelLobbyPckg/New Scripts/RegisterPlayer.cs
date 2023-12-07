using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;


public delegate void PlayerClick (object sender, System.EventArgs e);

public class RegisterPlayer : NetworkBehaviour/*, INetworkSerializable*/
{
    
    /*
    *
    * VARIABLES
    *
    */

    public event PlayerClick OnPlayerClick;
    
    private GameObject sceneManager;
    //private LobbySceneManagement managerScript;
    private bool registered = false;
    private Button rename;

    public int identity = -1;

    public int charIdentity = -1;

    public Animator[] animatorsList = new Animator[4];
    public Transform[] gunpositions;
    private GameObject holder;
    private GameObject gunStuff;
    public bool isDead;

    /*
    *
    * INHERENT & EVENT FUNCTIONS
    *
    */
    
    void Awake() {
        addJoinCodeServerRpc();
        //Debug.Log("awake");
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)) {
            //Debug.Log("Player clicked local");
            OnPlayerClick(this, System.EventArgs.Empty);
            Debug.Log(IsLocalPlayer + " " + IsHost + " " + IsClient + " " + IsServer);
        }  
        /*
        if (!registered) {
            if (sceneManager == null) {
                sceneManager = GameObject.FindWithTag("GameController");
                Debug.Log(sceneManager.name + " is the  controller");
            } else {
                var managerScript = sceneManager.GetComponent<LobbySceneManagement>();
                Debug.Log(managerScript +" scene begin");
                if (managerScript != null) {
                    registered = true;
                    Debug.Log(GetComponent<Transform>() + " Player Transform");
                    identity = managerScript.identifyPlayer(GetComponent<Transform>());
                }
            }
        }
        */
        
        /*
        if (IsLocalPlayer) {
            if (identity <= 0) {
                identity = LobbySceneManagement.singleton.identifyPlayer(this);
            }
        }*/
        
        /*
        if (rename == null) {
            Debug.Log("Button: " + LobbySceneManagement.singleton.renameButton);
            rename = LobbySceneManagement.singleton.renameButton;
            //rename.onClick.AddListener(() => {LobbySceneManagement.singleton.renamePlayer(identity); Debug.Log("clicked");});
            // /rename.onClick.AddListener(renamePlayer);
        }
        */
        
        /*
        if (IsHost && ) {
            LobbySceneManagement.singleton.levelSelector.changeLevel();
        }*/
    }

    public bool getIsHost() {
        return IsHost;
    }

    public bool getIsLocalPlayer() {
        return IsLocalPlayer;
    }

    public bool getIsClient() {
        return IsClient;
    }

    public bool getIsServer() {
        return IsServer;
    }

    public int getIdentity() {
        return identity;
    }



    /*
    *
    * SERIALIZATION
    *
    */

    //int[,]
    public static byte[] Serialize2DInt(int[,] toSerialize) {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, toSerialize);
        return ms.ToArray();
    }
 
    public static int[,] Deserialize2DInt(byte[] toDeserialize){
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(toDeserialize);
        return (int[,]) bf.Deserialize(ms);
    }

    //string[,]
    public static byte[] Serialize2DString(string[,] toSerialize) {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, toSerialize);
        return ms.ToArray();
    }
 
    public static string[,] Deserialize2DString(byte[] toDeserialize){
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(toDeserialize);
        //Debug.Log("deser string: " + bf.Deserialize(ms));
        return (string[,]) bf.Deserialize(ms);
    }



    /*
    *
    * RPCs
    *
    */

    //Initializes new client manager with host manager data
    [ServerRpc(RequireOwnership = false)]
    public void startManagerServerRpc() {
        Debug.Log("initialization server rpc");
        if (IsServer) {
            //var thisPlayerCount = LobbySceneManagement.singleton.playerCount;

            var thisPlayer1 = LobbySceneManagement.singleton.players[0];
            var thisPlayer2 = LobbySceneManagement.singleton.players[1];
            var thisPlayer3 = LobbySceneManagement.singleton.players[2];
            var thisPlayer4 = LobbySceneManagement.singleton.players[3];

            var thisCamsTaken1 = LobbySceneManagement.singleton.camsTaken[0];
            var thisCamsTaken2 = LobbySceneManagement.singleton.camsTaken[1];
            var thisCamsTaken3 = LobbySceneManagement.singleton.camsTaken[2];
            var thisCamsTaken4 = LobbySceneManagement.singleton.camsTaken[3];

            var thisJoinCode = LobbySceneManagement.singleton.joinCode;

            var thisPlayerLobbyInfo = Serialize2DString(LobbySceneManagement.singleton.playerLobbyInfo);
            var thisStatsArray = Serialize2DInt(LobbySceneManagement.singleton.statsArray);

            //Debug.Log("Manager Data: " + /*thisPlayerCount + " " + thisPlayer1 + " " + thisPlayer2 + " " + thisPlayer3 + " " + thisPlayer4 + " " + */thisCamsTaken1 + " " + thisCamsTaken2 + " " + thisCamsTaken3 + " " + thisCamsTaken4 + " " + thisJoinCode);
            startManagerClientRpc(/*thisPlayerCount, thisPlayer1, thisPlayer2, thisPlayer3, thisPlayer4,*/ thisCamsTaken1, thisCamsTaken2, thisCamsTaken3, thisCamsTaken4, thisJoinCode, thisPlayerLobbyInfo, thisStatsArray);
        }
    }

    [ClientRpc]
    public void startManagerClientRpc(/*int thisPlayerCount, RegisterPlayer thisPlayer1, RegisterPlayer thisPlayer2, RegisterPlayer thisPlayer3, RegisterPlayer thisPlayer4,*/
     bool thisCamsTaken1, bool thisCamsTaken2, bool thisCamsTaken3, bool thisCamsTaken4, string thisJoinCode, byte[] thisPlayerLobbyInfo, byte[] thisStatsArray) {
        
        //Debug.Log("Client initialization recieved: " + /*thisPlayerCount + " " + thisPlayer1 + " " + thisPlayer2 + " " + thisPlayer3 + " " + thisPlayer4 + " " + */thisCamsTaken1 + " " + thisCamsTaken2 + " " + thisCamsTaken3 + " " + thisCamsTaken4 + " " + thisJoinCode);


        //LobbySceneManagement.singleton.playerCount = thisPlayerCount;

        //LobbySceneManagement.singleton.players[0] = thisPlayer1;
        //LobbySceneManagement.singleton.players[1] = thisPlayer2;
        //LobbySceneManagement.singleton.players[2] = thisPlayer3;
        //LobbySceneManagement.singleton.players[3] = thisPlayer4;

        LobbySceneManagement.singleton.camsTaken[0] = thisCamsTaken1;
        LobbySceneManagement.singleton.camsTaken[1] = thisCamsTaken2;
        LobbySceneManagement.singleton.camsTaken[2] = thisCamsTaken3;
        LobbySceneManagement.singleton.camsTaken[3] = thisCamsTaken4;

        if (identity <= 0) {
            //Debug.Log("assigning player id");
            identity = LobbySceneManagement.singleton.identifyPlayer(this);
        }
        
        //Debug.Log("re-iding player");
        LobbySceneManagement.singleton.reIDPlayer();

        LobbySceneManagement.singleton.joinCode = thisJoinCode;
        
        //Unpacks Arrays
        for (int r = 0; r < 4; r++) {
            //Player lobby info (name, ready state)
            for (int c1 = 0; c1 < 2; c1++) {
                LobbySceneManagement.singleton.playerLobbyInfo[r, c1] = Deserialize2DString(thisPlayerLobbyInfo)[r, c1];
                //Debug.Log("PLI " + r + " " + c1 + " = " + Deserialize2DString(thisPlayerLobbyInfo)[r, c1]);
                if (c1 == 0 && Deserialize2DString(thisPlayerLobbyInfo)[r, c1] != null) {
                    LobbySceneManagement.singleton.playerNames[r].SetText(LobbySceneManagement.singleton.playerLobbyInfo[r, c1]);
                }
            }

            //Stats
            for (int c2 = 0; c2 < 4; c2++) {
                LobbySceneManagement.singleton.statsArray[r, c2] = Deserialize2DInt(thisStatsArray)[r, c2];
                //Debug.Log("Stats Array " + r + " " + c2 + " = " + Deserialize2DInt(thisStatsArray)[r, c2]);
                //Add bit to update newly joining client stats window
            }
        }
        

        
    }



    //Updates Most Recent Player Clicked
    [ServerRpc(RequireOwnership = false)]
    public void ClickedServerRpc(int playerID) {
        //Debug.Log("player clicked server");
        LobbySceneManagement.singleton.mostRecentPlayerClick = playerID;
        Debug.Log(LobbySceneManagement.singleton.mostRecentPlayerClick);
        if (IsServer) {
            ClickedClientRpc(playerID);
        }
    }

    [ClientRpc]
    public void ClickedClientRpc(int playerID) {
        //Debug.Log("player clicked client");
        LobbySceneManagement.singleton.mostRecentPlayerClick = playerID;
        Debug.Log(LobbySceneManagement.singleton.mostRecentPlayerClick);
    }



    //Renames player
    [ServerRpc(RequireOwnership = false)]
    public void renamePlayerServerRpc(string name, int playerID) {
        if (IsServer) {
            //Debug.Log("Renaming player " + LobbySceneManagement.singleton.mostRecentPlayerClick + " to: " + name + " on server"); 
            //LobbySceneManagement.singleton.playerNames[LobbySceneManagement.singleton.mostRecentPlayerClick - 1].SetText(name);   
            renamePlayerClientRpc(name, playerID - 1);    
        }
    }

    [ClientRpc]
    public void renamePlayerClientRpc(string name, int playerID) {
        if (IsClient) {
            //Debug.Log("Renaming player " + LobbySceneManagement.singleton.mostRecentPlayerClick + " to: " + name + " on client"); 
            //LobbySceneManagement.singleton.playerNames[LobbySceneManagement.singleton.mostRecentPlayerClick - 1].SetText(name); 
            LobbySceneManagement.singleton.playerNames[playerID].SetText(name);
            if (name.Length > 0) {
                LobbySceneManagement.singleton.playerNamesText[playerID] = name; 
            }
            

        }
          
    }

    

    //Identifies player
    [ServerRpc(RequireOwnership = false)] 
    public void identifyThisPlayerServerRpc(int id) {
        LobbySceneManagement.singleton.players[id] = this;
        LobbySceneManagement.singleton.camsTaken[id] = true;
        LobbySceneManagement.singleton.players[id].OnPlayerClick += LobbySceneManagement.singleton.Clicked;

        if (IsServer) {
            //Debug.Log("identifying and also updating level info");
            identifyThisPlayerClientRpc(id);
            //LobbySceneManagement.singleton.levelSelector.changeLevel();
        }
    }

    [ClientRpc]
    public void identifyThisPlayerClientRpc(int id) {
        LobbySceneManagement.singleton.players[id] = this;
        LobbySceneManagement.singleton.camsTaken[id] = true;
        LobbySceneManagement.singleton.players[id].OnPlayerClick += LobbySceneManagement.singleton.Clicked;
        

    }



    //Updates join code on all clients
    [ServerRpc(RequireOwnership = false)] 
    public void addJoinCodeServerRpc() {
        //Debug.Log("hit server rpc");
        //LobbySceneManagement.singleton.joinCodeText.SetText(code);
        if (IsServer) {
            addJoinCodeClientRpc(LobbySceneManagement.singleton.joinCode);
        }
    }

    [ClientRpc]
    public void addJoinCodeClientRpc(string code) {
        //Debug.Log("Join code: " + code);
        LobbySceneManagement.singleton.joinCode = code;
        LobbySceneManagement.singleton.joinCodeText.SetText(code);
    }



    //Changes player ready status
    [ServerRpc(RequireOwnership = false)]
    public void changeReadyServerRpc(string ready, int id) {
        if (IsServer) {
            changeReadyClientRpc(ready, id);
        }
    }

    [ClientRpc]
    public void changeReadyClientRpc(string ready, int id) {
        //LobbySceneManagement.singleton.playerLobbyInfo[LobbySceneManagement.singleton.mostRecentPlayerClick - 1, 1] = ready;
        LobbySceneManagement.singleton.playerLobbyInfo[id, 1] = ready;

    }



    //Handles host level selection
    [ServerRpc(RequireOwnership = false)]
    public void updateSelectedLevelServerRpc() {
        if (IsServer) {
            updateSelectedLevelClientRpc(LobbySceneManagement.singleton.SceneToPlay);
            //HostSingleton.Instance.lobby.lobbyName = LobbySceneManagement.singleton.SceneToPlay;
            updateLobbyInfo();
        }
    }

    [ClientRpc]
    public void updateSelectedLevelClientRpc(string nameIn) {
        LobbySceneManagement.singleton.levelSelector.updateLevelStuff(nameIn);
    }

    public async void updateLobbyInfo() {
        try
        {
            //Debug.Log("attempting to update name");
            UpdateLobbyOptions options = new UpdateLobbyOptions();
            options.Name = LobbySceneManagement.singleton.SceneToPlay;  
            //options.MaxPlayers = 4;
            //options.IsPrivate = false;

            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            // /options.HostId = AuthenticationService.Instance.PlayerId;
            /*
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "ExamplePrivateData", new DataObject(
                        visibility: DataObject.VisibilityOptions.Private,
                        value: "PrivateData")
                },
                {
                    "ExamplePublicData", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: "PublicData",
                        index: DataObject.IndexOptions.S1)
                },
            };*/

            var lobby = await LobbyService.Instance.UpdateLobbyAsync(HostSingleton.Instance.lobbyId, options);

            //...
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Passes damage call to playermanager
    public void takeDamage(int damageIn, int playerID) {
        Debug.Log("player " + playerID + " damageed by enemy for health: " + damageIn);
        LobbySceneManagement.singleton.playerCamObject.GetComponent<PlayerManager>().takeDamage(damageIn, playerID);
    }

    public void takeDamage(float damageIn, int playerID) {
        Debug.Log("player " + playerID + " damageed by enemy for health: " + damageIn);
        LobbySceneManagement.singleton.playerCamObject.GetComponent<PlayerManager>().takeDamage((int) damageIn, playerID);
    }



    //Enemy spawning scripts because why the fuck won't they work on the actual spawner
    [ServerRpc(RequireOwnership = false)]
    public void spawnTestServerRpc(int j, int i, int spawnInd) {
        if (IsServer) {
            //Debug.Log("server received " + j + " " + i + " " + spawnInd);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void spawnEnemyServerRpc(int DC, int indexInDC, int spawnerIndex, float payoutMod) {
        if (IsClient) {
            spawnEnemyClientRpc(DC,indexInDC, spawnerIndex);
            //Debug.Log("entering spawner rpc");
            //Debug.Log("server spawning enemy " + indexInDC + " of DC " + DC + " on spawner " + spawnerIndex);
            //spawnEnemyClientRpc(DC, indexInDC, spawnerIndex);
            Debug.Log(EnemyWaveSpawnerTake2.singleton.DCListHolder[DC][indexInDC]);
            NetworkObject newEnem = Instantiate(EnemyWaveSpawnerTake2.singleton.DCListHolder[DC][indexInDC], EnemyWaveSpawnerTake2.singleton.enemSpawners[spawnerIndex].position, Quaternion.identity);
            newEnem.GetComponent<NetworkObject>().Spawn();
            //zone.GetComponent<Rigidbody>().AddForce(throwDir);
            newEnem.GetComponent<EnemyHitRegister>().enemyID = EnemyWaveSpawnerTake2.singleton.spawnedEnemies.Count;
            newEnem.GetComponent<EnemyHitRegister>().payoutModifier = payoutMod;
            EnemyWaveSpawnerTake2.singleton.spawnedEnemies.Add(newEnem);
        }
    }

    
    [ClientRpc]
    public void spawnEnemyClientRpc(int DC, int indexInDC, int spawnerIndex) {
        if (IsClient) {
            //Debug.Log("client spawning enemy " + indexInDC + " of DC " + DC + " on spawner " + spawnerIndex);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void setupWaveServerRpc(int secsIn, int waveCount, bool isBreak) {
        if (IsServer) {
            //Debug.Log("setting wave " + waveCount + " on server");
            setupWaveClientRpc(secsIn, waveCount, isBreak);
        }
    }

    [ClientRpc]
    public void setupWaveClientRpc(int secsIn, int waveCount, bool isBreak) {
        if (IsClient) {
            Debug.Log("setting wave " + waveCount + " on client");
            if (LobbySceneManagement.singleton.dead) {
                Debug.Log("trying to res player");
                revivePlayer();
            }
            LobbySceneManagement.singleton.playerCamObject.GetComponentInChildren<WaveCounterHUD>().newWave(secsIn, waveCount, isBreak);
            
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void decrementSecondServerRpc() {
        if (IsServer) {
            Debug.Log("second passed on server");
            decrementSecondClientRpc();
        }
    }

    [ClientRpc]
    public void decrementSecondClientRpc() {
        if (IsClient) {
            Debug.Log("second passed on client");
            LobbySceneManagement.singleton.playerCamObject.GetComponentInChildren<WaveCounterHUD>().decrementSecond();
        }
    }

    public void playerDies() {
        Debug.Log("killing player");
        LobbySceneManagement.singleton.getLocalPlayer().GetComponent<FirstPersonMovement>().isMovementEnabled = false;
        LobbySceneManagement.singleton.dead = true;
        isDead = true;
        PlayerManager mng = LobbySceneManagement.singleton.playerCamObject.GetComponent<PlayerManager>();
        mng.alive = false;
        foreach (Animator anim in animatorsList) {
            if (anim.gameObject.active == true) {
                Debug.Log("Making mesh die");
                anim.SetBool("Death", true);
            }
        }
        //GameObject[] children = LobbySceneManagement.singleton.playerCamObject.GetComponentsInChildren<GameObject>();
        if (holder == null) {
            holder = LobbySceneManagement.singleton.playerCamObject.transform.Find("Holder").gameObject;
        }
        if (gunStuff == null) {
            gunStuff = LobbySceneManagement.singleton.playerCamObject.transform.Find("GunManager").gameObject;
        }
        holder.SetActive(false);
        gunStuff.SetActive(false);
        /*
        foreach (GameObject child in children) {
            child.SetActive(false);
        }*/
    }

    public void revivePlayer() {
        Debug.Log("resing player");
        if (holder == null) {
            holder = LobbySceneManagement.singleton.playerCamObject.transform.Find("Holder").gameObject;
        }
        if (gunStuff == null) {
            gunStuff = LobbySceneManagement.singleton.playerCamObject.transform.Find("GunManager").gameObject;
        }
        LobbySceneManagement.singleton.getLocalPlayer().GetComponent<FirstPersonMovement>().isMovementEnabled = true;
        LobbySceneManagement.singleton.dead = false;
        PlayerManager mng = LobbySceneManagement.singleton.playerCamObject.GetComponent<PlayerManager>();
        mng.alive = true;
        LobbySceneManagement.singleton.getLocalPlayer().isDead = false;
        mng.receiveHealth(mng.maxHealth, LobbySceneManagement.singleton.getLocalPlayer().identity);
        holder.SetActive(true);
        gunStuff.SetActive(true);
        foreach (Animator anim in animatorsList) {
            if (anim.gameObject.active == true) {
                Debug.Log("Making mesh live");
                anim.SetBool("Death", false);
            }
        }
        //Resets health
        mng.canDamage = true;
        mng.currentHealth = mng.maxHealth;
        mng.healthBar.setHealth(mng.maxHealth);

        //Zeroes money
        LobbySceneManagement.singleton.playerCamObject.gameObject.GetComponent<AddMoney>().money = 0;

        //Resets guns
        LobbySceneManagement.singleton.playerCamObject.gameObject.GetComponentInChildren<FixedGunManager>().ResetGuns();

        //Resets nades
        LobbySceneManagement.singleton.playerCamObject.gameObject.GetComponentInChildren<GrenadeThrower>().totalgrenades = 3;

        
        /*
        GameObject[] children = LobbySceneManagement.singleton.playerCamObject.GetComponentsInChildren<GameObject>();
        foreach (GameObject child in children) {
            child.SetActive(true);
        }*/

        

        gameObject.transform.position = LobbySceneManagement.singleton.playerSpawnZone.position;
    }

}  

//General structure: local object - singleton function - register player server rpc - register player client rpc

//Client managers need to copy host manager data on spawn