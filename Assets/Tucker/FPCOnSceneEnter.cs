using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class FPCOnSceneEnter : NetworkBehaviour
{   
    //public GameObject thisCam;
    //public GameObject controller;
    bool sceneBegin = false;
    public NetworkObject playerCam;
    public int pingCullLayer = -1;
    public Camera pingCam;

    [SerializeField] private CharacterDatabase characterDatabase;

    public GameObject[] classMeshPrefabs = new GameObject[4];
    public Material[] classIconColors = new Material[4];

    public int meshPlayerID;

    // Start is called before the first frame update
    void Start()
    {   
        SceneManager.activeSceneChanged += getClientCharachter;
    }

    public void getClientCharachter (Scene lastScene, Scene scene) {
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsServer()) {
            foreach (var client in MatchplayNetworkServer.Instance.ClientData) {
                var character = characterDatabase.GetCharacterById(client.Value.characterId);
                Debug.Log("character for player " + client.Value.clientId + " is: " + character);
                //Debug.Log("character is: " + character);
                if (character != null)
                {   
                    Debug.Log("Local Player ID: " + LobbySceneManagement.singleton.getLocalPlayer().identity);
                    Debug.Log("Local Player Char ID: " + client.Value.characterId);
                    Debug.Log("Local Player Character: " + character);
                    Debug.Log("client id: " + client.Value.clientId);

                    /*
                    Debug.Log("spawning character: " + character);
                    var spawnPos = new Vector3(Random.Range(-3f, 3f), 10f, Random.Range(-3f, 3f));
                    var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity);
                    characterInstance.SpawnAsPlayerObject(client.Value.clientId);
                    */
                }
                //setClientCharacterServerRpc(client.Value.characterId - 1, LobbySceneManagement.singleton.getLocalPlayer().identity - 1);
                setClientCharacterServerRpc(client.Value.characterId - 1, (int) client.Value.clientId);
            }   
        }    

    }

    [ServerRpc(RequireOwnership = false)]
    public void setClientCharacterServerRpc(int charId, int playerId) {
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsServer()) {
            setClientCharacterClientRpc(charId, playerId);
        }
    }

    [ClientRpc]
    public void setClientCharacterClientRpc(int charId, int playerId) {
        Debug.Log("setting char on client, playerID: " + playerId);
        if (LobbySceneManagement.singleton.players[playerId] == gameObject.GetComponent<RegisterPlayer>()) {
            gameObject.GetComponent<RegisterPlayer>().charIdentity = charId;
            Debug.Log("passed match");
            meshPlayerID = playerId + 1;
            classMeshPrefabs[charId].SetActive(true);
            if (charId == 1) {
                Debug.Log("activating jetpack");
                gameObject.GetComponent<Jetpack>().enabled = true;
            } 
            if (charId == 2) {
                Debug.Log("activating boomer");
                gameObject.GetComponent<AkaneExample>().enabled = true;
            }
            //Molotovs handled elsewhere
            LobbySceneManagement.singleton.statsPlayerId.Add(playerId);
            LobbySceneManagement.singleton.statsCharId.Add(charId);
            Debug.Log("Fetching player cam to set icons"/* + LobbySceneManagement.singleton.playerCamObject*/);
            //Debug.Log(LobbySceneManagement.singleton.playerCamObject.GetComponentInChildren<StatsManager>());
            //LobbySceneManagement.singleton.playerCamObject.GetComponentInChildren<StatsManager>().setSprite(charId, playerId);
        }
    }

    /*
    void Awake() {
        NetworkObject cam = Instantiate(playerCam, Vector3.zero , Quaternion.identity);
        Debug.Log("Instantiated");
        cam.GetComponent<NetworkObject>().Spawn();
    }*/

    // Update is called once per frame
    void Update()
    {
        
        if (!sceneBegin && SceneManager.GetActiveScene().name != "LobbyScene") {
            //Edit here
            //thisCam = beginScene.getFirstCam();
            //thisCam.SetActive(true);
            //Debug.Log(thisCam + " cam");
            //Debug.Log(GetComponent<Transform>());
            int thisCullLayer = LobbySceneManagement.singleton.getPingLayer();
            pingCullLayer = thisCullLayer;
            Debug.Log("Cull Layer: " + thisCullLayer);
            pingCam.cullingMask = pingCam.cullingMask ^ (1<<thisCullLayer);
            if (thisCullLayer != -1) {
                sceneBegin = true;
            }
        } else {
            


        }

        
    }
}
