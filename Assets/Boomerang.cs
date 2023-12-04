using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{

    bool go;//Will Be Used To Change Direction Of Weapon

    GameObject player;//Reference To The Main Character
    //GameObject sword;//Reference To The Main Character's Weapon
    [SerializeField] private Transform child;

    [SerializeField] private float distanceforward;
    Vector3 locationInFrontOfPlayer;//Location In Front Of Player To Travel To


    // Use this for initialization
    void Start()
    {
        go = false; //Set To Not Return Yet

        //sword = GameObject.Find("Sword");//The Weapon The Character Is Holding In The Scene

        player = GameObject.Find("Boomerang Alien");
        //TUCKERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR********************************************************************************************************
        //player = LobbySceneManagement.singleton.playerCamObject.transform.gameObject;


        ///sword.GetComponent<MeshRenderer>().enabled = false; //Turn Off The Mesh Render To Make The Weapon Invisible

        //Find The Weapon That Is The Child Of The Empty Object       

        //Adjust The Location Of The Player Accordingly, Here I Add To The Y position So That The Object Doesn't Go Too Low ...Also Pick A Location In Front Of The Player
        locationInFrontOfPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z) + player.transform.forward * distanceforward;

        StartCoroutine(Boom());//Now Start The Coroutine
    }

    IEnumerator Boom()
    {
        go = true;
        yield return new WaitForSeconds(1.5f);//Any Amount Of Time You Want
        go = false;
    }


    // Update is called once per frame
    void Update()
    {
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
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player") { 
            if(other.GetComponent<target>() != null)
            {
                //TUCKERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR
            }
            go = false;
        }
    }
}