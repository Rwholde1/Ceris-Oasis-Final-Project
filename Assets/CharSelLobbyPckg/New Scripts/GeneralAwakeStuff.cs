using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAwakeStuff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        Debug.Log("registering new player");
        //LobbySceneManagement.singleton.initializeNewPlayerManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
