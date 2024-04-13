using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static float sensitivity = 2.5f;

    public Slider sensitivitySlider;

    void Update()
    {
        sensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat("sensitivity", sensitivity);
        print("stored value: " + PlayerPrefs.GetFloat("sensitivity", 1));
    }
}
