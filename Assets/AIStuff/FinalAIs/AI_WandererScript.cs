using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_WandererScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    //PlayerAttributes player;
    public int playerTargetID = -1;

    public float ADT;
    public bool doSearch;
    public float damage = 15f;
    public float health;
    private Animator animator;
    private bool isAnimating;

    [SerializeField] public bool doAttack;

    public float timeBetweenAttacks;
    public float timeBetweenSearch;

    
    void Start()
    {
        ADT = 2.75f;
        timeBetweenAttacks = 1.5f;
        timeBetweenSearch = 2.5f;
        //damage = 15f; //Default 15
        doSearch = true;
        health = 100f;
        genState = GetComponent<AI_Gen_State>();
        
        animator = GetComponent<Animator>();

        doAttack = genState.doAttack;
        //damage *= genState.attackDamageModifier;
        genState.doSearch = doSearch;
        genState.timeAttack = timeBetweenAttacks;
        genState.ADT = ADT;

        GetComponent<AI_Gen_State>().wanderScript = this;

    }

    // Update is called once per frame
    void Update()
    {   
        doAttack = genState.doAttack;
        
        isAnimating = genState.isAnimating;
        doAttack = genState.doAttack;
        if (doAttack)
        {
            wanderAttack();

        }
    
        genState.doSearch = doSearch;
        //player = genState.targetObject.GetComponent<PlayerAttributes>();
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
            case AI_Gen_State.AI_STATE.CHASE:
                {
                    if (!isAnimating)
                    {
                        animator.Play("Walk");
                        genState.isAnimating = true;
                    }
                    break;
                }
            case AI_Gen_State.AI_STATE.ATTACK:
                {
                    if (!isAnimating)
                    {
                        animator.Play("2HitComboClawsAttackForward");
                        genState.isAnimating = true;
                    }
                    break;
                }
        }
    }

        

    void wanderAttack()
    {
        playerTargetID = genState.targetID;

        //player = genState.targetObject.GetComponent<PlayerAttributes>();
        if (genState.CastToPlayer(ADT * 1.1f))
            {
                
                Debug.Log("ATTACK HIT THE PLAYER " + playerTargetID);
                LobbySceneManagement.singleton.getLocalPlayer().takeDamage(damage, playerTargetID + 1);                
                genState.doAttack = false;
                StartCoroutine(AttackCooldown());
            }


        
        
        if (!genState.CastToPlayer(ADT)&& doSearch)
        {
            //animator.Play("Walk");
            genState.isAnimating = false;
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
