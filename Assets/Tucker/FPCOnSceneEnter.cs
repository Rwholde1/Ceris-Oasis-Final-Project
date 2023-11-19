using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class FPCOnSceneEnter : NetworkBehaviour
{   
    public GameObject thisCam;
    public GameObject controller;
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
            if (controller == null) {
                controller = GameObject.FindWithTag("GameController");
                Debug.Log(controller + " controller");
            } else {
                var beginScene = controller.GetComponent<BeginScene>();
                Debug.Log(beginScene +" scene begin");
                if (beginScene != null) {
                    sceneBegin = true;
                    //Edit here
                    //thisCam = beginScene.getFirstCam();
                    //thisCam.SetActive(true);
                    //Debug.Log(thisCam + " cam");
                    Debug.Log(GetComponent<Transform>());
                    int thisCullLayer = beginScene.getPingLayer(GetComponent<Transform>());
                    pingCam.cullingMask = pingCam.cullingMask ^ (1<<thisCullLayer);
                }
            }
        } else {
            


        }

        
    }
}
