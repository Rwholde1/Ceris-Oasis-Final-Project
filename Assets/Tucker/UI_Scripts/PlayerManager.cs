using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    private bool alive = true;
    [SerializeField] object primaryWeapon;
    [SerializeField] object secondaryWeapon;
    public HealthBar healthBar;

    //grenades
    //shielding?


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            alive = false;
        }


        if(Input.GetKeyDown(KeyCode.K)) {
            takeDamage(10);
        }

    }

    void takeDamage(int damageIn) {
        currentHealth -= damageIn;
        healthBar.setHealth(currentHealth);
    }

}
