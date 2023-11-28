using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private CharacterSelectButton selectButtonPrefab;
    [SerializeField] private PlayerCard[] playerCards;
    [SerializeField] private Image[] playerReadyIcons;
    [SerializeField] private Image[] playerCharIcons;
    //[SerializeField] private GameObject characterInfoPanel;
    //[SerializeField] private TMP_Text characterNameText;
    //[SerializeField] private Transform introSpawnPoint;
    [SerializeField] private TMP_Text joinCodePlaceholder;    
    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private Button lockInButton;

    public Sprite unready;
    public Sprite ready;

    private GameObject introInstance;
    public List<CharacterSelectButton> characterButtons = new List<CharacterSelectButton>();
    public NetworkList<CharacterSelectState> players;

    public TMP_Text lobbyHeader;

    public float readyWaitTime = 5f;
    public float currentReadyWait = 0f;
    public bool countingDown = false;
    public TMP_Text timerText;

    private void Awake()
    {
        players = new NetworkList<CharacterSelectState>();

        //
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = true;
        canvasGroup.enabled = true;
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            Character[] allCharacters = characterDatabase.GetAllCharacters();

            foreach (var character in allCharacters)
            {
                var selectbuttonInstance = Instantiate(selectButtonPrefab, charactersHolder);
                selectbuttonInstance.SetCharacter(this, character);
                characterButtons.Add(selectbuttonInstance);
            }

            players.OnListChanged += HandlePlayersStateChanged;

            joinCodeText.SetText("" + HostSingleton.Instance.RelayHostData.JoinCode);
            joinCodePlaceholder.SetText("");
            //LobbySceneManagement.singleton
            LobbySceneManagement.singleton.joinCodeText = joinCodeText;
            LobbySceneManagement.singleton.joinCode = "" + HostSingleton.Instance.RelayHostData.JoinCode;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }

        if (IsHost)
        {
            joinCodeText.SetText("" + HostSingleton.Instance.RelayHostData.JoinCode);
            joinCodePlaceholder.SetText("");
            //LobbySceneManagement.singleton
            LobbySceneManagement.singleton.joinCodeText = joinCodeText;
            LobbySceneManagement.singleton.joinCode = "" + HostSingleton.Instance.RelayHostData.JoinCode;
            //Debug.Log("Join code: " + HostSingleton.Instance.RelayHostData.JoinCode);
        }
        

    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            players.OnListChanged -= HandlePlayersStateChanged;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        players.Add(new CharacterSelectState(clientId));
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != clientId) { continue; }

            players.RemoveAt(i);
            break;
        }
    }

    public void Select(Character character)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log("character: " + character);
            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { continue; }

            if (players[i].IsLockedIn) { return; }

            if (players[i].CharacterId == character.Id) { return; }

            if (IsCharacterTaken(character.Id, false)) { return; }
        }

        //characterNameText.text = character.DisplayName;

        //characterInfoPanel.SetActive(true);

        if (introInstance != null)
        {
            Destroy(introInstance);
        }

        //introInstance = Instantiate(character.IntroPrefab, introSpawnPoint);
        Debug.Log("made it here");
        SelectServerRpc(character.Id);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {   
        Debug.Log("made it to SelectServerRPC");
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }

            if (!characterDatabase.IsValidCharacterId(characterId)) { return; }

            if (IsCharacterTaken(characterId, true)) { return; }

            players[i] = new CharacterSelectState(
                players[i].ClientId,
                characterId,
                players[i].IsLockedIn
            );
        }
    }

    public void LockIn()
    {

        /*
        for (int i = 0; i < players.Count; i++)
        {               
            if (!characterDatabase.IsValidCharacterId(players[i].CharacterId)) { return; }

            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { return; }

            if (players[i])

            var button = characterButtons[i].GetComponentInChildren<Button>();
            if (!players[i].IsLockedIn) {
                Debug.Log("Confirm local lock in " + characterButtons[i].name);
                button.enabled = false;
                //characterButtons[i].GetComponent<Button>().interactable = false;
            } else {
                Debug.Log("Undo local lock in " + characterButtons[i].name);
                button.enabled = true;
                //characterButtons[i].GetComponent<Button>().interactable = true;
            }
        }*/

        LockInServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void LockInServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Lockin server rpc");
        for (int i = 0; i < players.Count; i++)
        {   
            Debug.Log(i + "Client ID: " + players[i].ClientId + " Char ID: " + players[i].CharacterId + " ");
            
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }

            if (!characterDatabase.IsValidCharacterId(players[i].CharacterId)) { return; }

            //if (IsCharacterTaken(players[i].CharacterId, true)) { return; }

            if (!players[i].IsLockedIn) {
                Debug.Log("Confirm lock in " + characterButtons[i].name);
                players[i] = new CharacterSelectState(
                players[i].ClientId,
                players[i].CharacterId,
                true);
                //characterButtons[i].GetComponent<Button>().enabled = false;
                //characterButtons[i].GetComponent<Button>().interactable = false;
            } else {
                Debug.Log("Undo lock in " + characterButtons[i].name);
                players[i] = new CharacterSelectState(
                players[i].ClientId,
                players[i].CharacterId,
                false);
                //characterButtons[i].GetComponent<Button>().enabled = true;
                //characterButtons[i].GetComponent<Button>().interactable = true;
            }
            
        }

        //Ensures everyone is locked in
        foreach (var player in players)
        {
            if (!player.IsLockedIn) { 
                if (countingDown) {
                    stopTimer();
                }
                return; 
            }
        }
        Debug.Log("Prefab assignment");
        //Assigns players to prefabs for load-in
        foreach (var player in players)
        {
            MatchplayNetworkServer.Instance.SetCharacter(player.ClientId, player.CharacterId);
        }

        //Add countdown here
        if (!countingDown) {
            startTimer();
        }
        //MatchplayNetworkServer.Instance.StartGame();
    }

    private void HandlePlayersStateChanged(NetworkListEvent<CharacterSelectState> changeEvent)
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (players.Count > i)
            {
                playerCards[i].UpdateDisplay(players[i]);
            }
            else
            {
                playerCards[i].DisableDisplay();
            }
        }

        
        foreach (var button in characterButtons)
        {
            if (button.IsDisabled) { 
                if (!IsCharacterTaken(button.Character.Id, true)) {
                    Debug.Log("button enabled");
                    button.SetEnabled();
                }
            } else {
                if (IsCharacterTaken(button.Character.Id, false)) {
                    Debug.Log("button disabled");
                    button.SetDisabled();
                }
            }

            
            
                        Debug.Log(button.IsDisabled);

        }
        /*
        var ind = -1;
        foreach (var player in players)
        {
            ind++;
            if (player.ClientId != NetworkManager.Singleton.LocalClientId) { continue; }

            Debug.Log("IDS: " + players[ind].ClientId + " " + player.ClientId);
            if (player.IsLockedIn && players[ind].ClientId == player.ClientId)
            {   
                characterButtons[ind].button.interactable = false;
                Debug.Log("turned off button " + (ind + 1));
                //lockInButton.interactable = false;
                //break;
            } else {
                Debug.Log("turned on button " + (ind + 1));
                characterButtons[ind].button.interactable = true;
            }
            
            if (IsCharacterTaken(player.CharacterId, false))
            {
                //lockInButton.interactable = false;
                break;
            }

            //lockInButton.interactable = true;

            //break;
        }
        */
    }

    private bool IsCharacterTaken(int characterId, bool checkAll)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (!checkAll)
            {
                if (players[i].ClientId == NetworkManager.Singleton.LocalClientId) { continue; }
            }

            if (players[i].IsLockedIn && players[i].CharacterId == characterId)
            {
                return true;
            }
        }

        return false;
    }

    //
    void Update() {
        if (LobbySceneManagement.singleton.joinCodeText == null) {
            LobbySceneManagement.singleton.joinCodeText = joinCodeText;
            LobbySceneManagement.singleton.joinCode = "" + HostSingleton.Instance.RelayHostData.JoinCode;
        }

        for (int r = 0; r < 4; r++) {
            //Player lobby info (name, ready state)
            for (int c1 = 0; c1 < 2; c1++) {
                if (c1 == 1 && LobbySceneManagement.singleton.playerLobbyInfo[r, c1] != "ready") {
                    playerReadyIcons[r].sprite = unready;
                } else {
                    playerReadyIcons[r].sprite = ready;
                }
            }
        }  


        CharacterSelectButton currentButton = null;
        foreach (CharacterSelectButton buttn in characterButtons) {
            if (buttn.currentlySelected) {
                currentButton = buttn;
            }
        }

        if (playerCharIcons[/*LobbySceneManagement.singleton.localPlayerID - 1*/LobbySceneManagement.singleton.getLocalPlayerNetworkID()].sprite == null || currentButton.IsDisabled) {
            lockInButton.interactable = false;
        } else {
            lockInButton.interactable = true;
        }

        if (LobbySceneManagement.singleton.LobbyHeader == null) {
            LobbySceneManagement.singleton.LobbyHeader = lobbyHeader;
            LobbySceneManagement.singleton.levelSelector.changeLevel();
            //LobbySceneManagement.singleton.updateSelectedLevel();

        }



        //Ensures everyone is locked in
        foreach (var player in players)
        {
            if (!player.IsLockedIn) { 
                if (countingDown) {
                    stopTimer();
                }
                return; 
            }
        }

        //Add countdown here
        if (!countingDown) {
            startTimer();
        }



        if (countingDown) {
            if (currentReadyWait <= 0f) {
                countingDown = false;
                Debug.Log("Starting game instance");
                timerText.SetText("Waiting for all to ready up");
                MatchplayNetworkServer.Instance.StartGame();
            }
            Debug.Log("counting down");
            timerText.SetText("Starting in " + Mathf.Ceil(currentReadyWait) + "s");
            currentReadyWait -= Time.deltaTime;
        }
    }

    public void startTimer() {
        currentReadyWait = readyWaitTime;
        countingDown = true;
    }

    public void stopTimer() {
        currentReadyWait = 0f;
        countingDown = false;
        timerText.SetText("Waiting for all to ready up");
    }
}
