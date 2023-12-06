using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile1 : MonoBehaviour
{
    [SerializeField] float damage;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("YEOWCH");
            PlayerAttributes pAt;
            pAt = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
            pAt.Damage(damage);
            Destroy(gameObject);
        }
    }

}
