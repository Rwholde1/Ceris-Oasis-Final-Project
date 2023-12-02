using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUp : MonoBehaviour
{

    public GameObject readyPanel;
    public GameObject unreadyPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (LobbySceneManagement.singleton.playerLobbyInfo)
    }

    public void readyUp() {
        LobbySceneManagement.singleton.playerLobbyInfo[LobbySceneManagement.singleton.mostRecentPlayerClick - 1, 1] = "ready";
        LobbySceneManagement.singleton.changeReady("ready");
        unreadyPanel.SetActive(true);
        readyPanel.SetActive(false);
        
    }

    public void unready() {
        LobbySceneManagement.singleton.playerLobbyInfo[LobbySceneManagement.singleton.mostRecentPlayerClick - 1, 1] = "x";
        LobbySceneManagement.singleton.changeReady("x");
        readyPanel.SetActive(true);
        unreadyPanel.SetActive(false);
    }
}
