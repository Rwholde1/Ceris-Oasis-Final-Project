using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_LobberScript : MonoBehaviour
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
    public float timeBetweenSearch;

    void Awake()
    {
        ADT = 7.5f;
        timeBetweenAttacks = 3.5f;
        timeBetweenSearch = 2.5f;
        damage = 15f;
        doSearch = true;
    }
    void Start()
    {

        genState = GetComponent<AI_Gen_State>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributes>();


        doAttack = genState.doAttack;
        damage *= genState.attackDamageModifier;
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
        doSearch = genState.doSearch;
        if (doAttack)
        {
            genState.doAttack = false;
            if (genState.CastToPlayer(ADT))
            {
                Debug.Log("ATTACK HIT THE PLAYER");
                player.Damage(damage);
                StartCoroutine(AttackCooldown());
            }


        }
        if (!genState.CastToPlayer(ADT) && doSearch)
        {
            genState.state = AI_Gen_State.AI_STATE.CHASE;
            genState.doSearch = false;
            StartCoroutine(SearchRefresh());

        }

    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(5f);
        genState.doAttack = true;

    }
    IEnumerator SearchRefresh()
    {
        yield return new WaitForSeconds(2f);
        genState.doSearch = true;

    }

}

