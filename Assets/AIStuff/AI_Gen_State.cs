using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Gen_State : MonoBehaviour
{
    public enum AI_STATE
    {
        CHASE = 1,
        FLEE = 2,
        WAIT = 0,
        ATTACK = 3
    }
    [SerializeField] AI_STATE state;
    private Transform enemyT;
    private float speed = 1f;
    void Start()
    {
        state = AI_STATE.CHASE;
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
        switch(state)
    {
        case AI_STATE.CHASE:
            Debug.Log("Chasing");
            break;
        
        case AI_STATE.FLEE:
        
            Debug.Log("Fleeing");
            break;
        
        case AI_STATE.WAIT:
        
            Debug.Log("Waiting");
            break;
        
        case AI_STATE.ATTACK:
        
            Debug.Log("Attacking");
            break;
        
        default:
            Debug.Log("NOTHING)");
            break;
    }  
    }
    public void ChangeTarget()
    {
        //change the AI target
    }
    void AIChasePlayer()
    {
        //moves towards the target
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

