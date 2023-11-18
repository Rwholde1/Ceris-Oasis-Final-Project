using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginScene : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject[] camsList = new GameObject[4];
    public bool[] camsTaken = new bool[4];
    
    // Start is called before the first frame update
    void Start()
    {
        camsList[0] = cam1;
        camsList[1] = cam2;
        camsList[2] = cam3;
        camsList[3] = cam4;
        foreach(GameObject cam in camsList) {
            cam.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public GameObject getFirstCam() {
        for (int i = 0; i < 4; i++) {
            if (!camsTaken[i]) {
                camsTaken[i] = true;
                return camsList[i];
            }
        }
        return null;
    }
}
