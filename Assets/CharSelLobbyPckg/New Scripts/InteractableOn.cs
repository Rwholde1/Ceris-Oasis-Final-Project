/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class InteractableOn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ClearChildren();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearChildren()
    {
        Debug.Log(transform.childCount);
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            //DestroyImmediate(child.gameObject);
            if (child is UI.Selectable) {
                child.interactable = true;
                Debug.Log(child + " has been toggled on");
            }
        }

        Debug.Log(transform.childCount);
    }
}
*/