using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* CODE CREDIT to Zymu at https://forum.unity.com/threads/horizontal-compass-hud.381000/
*/
public class HorizontalCompass : MonoBehaviour
{
    public RawImage compass;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        compass.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);
    }
}
