using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class registerCoordinateReference : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbySceneManagement.singleton.coordReference == null) {
            LobbySceneManagement.singleton.coordReference = this.transform.position;
        }
    }
}
