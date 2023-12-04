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
    public int pID;

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
        GameObject boom = Instantiate(explosion, transform.position, transform.rotation);
        boom.GetComponent<Transform>().eulerAngles = new Vector3(0f, 0f, 0f);

        Debug.Log("explodin time");
        if (pID == LobbySceneManagement.singleton.getLocalPlayer().identity) {
            //Debug.Log()
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider nearbyObject in colliders)
            {   
                /*
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
                }*/
                EnemyHitRegister enem = nearbyObject.transform.GetComponentInChildren<EnemyHitRegister>();
                if (enem != null) {
                    float distance = Vector3.Distance(nearbyObject.transform.position, transform.position);
                    float distancePercentage = Mathf.Clamp01(radius / distance );
                    enem.takeDamage((int) (damage * distancePercentage), pID, "Single");
                }
                //GameObject sphere = Instantiate(DamageSphere, fireEffect.transform.position, fireEffect.transform.rotation);
                //sphere.GetComponent<MolotovSphere>().player = player;
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

