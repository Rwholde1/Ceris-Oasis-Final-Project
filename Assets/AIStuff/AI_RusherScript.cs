using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_RusherScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    PlayerAttributes player;
    public float damage;

    public float timeBetweenAttacks;

    void Start()
    {
        genState = GetComponent<AI_Gen_State>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributes>();

        genState.timeAttack = timeBetweenAttacks;
        damage = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (genState.doAttack)
        {
            rusherAttack();

        }
    }

    void rusherAttack()
    {
        if (genState.CastToPlayer(5f))
        {
            Debug.Log("ATTACK HIT THE PLAYER");
            player.Damage(damage);
        }

    }
}