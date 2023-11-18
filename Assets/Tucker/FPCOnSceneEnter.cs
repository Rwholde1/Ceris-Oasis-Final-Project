using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Netcode;

public class FPCOnSceneEnter : MonoBehaviour
{   
    public GameObject thisCam;
    public GameObject controller;
    bool sceneBegin = false;

    // Start is called before the first frame update
    void Start()
    {   

    }

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
                    thisCam = beginScene.getFirstCam();
                    thisCam.SetActive(true);
                    Debug.Log(thisCam + " cam");
                }
            }
        } else {


        }
    }
}
