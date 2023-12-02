using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.Networking;
using TMPro;

public class LobbySceneManagement : NetworkBehaviour
{
    public static LobbySceneManagement singleton = null;
    public int mostRecentPlayerClick;
    public bool[] camsTaken = new bool[4];
    public RegisterPlayer[] players = new RegisterPlayer[4];

    [SerializeField] public TMP_Text joinCodeText;
    public string joinCode;

    
    [SerializeField] public PlayerCard[] playerCards;
    [SerializeField] public TMP_Text[] playerNames;
    [SerializeField] public bool[] playerReady;
    //[SerializeField] private PlayerCard[] playerCards;



    [SerializeField] public GameObject renameButtonHolder;
    public Button renameButton;
    public TMP_Text localNameText;

    private Object localPlayerVar;
    public int localPlayerID; //not working correctly rn

    public string[,] playerLobbyInfo = new string[4, 2]; //First col names, second col ready status
    public int[,] statsArray = new int[4, 4]; //kills, assists, deaths, damage
    //Add array to store character choices? or find it in other scripts

    public TMP_Text LobbyHeader;
    public LevelSelector levelSelector;
    public string SceneToPlay = "Epsilon";

    //Ping Stuff
    public NetworkObject pingPrefab;

    //public GameObject cam1;
    //public GameObject cam2;
    //public GameObject cam3;
    //public GameObject cam4;

    //public GameObject[] camsList = new GameObject[4];
    public int[] layersList = new int[4];

    int layer1;
    int layer2;
    int layer3;
    int layer4;

    public PingManager pingManage;

    public Transform playerSpawnZone;
    public float playerSpawnZoneRadius;

    

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
        //Ping stuff
        //camsList[0] = cam1;
        //camsList[1] = cam2;
        //camsList[2] = cam3;
        //camsList[3] = cam4;
        /*foreach(GameObject cam in camsList) {
            cam.SetActive(false);
        }*/

        layer1 = LayerMask.NameToLayer("Ping1");
        layer2 = LayerMask.NameToLayer("Ping2");
        layer3 = LayerMask.NameToLayer("Ping3");
        layer4 = LayerMask.NameToLayer("Ping4");

        layersList[0] = layer1;
        layersList[1] = layer2;
        layersList[2] = layer3;
        layersList[3] = layer4;

        Debug.Log("Layer IDs: " + layer1 + " " + layer2 + " " + layer3 + " " + layer4 + " ");

        pingManage = pingPrefab.GetComponent<PingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("alive");
        if (renameButton == null) {
            renameButton = renameButtonHolder.GetComponent<Button>();
        }
        if (joinCodeText.text.Length == 0) {
            joinCodeText.SetText("" + joinCode);
        }


        if (localPlayerVar == null) {
            localPlayerVar = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            Debug.Log("trying to initialize");
            if (localPlayerVar != null) {
                initializeNewPlayerManager();
            } 
        }

        for (int r = 0; r < 4; r++) {
            //Player lobby info (name, ready state)
            for (int c1 = 0; c1 < 2; c1++) {
                if (c1 == 0 && playerLobbyInfo[r, c1] != playerNames[r].text && playerLobbyInfo[r, c1] != null) {
                    playerNames[r].text = playerLobbyInfo[r, c1];
                }
            }
        }

        if(pingManage == null) {
            pingManage = pingPrefab.GetComponent<PingManager>();
        }


  
    }

    //void updatePlayerCards

    public int identifyPlayer(RegisterPlayer player) {
        for (int i = 0; i < 4; i++) {
            if (!camsTaken[i]) {
                //camsTaken[i] = true;
                Debug.Log("sent transform " + (i + 1));
                localPlayerID = i + 1;
                player.identifyThisPlayerServerRpc(i);
                //players[i] = player;
                return i + 1;
            }
        }
        return -1;
    }

    public RegisterPlayer getLocalPlayer() {
        /*
        foreach(RegisterPlayer player in players) {
            if (player.IsLocalPlayer) {
                return player;
            }
        }
        return null;
        */
        return NetworkManager.LocalClient.PlayerObject.GetComponent<RegisterPlayer>();
    }

    public ulong getLocalPlayerNetworkID() {
        return NetworkManager.LocalClient.ClientId;
    }

    public Transform getLocalPlayerTransform() {
        return NetworkManager.LocalClient.PlayerObject.GetComponent<Transform>();
    }

    /*
    public void Clicked(object sender, System.EventArgs e) {
        if (IsLocalPlayer) {
            Debug.Log("called click server rpc");
            int PlayerIdentifier = (sender as RegisterPlayer).identity;
            ClickedServerRpc(PlayerIdentifier);
        }
    }*/

    public void Clicked(object sender, System.EventArgs e) {
        Debug.Log("player clicked manager");
        int PlayerIdentifier = (sender as RegisterPlayer).identity;
        if (PlayerIdentifier == getLocalPlayer().identity) {
            mostRecentPlayerClick = PlayerIdentifier;
            Debug.Log(mostRecentPlayerClick);
            (sender as RegisterPlayer).ClickedServerRpc(mostRecentPlayerClick);
        }
    }

    public void renamePlayer(string text) {
        localNameText.SetText(text);
        getLocalPlayer().renamePlayerServerRpc(text);
    }

    public void reIDPlayer() {
        getLocalPlayer().identifyThisPlayerServerRpc(getLocalPlayer().identity - 1);
    }
    

    /*
    [ServerRpc(RequireOwnership = false)]
    //Renames player on playercard
    public void renamePlayerServerRpc(int identity) {
        Debug.Log("Player " + identity + " was here");
        LobbySceneManagement.singleton.playerNames[identity - 1].SetText("Player " + identity + " was here");
    }
    */

    public void initializeNewPlayerManager() {
        Debug.Log("initializing new player: " + /*NetworkManager.Singleton.LocalClient.PlayerObject.name*/NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject());
        NetworkManager.LocalClient.PlayerObject.GetComponent<RegisterPlayer>().startManagerServerRpc();
    }

    public void changeReady(string ready) {
        getLocalPlayer().changeReadyServerRpc(ready);
    }

    public void updateSelectedLevel() {
        getLocalPlayer().updateSelectedLevelServerRpc();
    }

    public int getPingLayer() {
        //camsTaken[i] = true;
        //Debug.Log("sent transform " + (i + 1) + " . " + pingManage);
        //players[i] = playerTransform;
        layer1 = LayerMask.NameToLayer("Ping1");
        layer2 = LayerMask.NameToLayer("Ping2");
        layer3 = LayerMask.NameToLayer("Ping3");
        layer4 = LayerMask.NameToLayer("Ping4");

        layersList[0] = layer1;
        layersList[1] = layer2;
        layersList[2] = layer3;
        layersList[3] = layer4;

        pingManage.setTarget(getLocalPlayer().identity, getLocalPlayerTransform());
        Debug.Log("Layer Return: " + layersList[getLocalPlayer().identity - 1]);
        return layersList[getLocalPlayer().identity - 1];
    }

}
