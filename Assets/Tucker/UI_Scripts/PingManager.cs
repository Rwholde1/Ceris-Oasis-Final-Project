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

    float dist;
    // Start is called before the first frame update
    void Start()
    {
        //player = /*GameObject.FindWithTag("Player");*/ (GameObject) NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        target = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<Transform>();
        lifetime = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime <= 0)
            Destroy(gameObject);
        lifetime -= Time.deltaTime;

        //Orients ping to player
        transform.LookAt(target, Vector3.up);
        Vector3 retarget = Vector3.left * transform.localEulerAngles[0];
        transform.Rotate(retarget);

        //Determines distance and updates TMP
        dist = Vector3.Distance(transform.position, target.transform.position);
        distanceText.text = Mathf.Round(dist) + "m";

        if (dist > MinDist) {
            float ratio = dist / MinDist * scaleFactor * scaleFactor;
            if (ratio < scaleFactor) {
                ratio = scaleFactor;
            }
            transform.localScale = Vector3.one * ratio;
        } else {
            transform.localScale = Vector3.one * scaleFactor;
        }

    }
}
