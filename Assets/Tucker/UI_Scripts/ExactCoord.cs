using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExactCoord : MonoBehaviour
{
    public TMP_Text thisText;
    public Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        thisText.SetText("" + Mathf.Floor(player.localEulerAngles.y));
    }
}
