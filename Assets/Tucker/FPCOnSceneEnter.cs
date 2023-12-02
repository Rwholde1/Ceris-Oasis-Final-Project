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

    // Start is called before the first frame update
    void Start()
    {   

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
        
        if (!sceneBegin) {
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
