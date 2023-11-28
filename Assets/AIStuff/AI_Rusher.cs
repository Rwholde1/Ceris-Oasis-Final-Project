using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    PlayerAttributes player;
    public float damage;

    public bool doAttack;

    public float timeBetweenAttacks;

    void Start()
    {
        genState = GetComponent<AI_Gen_State>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributes>();

        genState.timeAttack = timeBetweenAttacks;
        timeBetweenAttacks = 5f;
        doAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(genState.CheckDistance()<1f)
        {
            rusherAttack();

        }
    }

    void rusherAttack()
    {
        if (doAttack)
        {
            doAttack = false;
            if (genState.CastToPlayer(.5f))
            {
                Debug.Log("ATTACK HIT THE PLAYER");
                player.Damage(damage);
                //StartCoroutine(AttackCooldown());
            }
            
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(5f);
        doAttack = true;
    }


}
