using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePenetration : MonoBehaviour
{
    public int increase = 5;
    [SerializeField] private GameObject player;
    //get player health;
    //Field for UI Scripts

    // Start is called before the first frame update
    void Start()
    {
        //pull health script
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //call public method in boomerang script that increases rate by "increase"
            //maybe call method to add image to set of powerups / increase number on one power-up
            Destroy(gameObject);
        }
    }
}
