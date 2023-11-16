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
    public StatsManager stats;

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
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            stats.addDeath(1);
            currentHealth = maxHealth;
            healthBar.setHealth(maxHealth);
        }

        if(Input.GetKeyDown(KeyCode.J)) {
            stats.addElim(1);
        }
        if(Input.GetKeyDown(KeyCode.K)) {
            stats.addAssist(1);
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            takeDamage(10);
        }
        if(Input.GetMouseButtonDown(1)) {
            var dmg = Mathf.Floor(GetComponent<Transform>().localEulerAngles.y);
            stats.addDamage(1, (int) dmg);
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
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            //Sets height offset and instantiates ping
            Vector3 offset = new Vector3 (hit.point.x, hit.point.y + 0.1f, hit.point.z);
            Instantiate(basicPing, offset, Quaternion.identity);
        }
    }

}
