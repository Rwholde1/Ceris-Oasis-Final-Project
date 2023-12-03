using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityManager : MonoBehaviour
{

    public Slider slider1;
    public Slider slider2;

    private float cooldownMax1;
    private float cooldownMax2;

    private float cooldown1;
    private float cooldown2;

    private bool lockedout1 = false;
    private bool lockedout2 = false;
    
    public TextMeshProUGUI counter1;
    public TextMeshProUGUI counter2;

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
        //Ability 1 Available
        if (!lockedout1) {
            counter1.text = "";
            //Ability 1 Trigger
            if(Input.GetKeyDown(KeyCode.Q)) {
                lockedout1 = true;
                slider1.value = 0;
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
                cooldown1 -= Time.deltaTime;
                counter1.text = "" + Mathf.Ceil(cooldown1);
                slider1.value = cooldownMax1 - cooldown1;
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
    
        
    }
}

