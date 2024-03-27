using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkedHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Slider slider;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}
