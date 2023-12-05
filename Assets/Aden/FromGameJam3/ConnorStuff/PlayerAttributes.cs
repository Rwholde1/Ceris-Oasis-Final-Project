/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float MaxHealth = 100f;
    [SerializeField] private float health = 100f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Damage(float damage)
    {
        health -= damage;
    }

    public void IncreaseMaxHealth(float maxIncrease)
    {
        MaxHealth += maxIncrease;
        health += maxIncrease;
    }
}
*/
