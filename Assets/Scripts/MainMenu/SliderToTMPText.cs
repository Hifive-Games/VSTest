using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderToTMPText : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public UnityEngine.UI.Slider slider;

    private void Start()
    {
        if (text == null)
        {
            text = GetComponent<TMPro.TMP_Text>();
        }

        if (slider == null)
        {
            slider = GetComponent<UnityEngine.UI.Slider>();
        }
    }

    public void UpdateText(float value)
    {
        value = value * 100;
        text.text = value.ToString("F0");
    }
}
