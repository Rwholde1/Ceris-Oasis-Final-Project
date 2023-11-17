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
    [SerializeField] AI_STATE state;
    private Transform enemyT;
    private float speed = 1f;

    GameObject targetObject;

    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 targetPos;
    void Start()
    {
        state = AI_STATE.CHASE;
        agent = GetComponent<NavMeshAgent>();
        enemyT = GetComponent<Transform>();
    }
    
    public int CheckState()
    {
        return (int)state;
    }
    float CheckDistance()
    {
    return 0f;
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
                break; 
                }
            case AI_STATE.ACTIVECHASE:
                {
                    Debug.Log("ACTIVELY Chasing");

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
                state = AI_STATE.CHASE;
                break;
        
            case AI_STATE.WAIT:
        
                Debug.Log("Waiting");
                targetPos = enemyT.position;

                break;
        
            case AI_STATE.ATTACK:
        
                Debug.Log("Attacking");
                Debug.Log("Right now changes state to chase player");
                ChangeTarget("Player");
                state = AI_STATE.CHASE;
                break;
        
            default:
                Debug.Log("NOTHING)");
                break;
        }  
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

