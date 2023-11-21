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


    [SerializeField] AI_STATE state;
    private Transform enemyT;
    private float speed = 1f;

    GameObject targetObject;

    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 targetPos;
    void Start()
    {
        //state = AI_STATE.CHASE;
        agent = GetComponent<NavMeshAgent>();
        enemyT = GetComponent<Transform>();
        attackWhenClose = true;
        targetPos = targetObject.transform.position;
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

                    if (CastToPlayer())
                    { 
                    ChangeTarget("Player");
                    state = AI_STATE.ACTIVECHASE;
                    }
                break; 
                }
            case AI_STATE.ACTIVECHASE:
                {
                    Debug.Log("ACTIVELY Chasing");
                    if(CheckDistance()<= ADT)
                    {
                        state= AI_STATE.ATTACK;
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
                if (CheckDistance() >= ADT && !CastToPlayer())
                {
                    state = AI_STATE.CHASE;
                }
                Debug.Log("Attacking");
                Debug.Log("Right now changes state to chase player");
                AIAttack();

                //Updates player pos
                ChangeTarget("Player");
                
                
                
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
    public bool CastToPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemyT.position, (2*enemyT.position) - CheckTarget("Player"), out hit, 50f, ~0))
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

    float CheckDistance() //method to return the distance between the target and the current position
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
        //attacks themselves will be in seperate scripts for enemies
    }



}

