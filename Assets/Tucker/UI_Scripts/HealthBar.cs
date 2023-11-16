using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    
    //Sets healthbar slider to health in
    public void setHealth(int healthIn) {
        slider.value = healthIn;
    }

    //Sets healthbar slider max, brings health up to that new max
    public void setMaxHealth (int maxHealthIn) {
        slider.maxValue = maxHealthIn;
        setHealth(maxHealthIn);
    }
}
