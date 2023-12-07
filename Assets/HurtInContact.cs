using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtInContact : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int damage;
    private float applicationsPerSecond = 2f;
    private float timer;

    void Start()
    {
        timer = 1f / applicationsPerSecond;

    }
    
    private void OnCollisionStay (Collision other)
    {
        /*
         target enemy = other.gameObject.GetComponent<target>();
        if(other.GetComponent<target>() != null) 
        { 
            StartCoroutine(doDamage(enemy));
        }*/
        // /Debug.Log()
        print("CLS");
        if (other.gameObject.transform.tag == "Player")
        {
            PlayerManager play = LobbySceneManagement.singleton.playerCamObject.GetComponent<PlayerManager>();

            print("CBS");
            Debug.Log("damage hit " + other.gameObject.name);
            //PlayerManager play = other.transform.GetComponentInParent<PlayerManager>();
            //if (play == null )
            //{
           //     int playerhit = other.gameObject.GetComponent<RegisterPlayer>().identity;
           // }
            Debug.Log("Damage player " + play + " for " + damage + " per second");
            if (play != null)
            {
                StartCoroutine(doDamage(play));
            }
        }
    }
    private IEnumerator doDamage(PlayerManager play)
    {
        Debug.Log("Do hurt to player");
        if (play != null)
        {
            Debug.Log("This is valid hurt target");

            //int id = play.meshPlayerID;
            //Debug.Log("target is player " + id);
            play.takeDamage((int)(damage * 0.25f), LobbySceneManagement.singleton.getLocalPlayer().identity);
            //Debug.Log(LobbySceneManagement.singleton.players[id - 1].gameObject.GetComponent<PlayerManager>());
            //LobbySceneManagement.singleton.players[id - 1].GetComponent<PlayerManager>().receiveHealth((int) (healthpersecond * 0.25f), LobbySceneManagement.singleton.getLocalPlayer().identity);
        }
        else
        {
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
}
