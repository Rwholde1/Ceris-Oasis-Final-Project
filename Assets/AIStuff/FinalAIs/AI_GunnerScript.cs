using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_GunnerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    PlayerAttributes player;
    public float ADT;
    public bool doSearch;
    public float damage;

    private bool isAnimating;
    private Animator animator;

    public float health;

    [SerializeField] public bool doAttack;
    public float timeBetweenAttacks;
    public float timeBetweenSearch;

    void Start()
    {
        health = 100;
        ADT = 10f;
        timeBetweenAttacks = 3.5f;
        timeBetweenSearch = 2.5f;
        damage = 15f;
        doSearch = true;
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
        isAnimating = genState.isAnimating;
        doAttack = genState.doAttack;
        doSearch = genState.doSearch;
        if (doAttack)
        {
            rusherAttack();
        }

    }
    private void Update()
    {
        if (health <= 0)
        {

            animator.Play("DeathCombat");
            genState.state = AI_Gen_State.AI_STATE.DEAD;
        }
    }

    void rusherAttack()
    {
        player = genState.targetObject.GetComponent<PlayerAttributes>();



        if (genState.CastToPlayer(ADT))
        {
            genState.doAttack = false;
            if (!isAnimating)
            {
                animator.Play("GetHitFrontAiming");
                genState.isAnimating = true;
            }

            Debug.Log("ATTACK HIT THE PLAYER");
            player.Damage(damage);
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

