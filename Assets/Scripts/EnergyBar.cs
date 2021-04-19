using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    /// <summary>
    /// Amount of energy as a float with 0 meaning no energy and 1 being the maximum
    /// </summary>
    public float Energy
    {
        get => slider.value;
        set => slider.value = value;
    }

    [SerializeField]
    private Slider slider;

    private void Start()
    {
        slider.value = slider.maxValue;
    }
}
