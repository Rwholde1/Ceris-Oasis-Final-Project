using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{

    bool go;//Will Be Used To Change Direction Of Weapon

    RegisterPlayer player;//Reference To The Main Character
    //GameObject sword;//Reference To The Main Character's Weapon
    [SerializeField] private Transform child;

    [SerializeField] private float distanceforward;
    Vector3 locationInFrontOfPlayer;//Location In Front Of Player To Travel To
    public int pID;
    public float halfTime = 30f; //How long boomerang shgould travel before returning
    public int damage = 50;
    public float timePassed = 0f;


    // Use this for initialization
    void Start()
    {
        go = true; //Set To Not Return Yet

        //sword = GameObject.Find("Sword");//The Weapon The Character Is Holding In The Scene

        //player = GameObject.Find("Boomerang Alien");
        player = LobbySceneManagement.singleton.getPlayerByCharID(3);
        Debug.Log("player on boomer is: " + player);
        //TUCKERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR********************************************************************************************************
        //player = LobbySceneManagement.singleton.playerCamObject.transform.gameObject;


        ///sword.GetComponent<MeshRenderer>().enabled = false; //Turn Off The Mesh Render To Make The Weapon Invisible

        //Find The Weapon That Is The Child Of The Empty Object       

        //Adjust The Location Of The Player Accordingly, Here I Add To The Y position So That The Object Doesn't Go Too Low ...Also Pick A Location In Front Of The Player
        locationInFrontOfPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z) + player.transform.forward * distanceforward;
        Debug.Log("loc. in front of player is " + locationInFrontOfPlayer);

        Boom();
    }

    void Boom()
    {
        Debug.Log("in boom");
        go = true;
        //yield return new WaitForSeconds(halfTime);//Any Amount Of Time You Want
        //go = false;
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(timePassed + " " + halfTime + " "  + go);
        if (timePassed >= halfTime) {
            Debug.Log("disabling go");
            go = false;
        }

        timePassed += Time.deltaTime;
        Debug.Log("boomer is alive and going " + go);
        child.transform.Rotate(0, Time.deltaTime * 500, 0); //Rotate The Object

        if (go)
        {
            transform.position = Vector3.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * 40); //Change The Position To The Location In Front Of The Player            
        }

        if (!go)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Time.deltaTime * 40); //Return To Player
        }

        if (!go && Vector3.Distance(player.transform.position, transform.position) < 1.5)
        {
            //Once It Is Close To The Player, Make The Player's Normal Weapon Visible, and Destroy The Clone
            //sword.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("destroying boomer");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pID == LobbySceneManagement.singleton.getLocalPlayer().identity) {
            Debug.Log("boomer hit " + other);
            if(other.gameObject.tag != "Player" && other.gameObject.tag != "MainCamera" || other.gameObject.GetComponent<FirstPersonLook>() != null) { 
                /*
                if(other.GetComponent<target>() != null)
                {
                    //TUCKERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR
                }*/
                EnemyHitRegister enem = other.transform.GetComponentInChildren<EnemyHitRegister>();
                if (enem != null) {
                    Debug.Log("hit enemy");
                    enem.takeDamage(damage, pID, "Single");
                }
                go = false;
            }
        }
    }
}