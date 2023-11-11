using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    [SerializeField] int cooldown1 = 10;
    [SerializeField] int cooldown2 = 10;

    private bool alive = true;
    [SerializeField] object primaryWeapon;
    [SerializeField] object secondaryWeapon;
    public HealthBar healthBar;
    public AbilityManager abilities;

    public Camera mainCam;
    public GameObject basicPing;

    //grenades
    //shielding?


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        abilities.setCooldown1(cooldown1);
        abilities.setCooldown2(cooldown2);
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

        if(Input.GetMouseButtonDown(2)) {
            createPing();
        }

    }

    void takeDamage(int damageIn) {
        currentHealth -= damageIn;
        healthBar.setHealth(currentHealth);
    }

    void createPing() {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                    Debug.Log("ping");
        if(Physics.Raycast(ray, out RaycastHit hit)) {
                        Debug.Log("ping1");
            //Sets height offset and instantiates ping
            Vector3 offset = new Vector3 (hit.point.x, hit.point.y + 0.1f, hit.point.z);
            Instantiate(basicPing, offset, Quaternion.identity);

            
            if (hit.collider.gameObject.layer == 8) {
                            Debug.Log("ping2");
                
            }
        }
    }

}
