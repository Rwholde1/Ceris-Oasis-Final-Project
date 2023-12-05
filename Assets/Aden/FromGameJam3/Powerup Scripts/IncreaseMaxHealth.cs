/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxHealth : MonoBehaviour
{
    public float increase = 20f;
    [SerializeField] private GameObject player;
    private PlayerAttributes pAt;
    //Field for UI Scripts

    // Start is called before the first frame update
    void Start()
    {
        pAt = player.GetComponent<PlayerAttributes>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pAt.IncreaseMaxHealth(increase);
            //maybe call method to add image to set of powerups / increase number on one power-up
            Destroy(gameObject);
        }
    }
}
*/