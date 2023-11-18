using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    public Slider shieldSlider;

    public void SetMaxValue(int value)
    {
        shieldSlider.maxValue = value;
        shieldSlider.value = value;
    }
    
    public void SetValue(float value)
    {
        shieldSlider.value = value;
    }
}
