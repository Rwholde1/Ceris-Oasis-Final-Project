using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_RusherScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    PlayerAttributes player;
    public float ADT;
    public bool doSearch;
    public float damage;

    [SerializeField] public bool doAttack;

    public float timeBetweenAttacks;

    void Start()
    {
        ADT = 1.5f;
        timeBetweenAttacks = 1.5f;
        damage = 10f;
        doSearch = false;
        genState = GetComponent<AI_Gen_State>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributes>();
        

        doAttack = genState.doAttack;

        genState.doSearch = doSearch;
        genState.timeAttack = timeBetweenAttacks;
        genState.ADT = ADT;
    }

    // Update is called once per frame
    void Update()
    {
        if (genState.CheckDistance() < ADT)
        {
            rusherAttack();

        }
    }

    void rusherAttack()
    {

        doAttack = genState.doAttack;
        if (doAttack)
        {
            genState.doAttack = false;
            if (genState.CastToPlayer(1.5f))
            {
                Debug.Log("ATTACK HIT THE PLAYER");
                player.Damage(damage);
                StartCoroutine(AttackCooldown());
            }
            

        }

        if (!genState.CastToPlayer(1.5f))
        {
            genState.state = AI_Gen_State.AI_STATE.ACTIVECHASE;
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(5f);
        genState.doAttack = true;

    }


}
