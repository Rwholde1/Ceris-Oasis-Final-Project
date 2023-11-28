using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Gen_State : MonoBehaviour
{
    public enum AI_STATE
    {
        CHASE = 1,
        FLEE = 2,
        WAIT = 0,
        ATTACK = 3,
        ACTIVECHASE = 4,
    }

    public bool attackWhenClose;
    public float ADT; //Attack Distance Threshold


    [SerializeField] public AI_STATE state;
    private Transform enemyT;
    private float speed = 1f;

    public GameObject targetObject;


    public float timeAttack;
    public bool canAttack = true;
    public bool doAttack = false;
    public bool doSearch = true;

    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 targetPos;
    void Start()
    {
        //state = AI_STATE.CHASE;
        agent = GetComponent<NavMeshAgent>();
        enemyT = GetComponent<Transform>();
        attackWhenClose = true;
        targetPos = targetObject.transform.position;
        timeAttack = 5f;
        //TEMP RESET TARGET TO 0
        targetPos = new Vector3(0, 0, 0);
    }
    
    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        agent.destination = targetPos;
        switch (state)
        {
            case AI_STATE.CHASE:
                {
                    Debug.Log("Chasing to target");



                    if (CastToPlayer(50f))
                    { 
                    ChangeTarget("Player");
                    state = AI_STATE.ACTIVECHASE;
                    }
                break; 
                }
            case AI_STATE.ACTIVECHASE:
                {
                    ChangeTarget("Player");
                    Debug.Log("ACTIVELY Chasing");
                    if(CastToPlayer(4f))
                    {
                        state= AI_STATE.ATTACK;
                    }
                    else if(!CastToPlayer(20f)&&doSearch)
                    {
                        state = AI_STATE.CHASE;
                    }
                    break;
                }
            case AI_STATE.FLEE:
        
                Debug.Log("Fleeing");
                Vector3 diff = new Vector3();
                diff = enemyT.position- targetPos;
                diff.Normalize();
                diff *= 15;

                targetPos = targetPos+ diff;
                StartCoroutine(FleeTimer());
                state = AI_STATE.ACTIVECHASE;
                break;
        
            case AI_STATE.WAIT:
        
                Debug.Log("Waiting");
                targetPos = enemyT.position;

                break;
        
            case AI_STATE.ATTACK:
                
                if(CheckDistance()<ADT*.9f)
                {
                    agent.velocity = Vector3.zero;
                    AIAttack();
                }
                else if (!CastToPlayer(ADT * 1.1f) && CheckDistance()<ADT)
                {
                    Debug.Log("They Ran Away!)");
                    agent.velocity = Vector3.one;

                    state = AI_STATE.CHASE;
                }

                /*else
                {
                    Debug.Log("Target changed to player");
                    ChangeTarget("Player");
                }*/



                //state = AI_STATE.CHASE;
                break;
        
            default:
                Debug.Log("NOTHING)");
                break;
        }  
    }

    public int CheckState() ///method to return the current state
    {
        return (int)state;
    }
    public bool CastToPlayer(float distance)
    {
        Debug.DrawRay(enemyT.position, (2 * enemyT.position) - CheckTarget("Player"), Color.green);
        RaycastHit hit;
        if (Physics.Raycast(enemyT.position, CheckTarget("Player") - enemyT.position, out hit, distance, ~0))
        {
            if (hit.transform.CompareTag("Player"))
            {
                print("Player hit raycasted");
                return true;
            }
        }
        print("Player Not HIT");
        return false;
    }

    public float CheckDistance() //method to return the distance between the target and the current position
    {
        return Mathf.Abs(Vector3.Magnitude(enemyT.position - targetPos));
    }

    public void ChangeTarget()
    {
        
    }
    public void ChangeTarget(string theTag)
    {
        //change the AI target based on the tag
        targetObject = GameObject.FindWithTag(theTag);
        Vector3 tempPos = targetObject.GetComponent<Transform>().position;
        targetPos = tempPos;
    }
    public Vector3 CheckTarget(string theTag)
    {
        //return the AI target POSITION based on the tag
        return GameObject.FindWithTag(theTag).GetComponent<Transform>().position;
    }
    void AIChasePlayer()
    {
        //moves towards the target

    }

    IEnumerator FleeTimer()
    {
        yield return new WaitForSeconds(5f);
        state = AI_STATE.ATTACK;
    }
    
    void AIWait()
    {
        //pretty much stays still
    }
    void AIAttack()
    {
       
        //activates attack, refresh on attack most likely stored in here
        if(canAttack)
        {
            Debug.Log("Attacking");
            StartCoroutine(ResetAttack());
            canAttack = false;
            doAttack = true;
            //DO ATTACK HERE
        }
        //attacks themselves will be in seperate scripts for enemies
    }
    IEnumerator ResetAttack()
    {
        
            yield return new WaitForSeconds(timeAttack);
            canAttack = true;
        
    }


}

