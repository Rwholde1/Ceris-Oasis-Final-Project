using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_RusherScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AI_Gen_State genState;
    AI_Gen_State state;
    //PlayerAttributes player;
    public int playerTargetID = -1;

    public float ADT;
    public bool doSearch;
    public float damage = 10f;
    //public float health;

    [SerializeField] public bool doAttack;
    bool isAnimating = false;
    public float timeBetweenAttacks;

    private Animator animator;

    
    void Start()
    {

        ADT = 2.0f;                              //ATTACK DISTANCE
        timeBetweenAttacks = 1.5f;
        //damage = 10f;
        doSearch = false;
        //health = 100f;
        genState = GetComponent<AI_Gen_State>();
        
        animator = GetComponent<Animator>();
        

        doAttack = genState.doAttack;
        //damage *= genState.attackDamageModifier;
        genState.doSearch = doSearch;
        genState.timeAttack = timeBetweenAttacks;
        genState.ADT = ADT;
    }


    
    void Update()
    {
        isAnimating = genState.isAnimating;
        if (doAttack)
        {
            rusherAttack();

        }

        doAttack = genState.doAttack;
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
        }
    }
    void rusherAttack()
    {
        //player = genState.targetObject.GetComponent<PlayerAttributes>();



        if (genState.CastToPlayer(ADT * 1f))
            {
            genState.doAttack = false;
            animator.Play("Jump");
                    genState.isAnimating = true;
                
                Debug.Log("ATTACK HIT THE PLAYER");
                LobbySceneManagement.singleton.getLocalPlayer().takeDamage(damage, playerTargetID + 1);

                StartCoroutine(AttackCooldown());
            }
            
            if (!genState.CastToPlayer(ADT))
            {
            genState.state = AI_Gen_State.AI_STATE.ACTIVECHASE;
            }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        genState.doAttack = true;

    }

    public void DieAnim() {
        animator.Play("JumpRM");
    }


}
