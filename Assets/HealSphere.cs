using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealSphere : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float healthpersecond;
    //private int moneytosendtoplayer;
    private float applicationsPerSecond = 2f;
    private float timer;


    private void Start()
    {
        Destroy(gameObject, 7f);
        Debug.Log("Sphere exists");

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
            Debug.Log("heals hit " + other);
            PlayerManager play = other.transform.GetComponentInParent<PlayerManager>();
            Debug.Log("healing player " + play + " for " + healthpersecond + " per second");
            if (play != null) {
                StartCoroutine(doHeals(play));
            }
        }
    }

    void Update() {
        if (timer <= 0f) {
            timer = 1f / applicationsPerSecond;
        }
        timer -= Time.deltaTime;
        Debug.Log("heal timer: " + timer);
    }

    private IEnumerator doHeals(PlayerManager play)
    {   
        Debug.Log("Do heals to player");
        if (play != null) {
            Debug.Log("This is valid healing target");

            //int id = play.meshPlayerID;
            //Debug.Log("target is player " + id);
            play.receiveHealth((int) (healthpersecond * 0.25f), LobbySceneManagement.singleton.getLocalPlayer().identity);
            //Debug.Log(LobbySceneManagement.singleton.players[id - 1].gameObject.GetComponent<PlayerManager>());
            //LobbySceneManagement.singleton.players[id - 1].GetComponent<PlayerManager>().receiveHealth((int) (healthpersecond * 0.25f), LobbySceneManagement.singleton.getLocalPlayer().identity);
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
    /*
    void sendmoney()
    {
        if (player != null) {
            player.GetComponentInParent<AddMoney>().money += moneytosendtoplayer;
            moneytosendtoplayer = 0;
        }
        
    }*/
}
