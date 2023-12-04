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
    bool isAnimating = false;
    public float timeBetweenAttacks;

    private Animator animator;
    void Start()
    {
        ADT = 1.5f;
        timeBetweenAttacks = 1.5f;
        damage = 10f;
        doSearch = false;
        genState = GetComponent<AI_Gen_State>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerAttributes>();
        animator = GetComponent<Animator>();
        

        doAttack = genState.doAttack;
        damage *= genState.attackDamageModifier;
        genState.doSearch = doSearch;
        genState.timeAttack = timeBetweenAttacks;
        genState.ADT = ADT;
    }

    // Update is called once per frame
    void Update()
    {
        isAnimating = genState.isAnimating;
        if (genState.CheckDistance() < ADT)
        {
            rusherAttack();

        }

        
    }
    private void FixedUpdate()
    {
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
            case AI_Gen_State.AI_STATE.ATTACK:
                {
                    if (!isAnimating)
                    {
                        animator.Play("Jump");
                        genState.isAnimating = true;
                    }
                    break;
                }
        }
    }
    void rusherAttack()
    {

        doAttack = genState.doAttack;
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

        if (!genState.CastToPlayer(ADT))
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
