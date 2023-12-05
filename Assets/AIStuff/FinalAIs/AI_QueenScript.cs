using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_QueenScript : MonoBehaviour                 
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    
    //PlayerAttributes player;
    public int playerTargetID = -1;

    public float ADT;
    public bool doSearch;
    public float damage = 25f;

    private bool isAnimating;
    private Animator animator;

    public float health;

    [SerializeField] public bool doAttack;
    public float timeBetweenAttacks;
    public float timeBetweenSearch;

    void Start()
    {
        health = 100; //more health more likely
        ADT = 10f;                                      //distance to do attacks
        timeBetweenAttacks = 3.5f;
       
        //damage = 25f;
        doSearch = true;
        genState = GetComponent<AI_Gen_State>();

        animator = GetComponent<Animator>();

        doAttack = genState.doAttack;
        
        //damage *= genState.attackDamageModifier;
        
        genState.timeAttack = timeBetweenAttacks;
        genState.ADT = ADT;
        isAnimating = false;
    }
    // Update is called once per frame
    void Update()
    {
        isAnimating = genState.isAnimating;
        doAttack = genState.doAttack;
        doSearch = genState.doSearch;
        if (doAttack)
        {
            rusherAttack();
        }
        switch (genState.state)
        {
            case AI_Gen_State.AI_STATE.ACTIVECHASE:
                {
                    if (!isAnimating)
                    {
                        animator.Play("Walk");
                        genState.isAnimating = true;
                    }
                    break;
                }
        }

    }

    void rusherAttack()
    {
        //player = genState.targetObject.GetComponent<PlayerAttributes>();



        if (genState.CastToPlayer(ADT * 1.05f))
        {
            genState.doAttack = false;
            if (!isAnimating)
            {
                animator.Play("2HitComboClawsAttack2");
                genState.isAnimating = true;
            }

            Debug.Log("ATTACK HIT THE PLAYER");
            //player.Damage(damage);
            LobbySceneManagement.singleton.getLocalPlayer().takeDamage(damage, playerTargetID + 1);
            
            StartCoroutine(AttackCooldown());
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
        yield return new WaitForSeconds(timeBetweenAttacks);
        genState.doAttack = true;

    }
    IEnumerator SearchRefresh()
    {
        yield return new WaitForSeconds(2f);
        genState.doSearch = true;

    }

}

