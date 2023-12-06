using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityManager : MonoBehaviour
{

    public Slider slider1;
    public Slider slider2;

    public Image icon1;
    public Image icon2;

    private float cooldownMax1;
    private float cooldownMax2;

    private float cooldown1;
    private float cooldown2;

    private bool lockedout1 = false;
    private bool lockedout2 = false;

    public int abilIndex = -1;
    
    public TextMeshProUGUI counter1;
    public TextMeshProUGUI counter2;

    public MolotovThrower pirateAbility1;
    public AkaneExample boomerAbility1;
    public HealingZoneCaster alienAbility1;
    public Jetpack commandoAbility1;

    public Sprite[] abilityIcons = new Sprite[4];

    public Sprite thisIcon;

    void Start() {
        //May need to be moved to update or something
        //Fetches boomerang thrower
        boomerAbility1 = LobbySceneManagement.singleton.getLocalPlayer().GetComponentInChildren<AkaneExample>();
        commandoAbility1 = LobbySceneManagement.singleton.getLocalPlayer().GetComponentInChildren<Jetpack>();
        checkCooldowns();
        if (commandoAbility1.enabled) {
            //cooldownMax1 = 20f;
            setCooldown1(99);
        }

        counter1.text = "";
    }

    //Sets ability 1's cooldown slider max
    public void setCooldown1 (int cooldownIn) {
        cooldownMax1 = (float) cooldownIn;
        cooldown1 = cooldownMax1;
        slider1.maxValue = cooldownMax1;
        slider1.value = cooldownMax1;
    }

    //Sets ability 2's cooldown slider max
    public void setCooldown2 (int cooldownIn) {
        cooldownMax2 = (float) cooldownIn;
        cooldown2 = cooldownMax2;
        slider2.maxValue = cooldownMax2;
        slider2.value = cooldownMax2;
    }

    void Update() {

        if (thisIcon == null) {
            icon1.enabled = false;
            checkCooldowns();
        }
        //Jetpack
        if (commandoAbility1.enabled) {
            Debug.Log("has jets");
            float rawFuel = commandoAbility1.currentFuel;
            int fuel = (int) Mathf.Floor(rawFuel);
            if (fuel > 99) { fuel = 99; }
            cooldown1 = fuel;
            //counter1.text = "" + cooldown1;
            slider1.value = cooldown1;
        }

        //Ability 1 Available
        if (!lockedout1 && !commandoAbility1.enabled) {
            counter1.text = "";
            //Ability 1 Trigger
            if(Input.GetKeyDown(KeyCode.Q)) {
                checkCooldowns();
                lockedout1 = true;
                slider1.value = 0;
                if (pirateAbility1.enabled) {
                    pirateAbility1.ThrowGrenade();
                }
                if (boomerAbility1.enabled)  {
                    boomerAbility1.throwBoomer();
                }
                if (alienAbility1.enabled)  {
                    alienAbility1.CastHeals();
                }
            }
        //Ability 1 On Cooldown
        } else {
            //Cooldown ends
            if (cooldown1 <= 0) {
                lockedout1 = false;
                cooldown1 = cooldownMax1;
                counter1.text = "";
            //Coolding down
            } else {
                if (!commandoAbility1.enabled) {
                    cooldown1 -= Time.deltaTime;
                    counter1.text = "" + Mathf.Ceil(cooldown1);
                    slider1.value = cooldownMax1 - cooldown1;
                }

            }
        }

        //Ability 2 Available
        if (!lockedout2) {
            counter2.text = "";
            //Ability 2 Trigger
            if(Input.GetKeyDown(KeyCode.E)) {
                lockedout2 = true;
                slider2.value = 0;
            }
        //Ability 2 On Cooldown
        } else {
            //Cooldown ends
            if (cooldown2 <= 0) {
                lockedout2 = false;
                cooldown2 = cooldownMax2;
                counter2.text = "";
            //Coolding down
            } else {
                cooldown2 -= Time.deltaTime;
                counter2.text = "" + Mathf.Ceil(cooldown2);
                slider2.value = cooldownMax2 - cooldown2;
            }
        }

        if (counter1.text == "") {
            icon1.enabled = true;
            icon1.sprite = thisIcon;
        } else {
            //icon1.enabled = false;
        }
        
    }

    public void checkCooldowns() {
        //Changes cooldown caps
        if (boomerAbility1.enabled) {
            //cooldownMax1 = 10f;
            setCooldown1(10);
            abilIndex = 2;
        }
        if (pirateAbility1.enabled) {
            //cooldownMax1 = 15f;
            setCooldown1(15);
            abilIndex = 3;
        }
        if (alienAbility1.enabled) {
            //cooldownMax1 = 20f;
            setCooldown1(20);
            abilIndex = 0;
        }
        if (commandoAbility1.enabled) {
            abilIndex = 1;
        }
        thisIcon = abilityIcons[abilIndex];
    }
}

