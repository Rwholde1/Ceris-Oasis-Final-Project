using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_WandererScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    PlayerAttributes player;
    public float ADT;
    public bool doSearch;
    public float damage;
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
        damage = 15f; //Default 15
        doSearch = true;
        health = 100f;
        genState = GetComponent<AI_Gen_State>();
        
        animator = GetComponent<Animator>();

        doAttack = genState.doAttack;
        damage *= genState.attackDamageModifier;
        genState.doSearch = doSearch;
        genState.timeAttack = timeBetweenAttacks;
        genState.ADT = ADT;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        player = genState.targetObject.GetComponent<PlayerAttributes>();
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
    void Update()
    {
        if (health <= 0)
        {

            animator.Play("Death");
            genState.state = AI_Gen_State.AI_STATE.DEAD;
        }
        doAttack = genState.doAttack;
        doSearch = genState.doSearch;
        isAnimating = genState.isAnimating;
        doAttack = genState.doAttack;
        if (doAttack)
        {
            wanderAttack();

        }
    }

    void wanderAttack()
    {

        player = genState.targetObject.GetComponent<PlayerAttributes>();
        if (genState.CastToPlayer(ADT))
            {
                
                Debug.Log("ATTACK HIT THE PLAYER");
                player.Damage(damage);
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
