using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_GunnerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    //PlayerAttributes player;
    //PlayerManager player;
    public int playerTargetID = -1;

    public float ADT;
    public bool doSearch;
    public float damage = 15f;

    private bool isAnimating;
    public Animator animator;

    public float health;

    [SerializeField] public bool doAttack;
    public float timeBetweenAttacks;
    public float timeBetweenSearch;

    void Start()
    {
        health = 100;                           
        ADT = 10f;                              //ATTACK DISTANCE
        timeBetweenAttacks = 3.5f;
        //timeBetweenSearch = 2.5f; redundant
        //damage = 15f;                           //DAMAGE
        doSearch = false;
        genState = GetComponent<AI_Gen_State>();
        
        animator = GetComponent<Animator>();

        doAttack = genState.doAttack;
        //damage *= genState.attackDamageModifier;
        genState.doSearch = doSearch;
        genState.timeAttack = timeBetweenAttacks;
        genState.ADT = ADT;
        isAnimating = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        doAttack = genState.doAttack;
        genState.doSearch = doSearch;
        if (doAttack)
        {
            rusherAttack();
        }
        if (genState.state == AI_Gen_State.AI_STATE.ACTIVECHASE && !isAnimating)
        {
            animator.Play("");
        }

        switch (genState.state)
        {
            case AI_Gen_State.AI_STATE.ACTIVECHASE:
                {
                    if (!isAnimating)
                    {
                        animator.Play("RunArmed");
                        genState.isAnimating = true;
                    }
                    break;
                }
        }*/
    }
    private void Update()
    {
        isAnimating = genState.isAnimating;

        doAttack = genState.doAttack;
        genState.doSearch = doSearch;
        if (doAttack)
        {
            rusherAttack();
            gameObject.transform.LookAt(genState.targetObject.transform, Vector3.up);
        }
        if (genState.state == AI_Gen_State.AI_STATE.ACTIVECHASE && !isAnimating)
        {
            animator.Play("");
        }

        switch (genState.state)
        {
            case AI_Gen_State.AI_STATE.ACTIVECHASE:
                {
                    if (!isAnimating)
                    {
                        animator.Play("RunArmed");
                        genState.isAnimating = true;
                    }
                    break;
                }
        }
        /*
        if (health <= 0)
        {
            Debug.Log("enemy " + this.gameObject + " died");
            animator.Play("DeathCombat");
            genState.state = AI_Gen_State.AI_STATE.DEAD;
        }*/
        
    }

    void rusherAttack()
    {
        //player = genState.targetObject.GetComponent<PlayerManager>();
        playerTargetID = genState.targetID;



        if (genState.CastToPlayer(ADT * 1.05f))
        {
            genState.doAttack = false;
            if (!isAnimating)
            {
                animator.Play("GetHitFrontAiming");
                genState.isAnimating = true;
            }

            Debug.Log("ATTACK HIT THE PLAYER");
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

