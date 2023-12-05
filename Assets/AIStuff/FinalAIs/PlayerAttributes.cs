/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float MaxHealth = 100f;
    [SerializeField] private float health = 100f;

    public bool canDamage = true;
    public float invincibleCooldown = 1f;
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
        if (canDamage)
        {
            health -= damage;
            canDamage = false;
            StartCoroutine(InvincibleTime());
        }
    }

    public void IncreaseMaxHealth(float maxIncrease)
    {
        MaxHealth += maxIncrease;
        health += maxIncrease;
    }
    IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(invincibleCooldown);
        canDamage = true;
    }
}
*/
