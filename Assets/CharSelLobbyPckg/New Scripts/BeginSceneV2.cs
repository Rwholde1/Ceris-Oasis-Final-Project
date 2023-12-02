using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BeginSceneV2 : NetworkBehaviour
{
    //public NetworkObject pingPrefab;

    //public GameObject cam1;
    //public GameObject cam2;
    //public GameObject cam3;
   // public GameObject cam4;

    //public GameObject[] camsList = new GameObject[4];
    public bool[] camsTaken = new bool[4];
    //public int[] layersList = new int[4];
    public Transform[] players = new Transform[4];

    //int layer1;
    //int layer2;
    //int layer3;
    //int layer4;

    //public PingManager pingManage;


    
    // Start is called before the first frame update
    void Start()
    {

        //camsList[0] = cam1;
        //camsList[1] = cam2;
        //camsList[2] = cam3;
        //camsList[3] = cam4;
        //foreach(GameObject cam in camsList) {
            //cam.SetActive(false);
        //}

        //layer1 = LayerMask.NameToLayer("Ping1");
        //layer2 = LayerMask.NameToLayer("Ping2");
        //layer3 = LayerMask.NameToLayer("Ping3");
        //layer4 = LayerMask.NameToLayer("Ping4");

        //layersList[0] = layer1;
        //layersList[1] = layer2;
        //layersList[2] = layer3;
        //layersList[3] = layer4;

        //Debug.Log("Layer IDs: " + layer1 + " " + layer2 + " " + layer3 + " " + layer4 + " ");

        //pingManage = pingPrefab.GetComponent<PingManager>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    /*
    public GameObject getFirstCam() {
        for (int i = 0; i < 4; i++) {
            if (!camsTaken[i]) {
                camsTaken[i] = true;
                return camsList[i];
            }
        }
        return null;
    }
    */
    /*
    public int getPingLayer(Transform playerTransform) {
        for (int i = 0; i < 4; i++) {
            if (!camsTaken[i]) {
                camsTaken[i] = true;
                Debug.Log("sent transform " + (i + 1) + " . " + pingManage);
                players[i] = playerTransform;
                pingManage.setTarget(i + 1, playerTransform);
                Debug.Log("Layer Return: " + layersList[i]);
                return layersList[i];
            }
        }

        return -1;
    }
    */
     public void identifyPlayer(Transform playerTransform) {
        for (int i = 0; i < 4; i++) {
            if (!camsTaken[i]) {
                camsTaken[i] = true;
                Debug.Log("sent transform " + (i + 1));
                players[i] = playerTransform;
                //pingManage.setTarget(i + 1, playerTransform);
                /*
                switch(i) {
                    case 0:
                        pingManage.target1 = playerTransform;
                        break;
                    case 1:
                        pingManage.target2 = playerTransform;
                        break;
                    case 2:
                        pingManage.target3 = playerTransform;
                        break;
                    case 3:
                        pingManage.target4 = playerTransform;
                        break;
                    default:
                        break;
                }
                return layersList[i];
                */
                //Debug.Log("Layer Return: " + layersList[i]);
                //return layersList[i];
            }
        }

        //return -1;
    }
}

//Game currently doesn't update arrays when players leave