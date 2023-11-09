using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBasicNav : MonoBehaviour
{
    public enum AI_States{
        WANDER,
        CHASE,
        ATTACK,
        RETREAT
    }

    [SerializeField] private Transform goal;
    
    [SerializeField] private float threshold = 5.0f;
    [SerializeField] private float power = 1.0f;
    private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] AI_States state = AI_States.WANDER;
    [SerializeField] private bool canAttack = true;
    float distance;
    [SerializeField] GameObject attack;
    [SerializeField] private bool canRelocate = true;

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

            if(distance <= threshold)
            {
                //agent.destination = transform.position;
                state = AI_States.ATTACK;
            }
            if(distance >= threshold*4)
            {
                state = AI_States.WANDER;
            }
        }
        if (state == AI_States.ATTACK)
        {
            agent.destination = transform.position;
            var lookPos = goal.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation , Time.deltaTime*2);
            if (canAttack)
            {
                Debug.Log("Attack!");
                canAttack = false;
                GameObject newProjectile = Instantiate(attack, transform.position, Quaternion.identity);
                newProjectile.GetComponent<Rigidbody>().AddForce((transform.forward + new Vector3(0,.35f,0))* power, ForceMode.Impulse);
                StartCoroutine(RefreshAttack());
                Destroy(newProjectile, 1);
            }
            if(distance>= threshold)
            {
                state = AI_States.CHASE;
            }
            if (distance <= threshold/2)
            {
                state = AI_States.RETREAT;
            }
        }
        if (state == AI_States.RETREAT)
        {

            agent.destination = transform.position + transform.position - goal.position;
            if(distance>= threshold*4)
            {
                state = AI_States.WANDER;
            }
        }
        if(state == AI_States.WANDER)
        {
            if (canRelocate)
            {
                canRelocate = false;
                agent.destination = new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5))+ transform.position;
                StartCoroutine(GetNewPosition());
            }
            if(distance<= threshold*3)
            {
                state = AI_States.CHASE;
            }

        }
    }

    IEnumerator RefreshAttack()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    IEnumerator GetNewPosition()
    {
        yield return new WaitForSeconds(5f);
        canRelocate = true;
    }



}
