using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MolotovSphere : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float damagepersecond;
    private int moneytosendtoplayer;
    private float applicationsPerSecond = 4f;
    private float timer;


    private void Start()
    {
        Destroy(gameObject, 7f);
        // Debug.Log("Sphere exists");

        timer = 1f / applicationsPerSecond;
    }
    private void OnTriggerStay(Collider other)
    {
        /*
         target enemy = other.gameObject.GetComponent<target>();
        if(other.GetComponent<target>() != null) 
        { 
            StartCoroutine(doDamage(enemy));
        }*/
        // /Debug.Log()
        if (timer <= 0f) {
            EnemyHitRegister enem = other.transform.GetComponentInChildren<EnemyHitRegister>();
            Debug.Log("damaging enem " + enem + " for " + damagepersecond + " per second");
            if (enem != null) {
                StartCoroutine(doDamage(enem));
            }
        }
    }

    void Update() {
        if (timer <= 0f) {
            timer = 1f / applicationsPerSecond;
        }
        timer -= Time.deltaTime;
        Debug.Log("fire timer: " + timer);
    }

    private IEnumerator doDamage(EnemyHitRegister enemy)
    {   
        Debug.Log("Do damage to enem");
        if (enemy != null) {
            Debug.Log("This is valid molotov enemy");
            enemy.takeDamage((int) (damagepersecond * 0.25f), LobbySceneManagement.singleton.getLocalPlayer().identity, "Sustained AOE");
        } else {
            yield return new WaitForSeconds(0.25f);
        }
        /*
        if (enemy.TakeDamage(damagepersecond * 0.25))
        {
            
            moneytosendtoplayer = enemy.gimmemoney();
            sendmoney();
        }
        else
        {

            yield return new WaitForSeconds(0.25f);
        }*/

    }
    void sendmoney()
    {
        if (player != null) {
            player.GetComponentInParent<AddMoney>().money += moneytosendtoplayer;
            moneytosendtoplayer = 0;
        }
        
    }
}
