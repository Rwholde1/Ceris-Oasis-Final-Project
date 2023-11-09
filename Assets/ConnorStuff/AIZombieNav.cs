using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIZombieNav : MonoBehaviour
{
    
    public enum AI_States
    {
        WANDER,
        CHASE,
        ATTACK,
    }
    [SerializeField] private Transform goal;
    float distance;
    [SerializeField] AI_States state = AI_States.WANDER;
    [SerializeField] private float threshold = 0f;
    private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] GameObject attack;
    [SerializeField] bool canAttack = true;
    [SerializeField] bool canRelocate = true;
    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        distance = Vector3.Distance(transform.position, goal.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, goal.position);
        if (state == AI_States.CHASE)
        {


            agent.destination = goal.position;

            if (distance <= threshold)
            {
                
                state = AI_States.ATTACK;
            }
            if (distance >= threshold * 4)
            {
                state = AI_States.WANDER;
            }
        }
        if (state == AI_States.WANDER)
        {
            if (canRelocate)
            {
                canRelocate = false;
                agent.destination = new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5)) + transform.position;
                StartCoroutine(GetNewPosition());
            }
            if (distance <= 5)
            {
                state = AI_States.CHASE;
            }

        }
        if (state == AI_States.ATTACK)
        {
            agent.destination = transform.position;
            var lookPos = goal.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
            if (canAttack)
            {
                Debug.Log("Attack!");
                canAttack = false;
                GameObject newProjectile = Instantiate(attack, transform.position, Quaternion.identity);
                newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
                StartCoroutine(RefreshAttack());
                Destroy(newProjectile, .5f);
            }
            if (distance >= threshold)
            {
                state = AI_States.CHASE;
            }
            
        }

    }
    IEnumerator GetNewPosition()
    {
        yield return new WaitForSeconds(5f);
        canRelocate = true;
    }
    IEnumerator RefreshAttack()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
}
