using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PingManager : NetworkBehaviour
{

    [SerializeField] float scaleFactor;
    [SerializeField] float MinDist;
    //private GameObject player;
    private Transform target;
    [SerializeField] TextMeshPro distanceText;
    [SerializeField] float lifetime;

    [SerializeField] GameObject ping1;
    [SerializeField] GameObject ping2;
    [SerializeField] GameObject ping3;
    [SerializeField] GameObject ping4;

    public Transform target1;
    public Transform target2;
    public Transform target3;
    public Transform target4;

    float dist;
    // Start is called before the first frame update
    void Start()
    {
        //player = /*GameObject.FindWithTag("Player");*/ (GameObject) NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        target1 = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Transform>();
        lifetime = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        target1 = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Transform>();
        repositionPing(ping1, target1);
        repositionPing(ping2, target2);
        repositionPing(ping3, target3);
        repositionPing(ping4, target4);
        
    }


    void repositionPing(GameObject ping, Transform target) {
    if (lifetime <= 0)
            Destroy(gameObject);
        lifetime -= Time.deltaTime;

        if (target == null)
            target = target1;

        //Orients ping to player
        ping.transform.LookAt(target, Vector3.up);
        Vector3 retarget = Vector3.left * ping.transform.localEulerAngles[0];
        ping.transform.Rotate(retarget);

        //Determines distance and updates TMP
        dist = Vector3.Distance(ping.transform.position, target.transform.position);
        distanceText.text = Mathf.Round(dist) + "m";

        if (dist > MinDist) {
            float ratio = dist / MinDist * scaleFactor * scaleFactor;
            if (ratio < scaleFactor) {
                ratio = scaleFactor;
            }
            ping.transform.localScale = Vector3.one * ratio;
        } else {
            ping.transform.localScale = Vector3.one * scaleFactor;
        }

}
}


