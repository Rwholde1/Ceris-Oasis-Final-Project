using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompleteRename : MonoBehaviour
{

    public GameObject renamePopup;
    public TMP_InputField renamePopupText;
    public GameObject errorPopup;
    public TMP_InputField errorPopupText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void finishRename() {
        //Empty name
        if (string.IsNullOrWhiteSpace(GetComponent<TMP_InputField>().text)) {
            
            errorPopup.SetActive(true);
            errorPopupText.text = "Empty Name Not Allowed: Please Choose A Longer Name";
            Debug.LogError("Empty name not allowed");
            return;
        }

        //Too long (> 10 chars)
        if (GetComponent<TMP_InputField>().text.Length > 10) {
            
            errorPopup.SetActive(true);
            errorPopupText.text = "Name Too Long: Must Be 10 Characters Or Less";
            Debug.LogError("Name too long (> 10)");
            return;
        }

        Debug.Log("local rename");
        LobbySceneManagement.singleton.renamePlayer(renamePopupText.text);
        //var id = LobbySceneManagement.singleton.localPlayerID;
        //LobbySceneManagement.singleton.playerLobbyInfo[LobbySceneManagement.singleton.mostRecentPlayerClick - 1, 0] = renamePopupText.text;
        LobbySceneManagement.singleton.playerLobbyInfo[LobbySceneManagement.singleton.getLocalPlayer().identity - 1, 0] = renamePopupText.text;
        //LobbySceneManagement.singleton.playerNamesText[id] = renamePopupText.text

        renamePopup.SetActive(false);

    }

    public void closePopup() {
        errorPopupText.text = "";
        errorPopup.SetActive(false);
    }
}
