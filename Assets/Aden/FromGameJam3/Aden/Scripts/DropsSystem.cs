using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float MaxHealth = 100f;
    [SerializeField] private float health = 100f;
    [SerializeField] private GameObject AttackRatePowerup;
    [SerializeField] private GameObject AttackDamagePowerup;
    [SerializeField] private GameObject HealthPowerup;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            int randomNumber = Random.Range(0, 12);
            if (randomNumber == 0)
            {
                AttackRateDrop();
            }
            if (randomNumber == 4)
            {
                AttackDamageDrop();
            }
            if (randomNumber == 8)
            {
                HealthDrop();
            }

            Destroy(gameObject);


        }

    }
    public void Damage(float damage)
    {
        health -= damage;
    }

    public void AttackRateDrop()
    {
        Debug.Log("I need more Boullets");
        Vector3 position = transform.position; 
        GameObject ARPowerup = Instantiate(AttackRatePowerup, position, Quaternion.identity);
        ARPowerup.SetActive(true);
        Destroy(ARPowerup, 7.0f);
    }
    public void AttackDamageDrop()
    {
        Debug.Log("Damage boost");
        Vector3 position = transform.position;
        GameObject ADPowerup = Instantiate(AttackDamagePowerup, position, Quaternion.identity);
        ADPowerup.SetActive(true);
        Destroy(ADPowerup, 7.0f);
    }
    public void HealthDrop()
    {
        Debug.Log("I need more healing");
        Vector3 position = transform.position;
        GameObject HPPowerup = Instantiate(HealthPowerup, position, Quaternion.identity);
        HPPowerup.SetActive(true);
        Destroy(HPPowerup, 7.0f);
    }
}
