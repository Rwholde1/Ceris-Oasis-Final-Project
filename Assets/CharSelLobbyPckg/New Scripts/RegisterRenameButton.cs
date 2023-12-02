using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterRenameButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbySceneManagement.singleton.renameButton == null) {
            LobbySceneManagement.singleton.renameButton = GetComponentInChildren<Button>();
        }
        if (LobbySceneManagement.singleton.localNameText == null) {
            LobbySceneManagement.singleton.localNameText = GetComponentInChildren<TMP_Text>();
        }
    }

}
