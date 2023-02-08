using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Slider Bar;

    public void UpdateBar(int value)
    {
        Bar.value = value;
    }

    public void SetMaxValue(int value)
    {
        Bar.maxValue = value;
    }
}
