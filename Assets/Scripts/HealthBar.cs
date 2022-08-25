using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    // Method: SetMaxHealth
    // Purpose: Sets the maximum value on the health bar based on the passed health
    // Restrictions: None
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Method: SetHealth
    // Purpose: Readjusts the slider to the current health that is passed in
    // Restrictions: None
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
