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
    private float speedMod;
    private GameObject playerObject;
    private RegisterPlayer[] playerArray;
    private int playerLiveCount;

    public GameObject targetObject;

    public float attackDamageModifier = 1f;
    public float timeAttack;
    public bool canAttack;
    public bool doAttack;
    public bool doSearch;
    public bool isAnimating;

    public float ADTMod = 1.01f;

    public EnemyHitRegister hitReg;
    public AI_GunnerScript gunScript;
    public AI_RusherScript rushScript;
    public AI_LobberScript lobScript;
    public AI_WandererScript wanderScript;
    public AI_QueenScript queenScript;

    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 targetPos;

    public int targetID = -3;

    void Start()
    {
        if (!LobbySceneManagement.singleton.getLocalPlayer().getIsServer()) {
            Invoke("killClientBrain", 0.5f);
        } else {
            speedMod = 1f;
            // /attackDamageModifier = 1f;
            canAttack = true;
            doAttack = false;
        
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
    }
    
    
    // Update is called once per frame
    void Update()
    {   
        if (LobbySceneManagement.singleton.getLocalPlayer().getIsServer()) {
            CheckIfPlayerDiedUpdate(playerObject);
            agent.destination = targetPos;
            switch (state)
            {
                case AI_STATE.CHASE:
                {
                    //Debug.Log("Chasing to RANDOM target");

                    if (CastToPlayer(50f) && !targetObject.GetComponent<RegisterPlayer>().isDead)
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
                    //Debug.Log("ACTIVELY Chasing");
                    if(CastToPlayer(ADT * 1.1f))
                    {
                        state = AI_STATE.ATTACK;
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
                {
                    //Debug.Log("Fleeing");
                    Vector3 diff = new Vector3();
                    diff = enemyT.position- targetPos;
                    diff.Normalize();
                    diff *= 15;

                    targetPos = targetPos+ diff;
                    StartCoroutine(FleeTimer());
                    state = AI_STATE.ACTIVECHASE;
                    break;
                }
        
            case AI_STATE.WAIT:
                {
                    //Debug.Log("Waiting");
                    ChangeSpeed(0);
                    targetPos = enemyT.position;

                    StartCoroutine(FleeTimer());
                    state = AI_STATE.CHASE;
                    break;
                }
        
            case AI_STATE.ATTACK:
                
                if(/*CheckDistance()<=ADT*1.1f*/     CastToPlayer(ADT * 1.2f))
                {
                    //Debug.Log("ACTIVE ATTACK STATE)");
                    ChangeSpeed(0);
                    AIAttack();
                }
                else //if (!CastToPlayer(ADT * 1.2f) && CheckDistance()>=ADT*2)
                {
                    //Debug.Log("They Ran Away!)");
                    ChangeSpeed(1);

                    state = AI_STATE.ACTIVECHASE;
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
                //Debug.Log("NOTHING)");
                break;
            }  
        }
        
    }


    public GameObject getRandomFromAllPlayer()
    {
        //playerArray = GameObject.FindGameObjectsWithTag("Player");
        playerArray = LobbySceneManagement.singleton.players;
        playerLiveCount = playerArray.Length;
        
        int randPick = Random.Range(0, LobbySceneManagement.singleton.statsPlayerId.Count);
        targetID = randPick;
        //Debug.Log("player hunted is " + playerArray[randPick].gameObject);
        return playerArray[randPick].gameObject;
    }

    public GameObject getRandomFromAllPlayer(RegisterPlayer[] players)
    {
        //playerArray = GameObject.FindGameObjectsWithTag("Player");
        playerArray = players;
        playerLiveCount = playerArray.Length;
        
        //int randPick = Random.Range(0, playerLiveCount - 1);
        int randPick = Random.Range(0, LobbySceneManagement.singleton.statsPlayerId.Count);
        targetID = randPick;
        //Debug.Log("player hunted is " + playerArray[randPick].gameObject);
        return playerArray[randPick].gameObject;
    }

    public int CheckState() ///method to return the current state
    {
        return (int)state;
    }

    public bool CastToPlayer(float distance)
    {
        Debug.DrawRay(enemyT.position, CheckTarget(targetObject) - enemyT.position, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(enemyT.position + new Vector3(0f, 0.5f, 0f), CheckTarget(targetObject) - enemyT.position, out hit, distance, ~0))
        {
            if (hit.transform.CompareTag("Player"))
            {
                //print("Player hit raycasted");
                return true;
            }
        }
        //print("Player Not HIT");
        return false;
    }

    public void ChangeSpeed(float newSpeed)
    {
        if (!(newSpeed == 0f))
        {
            agent.velocity = Vector3.one * 3.5f * newSpeed * speedMod;
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


    //Change parameter to handle player death
    public void CheckIfPlayerDiedUpdate(GameObject obj)
    {
        //Debug.Log("checking for target death");
        if (obj.GetComponent<RegisterPlayer>().isDead)
        {
            //Debug.Log("target is dead");
            bool allDead = true;
            RegisterPlayer[] validTargets = new RegisterPlayer[4];
            int i = 0;
            foreach (RegisterPlayer player in LobbySceneManagement.singleton.players) {
                if (player != null && !player.isDead) {
                    allDead = false;
                    validTargets[i] = player;
                    i++;
                }
            }
            if (allDead) {
                state = AI_STATE.WAIT;
            } else {
                isAnimating = false;
                playerObject = getRandomFromAllPlayer(validTargets);
                ChangeTarget(playerObject);
            }
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
            //Debug.Log("Attacking");
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

    public void killClientBrain() {
        //Debug.Log("disabling client AI scripts");
            hitReg.enabled = false;
            if (gunScript != null) {
                gunScript.enabled = false;
            }
            if (rushScript != null) {
                rushScript.enabled = false;
            }
            if (lobScript != null) {
                lobScript.enabled = false;
            }
            if (wanderScript != null) {
                wanderScript.enabled = false;
            }
            if (queenScript != null) {
                queenScript.enabled = false;
            }
    }
}

