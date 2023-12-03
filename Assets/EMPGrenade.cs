using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EMPGrenade : MonoBehaviour
{
    public float radius;
    public float delay = 3f;
    public GameObject explosion;
    public int damage;
    bool hasexploaded = false;
    public GameObject player;
    int moneytosendtoplayer;
        float countdown;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0 && !hasexploaded)
        {
            Explode();
            hasexploaded=true;
        }
    }
    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyobject in colliders)
        {
            target target = nearbyobject.GetComponent<target>();
            if (target != null)
            {
                float distance = Vector3.Distance(nearbyobject.transform.position, transform.position);
                float distancePercentage = Mathf.Clamp01(radius / distance );
                
                if (target.TakeDamage(damage * distancePercentage))
                {
                    moneytosendtoplayer = target.gimmemoney();
                    sendmoney();
                }
            }
        }

        Destroy(explosion, 3 );
        Destroy(gameObject, 4);
    }
    void sendmoney()
    {

        player.GetComponentInParent<AddMoney>().money += moneytosendtoplayer;
        moneytosendtoplayer = 0;
    }
}

