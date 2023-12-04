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
        DEAD = 5,
    }

    
    public float ADT; //Attack Distance Threshold


    [SerializeField] public AI_STATE state = AI_STATE.ACTIVECHASE;
    private Transform enemyT;

    private GameObject playerObject;
    private GameObject[] playerArray;
    private int playerLiveCount;

    public GameObject targetObject;

    public float attackDamageModifier;
    public float timeAttack;
    public bool canAttack;
    public bool doAttack;
    public bool doSearch;
    public bool isAnimating;

    

    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 targetPos;

    void Start()
    {

        attackDamageModifier = 1f;
        canAttack = true;
        doAttack = false;
        doSearch = true;
        isAnimating = false;
        //state = AI_STATE.CHASE;
        agent = GetComponent<NavMeshAgent>();
        enemyT = GetComponent<Transform>();
        //targetPos = targetPos;
        timeAttack = 5f;
        //TEMP RESET TARGET TO 0
        //targetPos = new Vector3(0, 0, 0);
        //ChangeTarget("Player");
        state = AI_STATE.ACTIVECHASE;
        playerObject = getRandomFromAllPlayer();
        targetObject = playerObject;
    }
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckIfPlayerDiedUpdate(playerObject);
        agent.destination = targetPos;
        switch (state)
        {
            case AI_STATE.CHASE:
                {
                    Debug.Log("Chasing to RANDOM target");

                    if (CastToPlayer(50f))
                    { 
                    ChangeTarget(targetObject);
                    state = AI_STATE.ACTIVECHASE;
                    isAnimating = false;
                    }
                break; 
                }
            case AI_STATE.ACTIVECHASE:
                {
                    ChangeTarget(targetObject);
                    Debug.Log("ACTIVELY Chasing");
                    if(CastToPlayer(4f))
                    {
                        state= AI_STATE.ATTACK;
                        isAnimating = false;
                    }
                    else if(!CastToPlayer(20f)&&doSearch)
                    {
                        state = AI_STATE.CHASE;
                        isAnimating = false;
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
                ChangeSpeed(0);
                targetPos = enemyT.position;

                break;
        
            case AI_STATE.ATTACK:
                
                if(CheckDistance()<=ADT*1.1f)
                {
                    Debug.Log("ACTIVE ATTACK STATE)");
                    ChangeSpeed(0);
                    AIAttack();
                }
                else if (!CastToPlayer(ADT * 1.2f) && CheckDistance()>=ADT*2)
                {
                    Debug.Log("They Ran Away!)");
                    ChangeSpeed(1);

                    state = AI_STATE.CHASE;
                    isAnimating = false;
                }
                break;
            case AI_STATE.DEAD:
                {
                    ChangeSpeed(0);
                    targetPos = enemyT.position;
                    StartCoroutine(KilledRIP());

                    break;
                }
            default:
                Debug.Log("NOTHING)");
                break;
        }  
    }
    public GameObject getRandomFromAllPlayer()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Player");
        playerLiveCount = playerArray.Length;
        
        int randPick = Random.Range(0, playerLiveCount-1);
        return playerArray[randPick];
    }
    public int CheckState() ///method to return the current state
    {
        return (int)state;
    }
    public bool CastToPlayer(float distance)
    {
        Debug.DrawRay(enemyT.position, CheckTarget(targetObject) - enemyT.position, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(enemyT.position, CheckTarget(targetObject) - enemyT.position, out hit, distance, ~0))
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

    public void ChangeSpeed(float newSpeed)
    {
        if (!(newSpeed == 0f))
        {
            agent.velocity = Vector3.one * 3.5f * newSpeed;
        }
        else
        {
            agent.velocity = Vector3.zero;
        }

    }
    public float CheckDistance() //method to return the distance between the target and the current position
    {
        return Mathf.Abs(Vector3.Magnitude(enemyT.position - targetPos));
    }

    //Probably Redundant
    public void ChangeTarget(GameObject theObject)
    {
        targetObject = theObject;
        
        targetPos = targetObject.GetComponent<Transform>().position;
    }
    public void ChangeTarget(string theTag)
    {
        //change the AI target based on the tag
        targetObject = GameObject.FindWithTag(theTag);
        Vector3 tempPos = targetObject.GetComponent<Transform>().position;
        targetPos = tempPos;
    }
    public void CheckIfPlayerDiedUpdate(GameObject obj)
    {
        if (!obj)
        {
            isAnimating = false;
            playerObject = getRandomFromAllPlayer();
            ChangeTarget(playerObject);
        }
    }
    public Vector3 CheckTarget(string theTag)
    {
        //return the AI target POSITION based on the tag
        return GameObject.FindWithTag(theTag).GetComponent<Transform>().position;
    }
    public Vector3 CheckTarget(GameObject obj)
    {
        //return the AI target POSITION based on the tag
        
        return obj.GetComponent<Transform>().position;
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
    IEnumerator KilledRIP()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}

